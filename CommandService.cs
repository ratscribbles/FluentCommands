﻿using System;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using FluentCommands.Builders;
using FluentCommands.CommandTypes;
using FluentCommands.Interfaces;
using FluentCommands.Attributes;
using FluentCommands.Exceptions;
using FluentCommands.Menus;
using FluentCommands.Utility;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Runtime.CompilerServices;
using FluentCommands.Logging;
using FluentCommands.Cache;
using Microsoft.Extensions.DependencyInjection;
using FluentCommands.CommandTypes.Steps;
using System.Diagnostics.CodeAnalysis;

[assembly: InternalsVisibleTo("FluentCommands.Tests.Unit")]

namespace FluentCommands
{
    //: Create methods that share internal cache services with outside dbs (EF core and such)

    /// <summary>
    /// The class responsible for handling the assembly and processing of Telegram Bot commands.
    /// </summary>
    public sealed class CommandService
    {
        private static readonly Lazy<IServiceCollection> _services = new Lazy<IServiceCollection>(() => new ServiceCollection());
        private static readonly Lazy<CommandService> _instance = new Lazy<CommandService>(() => new CommandService(_tempCfg));
        private static readonly Lazy<CommandServiceLogger> _defaultLogger = new Lazy<CommandServiceLogger>(() => new CommandServiceLogger());
        private static readonly Lazy<CommandServiceCache> _defaultCache = new Lazy<CommandServiceCache>(() => new CommandServiceCache());
        private static readonly Lazy<EmptyLogger> _emptyLogger = new Lazy<EmptyLogger>(() => new EmptyLogger());
        private static readonly IReadOnlyCollection<Type> _assemblyTypes;
        private static readonly IReadOnlyCollection<Type> _telegramEventArgs = new HashSet<Type> { typeof(CallbackQueryEventArgs), typeof(ChosenInlineResultEventArgs), typeof(InlineQueryEventArgs), typeof(MessageEventArgs), typeof(UpdateEventArgs) };
        private static readonly Dictionary<Type, ModuleBuilder> _tempModules = new Dictionary<Type, ModuleBuilder>();
        private static Toggle _lastMessageIsMenu = new Toggle(false); //: Probably delete
        private static ToggleOnce _commandServiceStarted = new ToggleOnce(false);
        private static CommandServiceConfigBuilder _tempCfg = new CommandServiceConfigBuilder();
        ///////
        private readonly CommandServiceConfig _config;
        private readonly IReadOnlyDictionary<Type, IReadOnlyModule> _modules;
        private readonly IReadOnlyDictionary<Type, IReadOnlyDictionary<ReadOnlyMemory<char>, ICommand>> _commands;
        private readonly IFluentDatabase? _customDatabase;
        private readonly IFluentLogger? _customLogger;
        ///////
        private static IReadOnlyDictionary<Type, IReadOnlyDictionary<ReadOnlyMemory<char>, ICommand>> Commands => _instance.Value._commands;
        internal static IReadOnlyDictionary<Type, IReadOnlyModule> Modules => _instance.Value._modules;
        internal static IFluentDatabase Cache
        {
            get
            {
                if (GlobalConfig.UsingCustomDatabase) return _instance.Value._customDatabase!; // Not null if true
                else return _defaultCache.Value;
            }
        }
        internal static IFluentLogger Logger
        {
            get
            {
                if (GlobalConfig.DisableLogging)
                {
                    if (GlobalConfig.UsingCustomLogger) return _instance.Value._customLogger!; // Not null if true
                    else return _defaultLogger.Value;
                }
                else return _emptyLogger.Value;
            }
        }
        internal static CommandServiceConfig GlobalConfig => _instance.Value._config;

        #region Constructors
        /// <summary>
        /// Guarantees the Assembly Types are cached before the CommandService starts.
        /// </summary>
        static CommandService()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> assemblyTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                List<Type> internalTypes;

                try { internalTypes = assembly.GetTypes().ToList(); }
                catch (ReflectionTypeLoadException e) { internalTypes = e.Types.Where(type => !(type is null)).ToList(); }

                assemblyTypes.AddRange(internalTypes);
            }

            _assemblyTypes = assemblyTypes;
        }

        /// <summary>
        /// Constructor for use only with the singleton. Enforces that internal collectons are completely unable to be modified and are read-only. Populates the following:
        /// <para>- Modules readonly dictionary</para>
        /// <para>- Commands readonly dictionary</para>
        /// <para>- Global config object</para>
        /// </summary>
        private CommandService(CommandServiceConfigBuilder cfg) 
        {
            _config = new CommandServiceConfig(cfg);

            if (_services.IsValueCreated)
            {
                var provider = _services.Value.BuildServiceProvider();
                _customDatabase = provider.GetService<IFluentDatabase>();
                _customLogger = provider.GetService<IFluentLogger>();
            }

            var tempCommands = new Dictionary<Type, Dictionary<ReadOnlyMemory<char>, ICommand>>();

            //: create an object that stores logging "events" and pops em at the end. mayb.
            
            Init_1_ModuleAssembler();
            Init_2_KeyboardAssembler();
            Init_3_CommandAssembler();

            var tempModulesToReadOnly = new Dictionary<Type, IReadOnlyModule>(_tempModules.Count);
            foreach (var kvp in _tempModules.ToList()) tempModulesToReadOnly.Add(kvp.Key, new ReadOnlyCommandModule(kvp.Value));

            var tempCommandsToReadOnly = new Dictionary<Type, IReadOnlyDictionary<ReadOnlyMemory<char>, ICommand>>();
            foreach (var kvp in tempCommands.ToList()) tempCommandsToReadOnly.Add(kvp.Key, kvp.Value);

            _modules = tempModulesToReadOnly;
            _commands = tempCommandsToReadOnly;

            void Init_1_ModuleAssembler()
            {
                /* Description:
                  *
                  * Attempts to collect the user's CommandModules and assemble them based on their OnBuilding methods.
                  * Fails if any exception is thrown. Only detects modules that inherit from CommandModule<> directly.
                  */

                // Collects *every* ModuleBuilder command context (all classes that derive from CommandContext)
                var allCommandContexts = _assemblyTypes
                    .Where(type => !(type.BaseType is null)
                        && type.BaseType.IsAbstract
                        && type.BaseType.IsGenericType
                        && type.BaseType.GetGenericTypeDefinition() == typeof(CommandModule<>))
                    .ToList();

                string unexpected = "An unexpected error occurred while building command module: ";

                if (allCommandContexts is null) throw new CommandOnBuildingException(unexpected + "Collection of command contexts was null. Please submit a bug report and/or contact the creator of this library if this issue persists.");

                foreach (var context in allCommandContexts)
                {
                    object moduleContext;
                    try { moduleContext = Activator.CreateInstance(context) ?? throw new CommandOnBuildingException(); }
                    catch (MissingMethodException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (ArgumentNullException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (ArgumentException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (NotSupportedException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetInvocationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (MethodAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (MemberAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (System.Runtime.InteropServices.InvalidComObjectException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (System.Runtime.InteropServices.COMException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TypeLoadException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    PropertyInfo property;
                    try { property = context.GetProperty("CommandClass", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new CommandOnBuildingException(); }
                    catch (AmbiguousMatchException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (ArgumentNullException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    MethodInfo method_OnBuilding;
                    try { method_OnBuilding = context.GetMethod("OnBuilding", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new CommandOnBuildingException(); }
                    catch (AmbiguousMatchException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (ArgumentNullException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    MethodInfo method_OnConfiguring;
                    try { method_OnConfiguring = context.GetMethod("OnConfiguring", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new CommandOnBuildingException(); }
                    catch (AmbiguousMatchException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (ArgumentNullException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    Type commandClass;
                    try { commandClass = (Type?)property.GetValue(moduleContext, null) ?? throw new CommandOnBuildingException(unexpected); }
                    catch (ArgumentException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetParameterCountException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (MethodAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetInvocationException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    var moduleBuilder = new ModuleBuilder(commandClass);
                    var moduleConfigBuilder = new ModuleConfigBuilder();

                    // Modules! Assemble!
                    try
                    {
                        method_OnBuilding.Invoke(moduleContext, new object[] { moduleBuilder });
                        if (moduleBuilder is null) throw new CommandOnBuildingException(); //: describe in detail
                    }
                    catch (TargetException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (ArgumentException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetInvocationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetParameterCountException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (MethodAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (InvalidOperationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (NotSupportedException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    try
                    {
                        method_OnConfiguring.Invoke(moduleContext, new object[] { moduleConfigBuilder });
                        if (moduleConfigBuilder is null) throw new CommandOnBuildingException(); //: describe in detail
                    }
                    catch (TargetException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (ArgumentException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetInvocationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetParameterCountException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (MethodAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (InvalidOperationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (NotSupportedException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    moduleBuilder.SetConfig(new ModuleConfig(moduleConfigBuilder));

                    UpdateBuilderInTempModules(moduleBuilder, commandClass);

                    foreach (var commandName in moduleBuilder.ModuleCommandBases.Keys)
                    {
                        AuxiliaryMethods.CheckCommandNameValidity(commandName);
                    }
                }
            }
            void Init_2_KeyboardAssembler() 
            {
                /* Description:
                  *
                  * Attempts to update every defined keyboard in every CommandBaseBuilder contained within each ModuleBuilder.
                  * Runs an init-exclusive version of the UpdateKeyboardRows method to properly setup the _tempModules dictionary.
                  */

                foreach (var kvp in _tempModules.ToList())
                {
                    var commandClass = kvp.Key;
                    var module = kvp.Value;

                    foreach (var moduleKvp in module.ModuleCommandBases.ToList())
                    {
                        var commandName = moduleKvp.Key;
                        var commandBase = moduleKvp.Value;

                        if (commandBase.KeyboardInfo is null) continue;
                        else
                        {
                            if (commandBase.KeyboardInfo.InlineRows.Any()) commandBase.KeyboardInfo.UpdateInline(UpdateKeyboardRows_InitialBeforeAssigningModuleProperty(commandBase.KeyboardInfo.InlineRows, commandClass, commandName));
                            if (commandBase.KeyboardInfo.ReplyRows.Any()) commandBase.KeyboardInfo.UpdateReply(UpdateKeyboardRows_InitialBeforeAssigningModuleProperty(commandBase.KeyboardInfo.ReplyRows, commandClass, commandName));

                            _tempModules[commandClass][commandName] = commandBase;
                        }
                    }
                }

                static List<TButton[]> UpdateKeyboardRows_InitialBeforeAssigningModuleProperty<TButton>(List<TButton[]> rows, Type? parentModule = null, string? parentCommandName = null)
                    where TButton : IKeyboardButton
                {
                    List<TButton[]> updatedKeyboardBuilder = new List<TButton[]>();
                    Type buttonType = typeof(TButton);
                    ModuleConfig? config = _tempModules[parentModule ?? throw new InvalidKeyboardRowException("Error updating keyboard rows. (Keyboard belonged to a Command, but the Command's module type was null.")]?.Config
                            ?? throw new CommandOnBuildingException($"Module: \"{parentModule.FullName ?? "NULL"}\" config was null while building commands.");
                    Func<string, Exception> keyboardException = (string s) => { return new CommandOnBuildingException(s); };
                    string keyboardContainer = $"Command \"{parentCommandName ?? "NULL"}\"";

                    foreach (var row in rows)
                    {
                        var updatedKeyboardButtons = new List<TButton>();

                        foreach (var button in row)
                        {
                            if (button is { }
                                && button.Text is { }
                                && button.Text.Contains("COMMANDBASEBUILDERREFERENCE"))
                            {
                                var match = FluentRegex.CheckButtonReference.Match(button.Text);
                                if (!match.Success)
                                {
                                    match = FluentRegex.CheckButtonLinkedReference.Match(button.Text);
                                    if (!match.Success) throw keyboardException($"Unknown error occurred while building {keyboardContainer} keyboard(s): button contained reference text \"{button.Text}\"");
                                    else UpdateButton(match, true);
                                }
                                else UpdateButton(match);

                                // Locates the reference being pointed to by this TButton and updates it.
                                void UpdateButton(Match m, bool isLinked = false)
                                {
                                    IKeyboardButton? referencedButton;

                                    string commandNameReference = m.Groups[1].Value ?? throw keyboardException($"An unknown error occurred while building {keyboardContainer} keyboards (command Name Reference was null).");

                                    if (isLinked)
                                    {
                                        string moduleTextReference = match.Groups[2].Value ?? throw keyboardException($"An unknown error occurred while building {keyboardContainer} keyboard(s) (module text reference was null).");

                                        var referencedModule = _assemblyTypes
                                            .Where(type => type.Name == moduleTextReference)
                                            .FirstOrDefault();

                                        if (referencedModule is null) throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a module that doesn't appear to exist.");

                                        if (!(referencedModule.BaseType is { }
                                            && referencedModule.BaseType.IsAbstract
                                            && referencedModule.BaseType.IsGenericType
                                            && referencedModule.BaseType.GetGenericTypeDefinition() == typeof(CommandModule<>)))
                                            throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a module that doesn't appear to be a valid command context: {referencedModule?.FullName ?? "NULL"} (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                        if (!_tempModules[referencedModule].ModuleCommandBases.ContainsKey(commandNameReference))
                                            throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command that doesn't exist in linked module: {referencedModule?.FullName ?? "NULL"} (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                        referencedButton = _tempModules[referencedModule].ModuleCommandBases[commandNameReference]?.InButton;
                                    }
                                    else
                                    {
                                        if (parentModule is null || !_tempModules[parentModule].ModuleCommandBases.ContainsKey(commandNameReference))
                                            throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command in Module: {parentModule?.FullName ?? "\"NULL (check stack trace)\""} that doesn't appear to exist. (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                        referencedButton = _tempModules[parentModule].ModuleCommandBases[commandNameReference]?.InButton;
                                    }

                                    if (referencedButton is null || buttonType != referencedButton.GetType())
                                    {
                                        if (!config.BruteForceKeyboardReferences) throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command that doesn't have a keyboard button, and the configuration for this module ({parentModule?.FullName ?? "\"NULL (check stack trace)\""}) is set to terminate building when this occurs.");
                                        else
                                        {
                                            // Attempts to create a reference to the command when a button reference isn't available.
                                            referencedButton = (buttonType) switch
                                            {
                                                var _ when buttonType == typeof(InlineKeyboardButton) => InlineKeyboardButton.WithCallbackData(commandNameReference, $"BUTTONREFERENCEDCOMMAND::{commandNameReference}::"),
                                                var _ when buttonType == typeof(KeyboardButton) => new KeyboardButton(commandNameReference),
                                                _ => throw keyboardException($"An unknown exception occurred while building the keyboards for {keyboardContainer} (no type detected for TButton)"),
                                            };
                                        }
                                    }

                                    updatedKeyboardButtons.Add((TButton)referencedButton);
                                }
                            }
                            else updatedKeyboardButtons.Add(button);
                        }

                        updatedKeyboardBuilder.Add(updatedKeyboardButtons.ToArray());
                    }

                    return updatedKeyboardBuilder;
                }
            }
            void Init_3_CommandAssembler()
            {
                /* Description:
                  *
                  * Attempts to create Command objects based on the CommandModule method sharing the same name.
                  * Fails if method signature is wrong. Adds completed commands to the Command dictionary.
                  */

                // With modules assembled, can collect *every* method labeled as a Command:
                var allCommandMethods = _assemblyTypes
                    .Where(type => type.IsClass && _tempModules.ContainsKey(type))
                    .SelectMany(type => type.GetMethods())
                    .Where(method => !(method is null) && method.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                    .ToList();

                // Pre-emptively checks for duplicates (this will likely be a common error)
                var allCommandDuplicates = allCommandMethods
                    .Where(m => m.GetCustomAttribute<StepAttribute>() is null)
                    .GroupBy(m => m.GetCustomAttribute<CommandAttribute>()?.Name)
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g);

                // Will throw; responsible for formatting the exception.
                if(allCommandDuplicates.Count() != 0)
                {
                    Dictionary<string, (string Name, string MethodName)> duplicateNames = new Dictionary<string, (string, string)>();

                    foreach(var m in allCommandDuplicates)
                    {
                        duplicateNames[m.DeclaringType?.FullName ?? "??NULL??"] = (m.GetCustomAttribute<CommandAttribute>()?.Name ?? "??NULL??", m.Name);
                    }

                    bool oneDuplicate = duplicateNames.Count() == 1;
                    string duplicates = oneDuplicate ? "This Command has one duplicate " : "These Commands have one or more duplicates ";

                    int moduleCounter = 0;
                    int moduleTotal = duplicateNames.Keys.Count();
                    foreach (var module in duplicateNames.Keys)
                    {
                        moduleCounter++;

                        duplicates += $"in module {module}: ";
                        var namesList = duplicateNames.Select(n => n.Value).ToList();

                        int nameCounter = 0;
                        int nameTotal = namesList.Count();
                        foreach (var (Name, MethodName) in namesList) 
                        {
                            nameCounter++;

                            if (nameCounter == nameTotal)
                            {
                                if (moduleCounter == moduleTotal) duplicates += $"\"{Name}\" (method name: \"{MethodName}\"). Please check to make sure there are no conflicting Command names when running the Command Service.";
                                else duplicates += $"\"{Name}\" (method name: \"{MethodName}\"); ";
                            }
                            else duplicates += $"\"{Name}\" (method name: \"{MethodName}\"), ";
                        }
                    }
                    
                    throw new DuplicateCommandException(duplicates);
                }

                // Command Assembler
                foreach (var method in allCommandMethods)
                {
                    var module = method.DeclaringType ?? throw new CommandOnBuildingException("Error getting the module (DeclaringType) of a method while command building. (Returned null.)");

                    if (!tempCommands.ContainsKey(module)) tempCommands[module] = new Dictionary<ReadOnlyMemory<char>, ICommand>(CommandNameComparer.Default);

                    var methodAttributeCollection = method.GetCustomAttributes();
                    (
                        CommandAttribute Command,
                        PermissionsAttribute? Permissions,
                        RoomTypeAttribute? RoomType,
                        StepAttribute? Step
                    //? Add as needed...
                    )
                    attribs = (
                        methodAttributeCollection.FirstOrDefault(a => a is CommandAttribute) as CommandAttribute ?? throw new CommandOnBuildingException($"Error determining the Command Attribute name of a method in module: {module.FullName ?? "NULL"} while command building. (Returned null.)"),
                        methodAttributeCollection.FirstOrDefault(a => a is PermissionsAttribute) as PermissionsAttribute ?? module.GetCustomAttribute<PermissionsAttribute>(),
                        methodAttributeCollection.FirstOrDefault(a => a is RoomTypeAttribute) as RoomTypeAttribute ?? module.GetCustomAttribute<RoomTypeAttribute>(),
                        methodAttributeCollection.FirstOrDefault(a => a is StepAttribute) as StepAttribute
                    //? Add as needed...
                    );

                    var commandInfo = $"Command \"{attribs.Command.Name}\" (method name: \"{method.Name}\") in module {module.FullName}:";

                    if (string.IsNullOrWhiteSpace(attribs.Command.Name)) throw new InvalidCommandNameException($"A command in module {module.FullName} had a Command Attribute with a null or empty command name. Command names cannot be null, empty, or contain only whitespace characters.");

                    CommandBaseBuilder thisCommandBase;
                    if (_tempModules[module].ModuleCommandBases.TryGetValue(attribs.Command.Name, out var dictCommandBase)) thisCommandBase = dictCommandBase;
                    else thisCommandBase = new CommandBaseBuilder(attribs.Command.Name);

                    #region Setters. Add to this if additional functionality needs to be created later.
                    // Permissions
                    thisCommandBase.Set_Permissions(attribs.Permissions);

                    // Steps
                    if(TrySet_Steps().Continue) continue;
                    ////
                    #endregion

                    TryAddCommand(thisCommandBase);

                    //// Local Functions:

                    // Attempts to add the Command to the dictionary. Throws on failure.
                    void TryAddCommand(CommandBaseBuilder commandBase)
                    {
                        foreach (var alias in commandBase.InAliases) AuxiliaryMethods.CheckCommandNameValidity(commandBase.Name, true, alias);

                        var @params = method.GetParameters();
                        var length = @params.Length; // Update support.

                        // Checks return type for the incoming method. If it fails, it throws.
                        void CheckReturnType<TReturn>(Type returnType)
                        {
                            var invalidReturnType = new CommandOnBuildingException($"{commandInfo} method had invalid return type. (Was type: \"{returnType.Name}\". Expected type: \"{typeof(TReturn).Name}\".)");
                            if (returnType != typeof(TReturn)) throw invalidReturnType;
                        }

                        switch (commandBase.CommandType)
                        {
                            case CommandType.Default: CheckReturnType<Task>(method.ReturnType); break;
                            case CommandType.Step: CheckReturnType<Task<IStep>>(method.ReturnType); break;
                            //? Add as needed.
                        }

                        // Checks the incoming method for signature validity; throws if not valid.
                        if (!method.IsStatic
                          && length > 1
                          && _telegramEventArgs.Contains(@params[1].ParameterType))
                        {
                            if(length == 2)
                            {
                                // Passes the method's EventArgs parameter type.
                                AddCommand(commandBase, @params[1].ParameterType);
                            }
                            else if(length == 3 /* && @params[3].ParameterType == typeof(SomeType) */)
                            {
                                // This conditional is an example of how to set up different method signatures in the future, if updates require different checks.
                            }
                        }
                        else throw new CommandOnBuildingException($"{commandInfo} method had invalid signature.");

                        // Adds the finished command to the command list.
                        void AddCommand(CommandBaseBuilder c, Type t)
                        {
                            ICommand newCommand;
                            try
                            {
                                newCommand = t switch
                                {
                                    var _ when t == typeof(CallbackQueryEventArgs) => new Command<CallbackQueryEventArgs>(c, method, module),
                                    var _ when t == typeof(ChosenInlineResultEventArgs) => new Command<ChosenInlineResultEventArgs>(c, method, module),
                                    var _ when t == typeof(InlineQueryEventArgs) => new Command<InlineQueryEventArgs>(c, method, module),
                                    var _ when t == typeof(MessageEventArgs) => new Command<MessageEventArgs>(c, method, module),
                                    var _ when t == typeof(UpdateEventArgs) => new Command<UpdateEventArgs>(c, method, module),
                                    _ => throw new CommandOnBuildingException($"{commandInfo} failed to build. (Could not cast to proper command type. If you encounter this error, please notify the creator of the library. This should never happen.)")
                                };

                                if (newCommand is null) throw new CommandOnBuildingException($"{commandInfo} failed to build. (Attempt to add command resulted in a null command.)");
                            }
                            catch (ArgumentNullException e) { throw new CommandOnBuildingException($"{commandInfo} method was null.", e); }
                            catch (ArgumentException e) { throw new CommandOnBuildingException(commandInfo, e); }
                            catch (MissingMethodException e) { throw new CommandOnBuildingException($"{commandInfo} method not found.", e); }
                            catch (MethodAccessException e) { throw new CommandOnBuildingException($"{commandInfo} method MUST be marked public.", e); }

                            try
                            {
                                tempCommands[module].Add(attribs.Command.Name.AsMemory(), newCommand);
                                AddAliases(newCommand, commandBase.InAliases);
                            }
                            catch (ArgumentNullException e) { throw new CommandOnBuildingException($"An unexpected error occurred while building commmands in module: {module.FullName} (shouldn't ever happen, please submit a bug report if you ecnounter this error):", e); }
                            catch (ArgumentException) { throw new DuplicateCommandException($"{commandInfo} had a duplicate when attempting to add to internal dictionary. Please check to make sure there are no conflicting command names."); }
                        }
                    }

                    // Adds aliases for the command being added to the dictionary.
                    void AddAliases(ICommand commandToReference, string[] aliases)
                    {
                        foreach (string alias in aliases)
                        {
                            if (!tempCommands[module].TryGetValue(alias.AsMemory(), out _)) tempCommands[module].Add(alias.AsMemory(), commandToReference);
                            else throw new DuplicateCommandException($"{commandInfo} had an alias that shared a name with an existing command: {alias}. Please check to make sure there are no conflicting command names.");
                        }
                    }

                    // Local Setter Functions:
                    // Steps: Returns false if the 
                    (bool Continue, bool _) TrySet_Steps()
                    {
                        bool _ = false; //! a "discard" to allow for the named bool
                        HashSet<Type> supportedStepArgs = new HashSet<Type> { typeof(MessageEventArgs), typeof(CallbackQueryEventArgs) };
                        if (attribs.Step is { })
                        {
                            if (attribs.Step.StepNum == 0)
                            {
                                bool hasNoStepZero, hasDuplicateSteps, hasDuplicateWithNoStepAttribute, hasInvalidSignatures, toThrow;

                                var commandMethods = allCommandMethods.Where(m => m.GetCustomAttribute<CommandAttribute>()?.Name == attribs.Command.Name);
                                var commandMethodsWithStepAttribute = commandMethods.Where(m => m.GetCustomAttribute<StepAttribute>() is { });
                                var commandMethodsWithStepAttributeInt = commandMethodsWithStepAttribute.Select(m => m.GetCustomAttribute<StepAttribute>()?.StepNum).ToArray();
                                var commandMethodsWithInvalidSignature = commandMethodsWithStepAttribute.Where(m => !supportedStepArgs.Contains(m.GetParameters()[1].ParameterType));

                                hasNoStepZero = !(commandMethodsWithStepAttribute.Any(m => m.GetCustomAttribute<StepAttribute>() is { StepNum: 0 }));
                                hasDuplicateSteps = commandMethodsWithStepAttributeInt.Count() != commandMethodsWithStepAttributeInt.Distinct().Count();
                                hasDuplicateWithNoStepAttribute = commandMethods.Any(m => m.GetCustomAttribute<StepAttribute>() is null);
                                hasInvalidSignatures = commandMethodsWithInvalidSignature.Any();
                                toThrow = hasNoStepZero || hasDuplicateSteps || hasDuplicateWithNoStepAttribute || hasInvalidSignatures;

                                if (toThrow)
                                {
                                    List<Exception> exceptions = new List<Exception>();

                                    if (hasNoStepZero) exceptions.Add(new CommandOnBuildingException($"{commandInfo} has commands marked with the Step Attribute, but does not designate a parent command (a Step Attribute with a value of 0)."));
                                    if (hasDuplicateSteps)
                                    {
                                        // Gets non-distinct step numbers
                                        //: Make this LINQ more efficient later.
                                        var commandStepDuplicates = commandMethodsWithStepAttribute
                                            .GroupBy(m => m.GetCustomAttribute<StepAttribute>()?.StepNum)
                                            .Where(g => g.Count() > 1)
                                            .SelectMany(g => g)
                                            .Select(m => m.GetCustomAttribute<StepAttribute>()?.StepNum)
                                            .Distinct();

                                        bool oneDuplicate = commandStepDuplicates.Count() == 1;
                                        string duplicates = oneDuplicate ? "Step " : "Steps ";
                                        foreach (var num in commandStepDuplicates) { duplicates += $"{num}, "; }
                                        duplicates += oneDuplicate ? "has a" : "have";

                                        exceptions.Add(new CommandOnBuildingException($"{commandInfo} {duplicates} duplicate step(s) defined with the Step Attribute. Please check to make sure there are no conflicting steps (step numbers must be unique)."));
                                    }
                                    if (hasDuplicateWithNoStepAttribute) exceptions.Add(new CommandOnBuildingException($"{commandInfo} has one or more commands are defined with either a Step Attribute, or no Step Attribute. Command Step methods must ALL be marked with Step Attributes. If you did not mean to give this Command Steps, please remove Step Attributes for this Command method."));
                                    if (hasInvalidSignatures)
                                    {
                                        var commandStepsInvalidParameters = commandMethodsWithInvalidSignature
                                            .Select(m => m.GetCustomAttribute<StepAttribute>()?.StepNum)
                                            .Distinct();

                                        bool oneDuplicate = commandMethodsWithInvalidSignature.Count() == 1;
                                        string duplicates = oneDuplicate ? "Step " : "Steps ";
                                        foreach (var num in commandStepsInvalidParameters) { duplicates += $"{num}, "; }

                                        exceptions.Add(new CommandOnBuildingException($"{commandInfo} {duplicates} step method(s) are defined with the wrong method signature or EventArgs type. FluentCommands currently only supports Telegram Update EventArgs that contain valid Message objects (CallbackQueryEventArgs and MessageEventArgs). If you would like to see this feature expanded, please visit the Github page for this project and submit a ticket."));
                                    }
                                    if (exceptions.Count == 1)
                                        throw exceptions.First();
                                    else
                                        throw new CommandOnBuildingException($"There were errors constructing Steps for command \"{attribs.Command.Name}\": ", new AggregateException(exceptions));
                                }

                                try { thisCommandBase.Set_Steps(commandMethodsWithStepAttribute); }
                                catch (ArgumentException e) { throw new CommandOnBuildingException($"{commandInfo} had one or more Step methods that were the wrong method return type (must return Task<IStep>): ", e); }

                                return (false, _);
                            }
                            else return (true, _);
                        }
                        
                        return (false, _);
                    }
                }
            }
        }
        #endregion

        #region Start Overloads
        /// <summary>
        /// Initializes the <see cref="CommandService"/> with a default <see cref="CommandServiceConfig"/>.
        /// </summary>
        public static void Start() 
        {
            if (_commandServiceStarted) { Task.Run(async () => await Logger.Warning("Attempted to start the CommandService after it was already started. This action has no effect. Please consider checking your code and restarting your application to prevent this warning.").ConfigureAwait(false)); return; }
            _ =_instance.Value;
            _commandServiceStarted.Value = true;
        }

        /// <summary>
        /// Initializes the <see cref="CommandService"/>.
        /// </summary>
        public static void Start(CommandServiceConfigBuilder cfg)
        {
            if (_commandServiceStarted) { Task.Run(async () => await Logger.Warning("Attempted to start the CommandService after it was already started. This action has no effect; consider checking your code and restarting your application to prevent this warning.").ConfigureAwait(false)); return; }
            if (cfg is null) throw new CommandOnBuildingException("CommandServiceConfig was null.");

            _tempCfg = cfg;
            _ =_instance.Value;
            _commandServiceStarted.Value = true;
        }

        //: Create code examples for this documentation
        /// <summary>
        /// Initializes the <see cref="CommandService"/>.
        /// </summary>
        public static void Start(Action<CommandServiceConfigBuilder> buildAction)
        {
            if (_commandServiceStarted) { Task.Run(async () => await Logger.Warning("Attempted to start the CommandService after it was already started. This action has no effect; consider checking your code and restarting to prevent this warning.").ConfigureAwait(false)); return; }
            
            CommandServiceConfigBuilder cfg = new CommandServiceConfigBuilder();

            if (buildAction is { }) buildAction(cfg);
            else throw new CommandOnBuildingException("BuildAction for the CommandServiceConfig was null.");

            if (cfg is null) throw new CommandOnBuildingException("CommandServiceConfig was null. Please check your BuildAction delegate and restart your application. If this issue persists, please contact the creator of this library.");

            _tempCfg = cfg;
            _ =_instance.Value;
            _commandServiceStarted.Value = true;
        }

        public static ICommandServiceInitializer AddLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger
            => new CommandServiceStartNavigator().AddLogger<TLoggerImplementation>();

        public static ICommandServiceInitializer AddLogger(IFluentLogger implementationInstance)
            => new CommandServiceStartNavigator().AddLogger(implementationInstance);

        public static ICommandServiceInitializer AddLogger(Type implementationType)
            => new CommandServiceStartNavigator().AddLogger(implementationType);

        public static ICommandServiceInitializer AddDatabase<TDatabaseImplementation>() where TDatabaseImplementation : class, IFluentDatabase
            => new CommandServiceStartNavigator().AddDatabase<TDatabaseImplementation>();

        public static ICommandServiceInitializer AddDatabase(Type implementationType)
            => new CommandServiceStartNavigator().AddDatabase(implementationType);

        internal class CommandServiceStartNavigator : ICommandServiceInitializer
        {
            public ICommandServiceInitializer AddLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger
            {
                _services.Value.AddLogger<TLoggerImplementation>();
                return this;
            }

            public ICommandServiceInitializer AddLogger(IFluentLogger implementationInstance)
            {
                _services.Value.AddLogger(implementationInstance);
                return this;
            }

            public ICommandServiceInitializer AddLogger(Type implementationType)
            {
                _services.Value.AddLogger(implementationType);
                return this;
            }

            public ICommandServiceInitializer AddDatabase<TDatabaseImplementation>() where TDatabaseImplementation : class, IFluentDatabase
            {
                _services.Value.AddDatabase<TDatabaseImplementation>();
                return this;
            }

            public ICommandServiceInitializer AddDatabase(Type implementationType)
            {
                _services.Value.AddDatabase(implementationType);
                return this;
            }

            public void Start() => CommandService.Start();
            public void Start(CommandServiceConfigBuilder cfg) => CommandService.Start(cfg);
            public void Start(Action<CommandServiceConfigBuilder> buildAction) => CommandService.Start(buildAction);
        }
        #endregion

        #region Module Building Overloads
        /// <summary>
        /// Builds a <see cref="Command"/> module.
        /// <para>Provided type is the class that contains the commands being built.</para>
        /// </summary>  
        /// <typeparam name="TModule">The class that contains the commands the <see cref="ModuleBuilder"/> is being built for.</typeparam>
        /// <param name="buildAction">The "build action" is an <see cref="Action"/> that allows the user to configure the builder object—an alternate format to construct the <see cref="ModuleBuilder"/>.</param>
        public static void Module<TModule>(Action<IModuleBuilder> buildAction) where TModule : class
        {
            //? This method should stay generic for the purposes of quick bots that only need a small model in the main 
            //? method of the program class, and the actual command implementations in a separate class

            var moduleType = typeof(TModule);
            var module = new ModuleBuilder(moduleType);

            buildAction(module);

            if (!_tempModules.ContainsKey(moduleType)) _tempModules.Add(moduleType, module);
            else throw new CommandOnBuildingException($"This module, {moduleType.FullName ?? "NULL"}, appears to be a duplicate of another module with the same class type. You may only have one ModuleBuilder per class.");
        }

        /// <summary>
        /// Builds a <see cref="Command"/> module.
        /// <para>Provided type is the class that contains the commands being built.</para>
        /// </summary>
        /// <typeparam name="TModule">The class that contains the commands being built.</typeparam>
        /// <returns>Returns this <see cref="ModuleBuilder"/> to continue the fluent building process.</returns>
        public static IModuleBuilder Module<TModule>() where TModule : class
        {
            //? This method should stay generic for the purposes of quick bots that only need a small model in the main 
            //? method of the program class, and the actual command implementations in a separate class

            var moduleType = typeof(TModule);
            var module = new ModuleBuilder(moduleType);
            if (!_tempModules.ContainsKey(moduleType)) _tempModules.Add(moduleType, module);
            else throw new CommandOnBuildingException($"This module, {moduleType.FullName ?? "NULL"}, appears to be a duplicate of another module with the same class type. You may only have one ModuleBuilder per class.");
            return _tempModules[moduleType];
        }

        /// <summary>
        /// Builds a <see cref="Command"/> module.
        /// <para>Provided type is the class that contains the commands being built.</para>
        /// </summary>
        /// <returns>Returns this <see cref="ModuleBuilder"/> to continue the fluent building process.</returns>
        public static IModuleBuilder Module(Type module)
        {
            var moduleModule = new ModuleBuilder(module);
            if (!_tempModules.ContainsKey(module)) _tempModules.Add(module, moduleModule);
            else throw new CommandOnBuildingException($"This module, {module.FullName ?? "NULL"}, appears to be a duplicate of another module with the same class type. You may only have one ModuleBuilder per class.");
            return _tempModules[module];
        }
        #endregion

        #region Evaluate/ProcessInput Overloads
        public static async Task Evaluate<TModule>(TelegramBotClient client, CallbackQueryEventArgs e) where TModule : CommandModule<TModule> =>
            await Evaluate_Internal(typeof(TModule), client, e).ConfigureAwait(false);
        public static async Task Evaluate(Type module, TelegramBotClient client, CallbackQueryEventArgs e) =>
            await Evaluate_Internal(module, client, e).ConfigureAwait(false);

        public static async Task Evaluate<TModule>(TelegramBotClient client, ChosenInlineResultEventArgs e) where TModule : CommandModule<TModule> =>
            await Evaluate_Internal(typeof(TModule), client, e).ConfigureAwait(false);
        public static async Task Evaluate(Type module, TelegramBotClient client, ChosenInlineResultEventArgs e) =>
            await Evaluate_Internal(module, client, e).ConfigureAwait(false);

        public static async Task Evaluate<TModule>(TelegramBotClient client, InlineQueryEventArgs e) where TModule : CommandModule<TModule> =>
            await Evaluate_Internal(typeof(TModule), client, e).ConfigureAwait(false);
        public static async Task Evaluate(Type module, TelegramBotClient client, InlineQueryEventArgs e) =>
            await Evaluate_Internal(module, client, e).ConfigureAwait(false);

        public static async Task Evaluate<TModule>(TelegramBotClient client, MessageEventArgs e) where TModule : CommandModule<TModule> =>
            await Evaluate_Internal(typeof(TModule), client, e).ConfigureAwait(false);
        public static async Task Evaluate(Type module, TelegramBotClient client, MessageEventArgs e) =>
            await Evaluate_Internal(module, client, e).ConfigureAwait(false);

        public static async Task Evaluate<TModule>(TelegramBotClient client, UpdateEventArgs e) where TModule : CommandModule<TModule> =>
            await Evaluate_Internal(typeof(TModule), client, e).ConfigureAwait(false);
        public static async Task Evaluate(Type module, TelegramBotClient client, UpdateEventArgs e) =>
            await Evaluate_Internal(module, client, e).ConfigureAwait(false);

        /// <summary>
        /// Processes the input for a user's given args from an Evaluate method.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        private static async Task Evaluate_Internal(Type moduleType, TelegramBotClient client, TelegramUpdateEventArgs e)
        {
            if (!_commandServiceStarted) return; //? Can't log this; logging requires the service to have been started.

            if (moduleType is null) throw new NullReferenceException("The Module type was null.");
            if (client is null) throw new NullReferenceException("The TelegramBotClient was null.");
            if (e is null || e.HasNoArgs) throw new NullReferenceException("The EventArgs was null or contained no valid EventArgs."); //? Log?
            if (!Modules.TryGetValue(moduleType, out var module)) throw new ArgumentException($"There was no module found for the provided type: {moduleType.FullName}"); 

            //: When redoing the exceptions here, make sure to reflect them both in this method's XML summary as well as the ones that use this method to function

            var logger = module.Logger;
            _ = e.TryGetChatId(out var cId);
            _ = e.TryGetUserId(out var uId);
            var state = await Cache.GetState(cId, uId).ConfigureAwait(false);

            if (state.CurrentlyAccessed) return; //: Possibly log? Possibly inform the user? Possibly include this as an option in the global config
            else state.CurrentlyAccessed = true; //: This might not work with outside DBs injected into the framework. Consider temporarily storing user states in a ConcurrentDictionary. Null it out when the state is released. If not null, return. If null, continue. All you need to do is have a valid user id/chat id lookup and it would work without implementing IEquatable

            //? Add to this as needed (for feature updates that rely on the FluentState class)
            if (state is { StepState: { CommandStepInfo: { IsEmpty: false } } })
            {
                if (TryGetCommand(state.StepState.CommandStepInfo.CurrentCommandName.AsMemory(), moduleType, out var stepCommand)) { await ProcessCommand(stepCommand).ConfigureAwait(false); return; }
            }

            if (!AuxiliaryMethods.TryGetEventArgsRawInput(e, out ReadOnlyMemory<char> input)) return;
            var botId = client.BotId;
            var config = module.Config;
            var prefix = config.Prefix; //! Not AsMemory() due to the possibility of this string changing elsewhere during execution

            try
            {
                if (MemoryExtensions.StartsWith(input.Span, prefix, StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Slice(prefix.Length);

                    var commandMatch = FluentRegex.CheckCommand.Match(input.Span.ToString());

                    if (commandMatch.Success)
                    {
                        if (commandMatch.Groups.Count > 1)
                        {
                            var commandName = commandMatch.Groups[1].Value.AsMemory();
                            if (TryGetCommand(commandName, moduleType, out var command)) await ProcessCommand(command).ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (ArgumentNullException) { return; } //: Catch, default error message?, Log it, re-throw.
            catch (ArgumentException) { return; } //: Catch, Log it, re-throw.
            catch (RegexMatchTimeoutException) { return; } //: Catch, Log it, re-throw? maybe not re-throw

            // Processes the Command<TArgs> based on the type of its TArgs.
            async Task ProcessCommand(ICommand cmd)
            {
                switch (cmd.CommandType)
                {
                    case CommandType.Default:
                    {
                        switch (cmd)
                        {
                            case var _ when cmd is Command<CallbackQueryEventArgs> c:
                            {
                                if (e.TryGetCallbackQueryEventArgs(out var args)) await c.Invoke(client, args).ConfigureAwait(false);
                                else; //: error message, log.
                                break;
                            }
                            case var _ when cmd is Command<ChosenInlineResultEventArgs> c:
                            {
                                if (e.TryGetChosenInlineResultEventArgs(out var args)) await c.Invoke(client, args).ConfigureAwait(false);
                                else; //: log.
                                break;
                            }
                            case var _ when cmd is Command<InlineQueryEventArgs> c:
                            {
                                if (e.TryGetInlineQueryEventArgs(out var args)) await c.Invoke(client, args).ConfigureAwait(false);
                                else; //: log.
                                break;
                            }
                            case var _ when cmd is Command<MessageEventArgs> c:
                            {
                                if (e.TryGetMessageEventArgs(out var args)) await c.Invoke(client, args).ConfigureAwait(false);
                                else; //: log.
                                break;
                            }
                            case var _ when cmd is Command<UpdateEventArgs> c:
                            {
                                if (e.TryGetUpdateEventArgs(out var args)) await c.Invoke(client, args).ConfigureAwait(false);
                                else; //: log.  
                                break;
                            }
                            default:
                                //: Perform logging.
                                break;
                        }
                        break;
                    } 
                    case CommandType.Step:
                    {
                        switch (cmd)
                        {
                            case var _ when cmd is Command<CallbackQueryEventArgs> c:
                            {
                                await EvaluateStep(c).ConfigureAwait(false);
                                break;
                            }
                            case var _ when cmd is Command<MessageEventArgs> c:
                            {
                                await EvaluateStep(c).ConfigureAwait(false);
                                break;
                            }
                            default:
                            //: Perform logging.
                            break;
                        }
                        break;
                    }
                }
            }
            // If the Command has a StepInfo, attempts to evaluate the Command as a Step-Command.
            async Task EvaluateStep<TArgs>(Command<TArgs> c) where TArgs : EventArgs
            {
                int stepNum;
                if (state.IsDefault) stepNum = 0;
                else stepNum = state.StepState.CurrentStepNumber;

                IStep? stepReturn;
                var invoke = EvaluateInvoker<IStep>(c.StepInfo![stepNum]?.Delegate);
                stepReturn = await invoke.ConfigureAwait(false);

                if (stepReturn is null)
                {
                    if (c.StepInfo![stepNum] is null) return; //: Log this OR throw an exception? The user may have entered a stepnum that doesnt exist for this command
                    else return; //: Log this, but it should never happen lol.
                }

                if (stepReturn.OnResult is { }) await stepReturn.OnResult().ConfigureAwait(false);
                await state.StepState.Update(c as ICommand, stepReturn).ConfigureAwait(false);

                state.CurrentlyAccessed = false;

                await Cache.AddOrUpdateState(state).ConfigureAwait(false);
            }
            // Returns null if it fails to evaluate.
            async Task<TReturn?> EvaluateInvoker<TReturn>(Delegate? d) where TReturn : class
            {
                if (d is null) return null;

                switch (d)
                {
                    case CommandDelegate<CallbackQueryEventArgs, TReturn> c: 
                    {
                        if (e.TryGetCallbackQueryEventArgs(out var args)) return await c.Invoke(client, args).ConfigureAwait(false);
                        else return null;
                    }
                    case CommandDelegate<ChosenInlineResultEventArgs, TReturn> c: 
                    {
                        if (e.TryGetChosenInlineResultEventArgs(out var args)) return await c.Invoke(client, args).ConfigureAwait(false);
                        else return null;
                    }
                    case CommandDelegate<InlineQueryEventArgs, TReturn> c: 
                    {
                        if (e.TryGetInlineQueryEventArgs(out var args)) return await c.Invoke(client, args).ConfigureAwait(false);
                        else return null;
                    }
                    case CommandDelegate<MessageEventArgs, TReturn> c:
                    {
                        if (e.TryGetMessageEventArgs(out var args)) return await c.Invoke(client, args).ConfigureAwait(false);
                        else return null;
                    }
                    case CommandDelegate<UpdateEventArgs, TReturn> c:
                    {
                        if (e.TryGetUpdateEventArgs(out var args)) return await c.Invoke(client, args).ConfigureAwait(false);
                        else return null;
                    }
                    default:
                        return null;
                }
            }
        }

        #endregion

        #region BotLastMessages Methods
        /// <summary>
        /// Returns the last <see cref="Message"/>(s) sent by this <see cref="TelegramBotClient"/> for the <see cref="Chat"/> instance indicated by this <see cref="EventArgs"/>. 
        /// <para>Returns a collection with a length of 1 except for albums. Returns an empty collection if not found.</para>
        /// </summary>
        /// <param name="client">The <see cref="TelegramBotClient"/> that sent the <see cref="Message"/> being retrieved in this method.</param>
        /// <param name="e">The <see cref="UpdateEventArgs"/> being used to locate the last <see cref="Message"/>.</param>
        /// <returns>Returns the last <see cref="Message"/> objects sent by the bot in this <see cref="Chat"/> instance as an <see cref="IReadOnlyCollection{Message}"/>.</returns>
        public static IReadOnlyCollection<Message> BotLastMessages(TelegramBotClient client, TelegramUpdateEventArgs e)
        {
            //long chatId;
            //if (AuxiliaryMethods.TryGetEventArgsChatId(e, out long c_id)) chatId = c_id;
            //else if (AuxiliaryMethods.TryGetEventArgsUserId(e, out int u_id)) chatId = u_id;
            //else throw new Exception();

            //bool success;
            //byte attempts = 0;
            //Message[]? m;
            //do
            //{
            //    success = _botLastMessages[client.BotId].TryGetValue(chatId, out m);
            //    attempts++;
            //} 
            //while (!success && attempts < 10);

            //if (m is null) m = new Message[] { };

            return new Message[0];
        }

        /// <summary>
        /// Attempts to update the internal cache of the bot's last message(s).
        /// <para>Clears the history of all last messages, and adds the new one(s). Adds an empty collection if it fails.</para>
        /// </summary>
        internal static void UpdateBotLastMessages(TelegramBotClient client, long chatId, params Message[] messages)
        {
            //bool success;
            //byte attempts = 0;
            //try
            //{
            //    _botLastMessages[client.BotId].TryRemove(chatId, out _);

            //    do
            //    {
            //        success = _botLastMessages[client.BotId].TryAdd(chatId, messages);
            //        attempts++;
            //    } 
            //    while (!success && attempts < 10);

            //    if (!success && attempts == 10) _botLastMessages[client.BotId].TryAdd(chatId, new Message[] { });
            //}
            //catch (ArgumentNullException e) { return; } //: log it, check global cfg if should throw
            //catch (OverflowException e) { return; } //: log it, check global cfg if should throw
        }

        #endregion

        #region Helper Methods
        public async Task CallCommand<TModule>(TelegramBotClient client, TelegramUpdateEventArgs e, string commandName) where TModule : CommandModule<TModule>
        {
            await Evaluate_Internal(typeof(TModule), client, e);

            //: this method is probably unnecessary. think about it later
        }

        private static bool TryGetCommand(ReadOnlyMemory<char> inName, Type moduleType, [NotNullWhen(true)] out ICommand? outCommand)
        {
            if (!Commands.TryGetValue(moduleType, out var moduleDict)) { outCommand = null; return false; }
            if (!moduleDict.TryGetValue(inName, out outCommand)) return false;
            else return true;
        }
        #endregion

        #region Update Methods
        /// <summary>Exposes the private TempModules dictionary to update ModuleBuilders being built using the <see cref="Interfaces.BaseBuilderOfModule.ICommandBaseBuilderOfModule"/> format.</summary>
        internal static void UpdateBuilderInTempModules(ModuleBuilder m, Type t) => _tempModules[t] = m;

        /// <summary>Updates keyboard rows by iterating through each row and checking each button for an implicitly-converted KeybaordButtonReference.</summary>
        internal static List<TButton[]> UpdateKeyboardRows<TButton>(List<TButton[]> rows, Type? parentModule = null, string? parentCommandName = null, bool isMenu = true)
            where TButton : IKeyboardButton
        {
            List<TButton[]> updatedKeyboardBuilder = new List<TButton[]>();
            Type buttonType = typeof(TButton);

            ModuleConfig? config;
            Func<string, Exception> keyboardException;
            string keyboardContainer;
            if (isMenu)
            {
                config = new ModuleConfig(new ModuleConfigBuilder());

                keyboardContainer = "Menu";

                keyboardException = (string s) =>
                {
                    return new MenuReplyMarkupException(s);
                };
            }
            else
            {
                config = Modules[parentModule ?? throw new InvalidKeyboardRowException("Error updating keyboard rows. (Keyboard belonged to a Command, but the Command's module type was null.")]?.Config
                    ?? throw new NullReferenceException($"Module: \"{parentModule.FullName ?? "NULL"}\" config was null while updating keyboard rows for this Command.");

                keyboardContainer = $"Command \"{parentCommandName ?? "NULL"}\"";

                keyboardException = (string s) =>
                {
                    return new CommandOnBuildingException(s);
                };
            }

            foreach (var row in rows)
            {
                var updatedKeyboardButtons = new List<TButton>();

                foreach (var button in row)
                {
                    if (button is { }
                        && button.Text is { }
                        && button.Text.Contains("COMMANDBASEBUILDERREFERENCE"))
                    {
                        var match = FluentRegex.CheckButtonReference.Match(button.Text);
                        if (!match.Success)
                        {
                            match = FluentRegex.CheckButtonLinkedReference.Match(button.Text);
                            if (!match.Success) throw keyboardException($"Unknown error occurred while building {keyboardContainer} keyboard(s): button contained reference text \"{button.Text}\"");
                            else UpdateButton(match, true);
                        }
                        else UpdateButton(match);

                        // Locates the reference being pointed to by this TButton and updates it.
                        void UpdateButton(Match m, bool isLinked = false)
                        {
                            IKeyboardButton? referencedButton;

                            string commandNameReference = m.Groups[1].Value ?? throw keyboardException($"An unknown error occurred while building {keyboardContainer} keyboards (command Name Reference was null).");

                            if (isLinked)
                            {
                                string moduleTextReference = match.Groups[2].Value ?? throw keyboardException($"An unknown error occurred while building {keyboardContainer} keyboard(s) (module text reference was null).");

                                var referencedModule = _assemblyTypes
                                    .Where(type => type.Name == moduleTextReference)
                                    .FirstOrDefault();

                                if (referencedModule is null) throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a module that doesn't appear to exist.");

                                if (!(referencedModule.BaseType is { }
                                    && referencedModule.BaseType.IsAbstract
                                    && referencedModule.BaseType.IsGenericType
                                    && referencedModule.BaseType.GetGenericTypeDefinition() == typeof(CommandModule<>)))
                                    throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a module that doesn't appear to be a valid command context: {referencedModule?.FullName ?? "NULL"} (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                if (!_tempModules[referencedModule].ModuleCommandBases.ContainsKey(commandNameReference))
                                    throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command that doesn't exist in linked module: {referencedModule?.FullName ?? "NULL"} (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                referencedButton = _tempModules[referencedModule].ModuleCommandBases[commandNameReference]?.InButton;
                            }
                            else
                            {
                                if (parentModule is null || !_tempModules[parentModule].ModuleCommandBases.ContainsKey(commandNameReference))
                                    throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command in Module: {parentModule?.FullName ?? "\"NULL (check stack trace)\""} that doesn't appear to exist. (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                referencedButton = _tempModules[parentModule].ModuleCommandBases[commandNameReference]?.InButton;
                            }

                            if (referencedButton is null || buttonType != referencedButton.GetType())
                            {
                                if (!config.BruteForceKeyboardReferences) throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command that doesn't have a keyboard button, and the configuration for this module ({parentModule?.FullName ?? "\"NULL (check stack trace)\""}) is set to terminate building when this occurs.");
                                else
                                {
                                    // Attempts to create a reference to the command when a button reference isn't available.
                                    referencedButton = (buttonType) switch
                                    {
                                        var _ when buttonType == typeof(InlineKeyboardButton) => InlineKeyboardButton.WithCallbackData(commandNameReference, $"BUTTONREFERENCEDCOMMAND::{commandNameReference}::"),
                                        var _ when buttonType == typeof(KeyboardButton) => new KeyboardButton(commandNameReference),
                                        _ => throw keyboardException($"An unknown exception occurred while building the keyboards for {keyboardContainer} (no valid type detected for TButton)"),
                                    };
                                }
                            }

                            updatedKeyboardButtons.Add((TButton)referencedButton);
                        }
                    }
                    else updatedKeyboardButtons.Add(button);
                }

                updatedKeyboardBuilder.Add(updatedKeyboardButtons.ToArray());
            }

            return updatedKeyboardBuilder;
        }
        #endregion
    }
}
