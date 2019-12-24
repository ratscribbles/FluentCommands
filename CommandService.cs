using System;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using FluentCommands.Builders;
using FluentCommands.Commands;
using FluentCommands.Interfaces;
using FluentCommands.Attributes;
using FluentCommands.Exceptions;
using FluentCommands.Extensions;
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
using FluentCommands.Commands.Steps;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;

//[assembly: InternalsVisibleTo("FluentCommands.Tests.Unit")]

namespace FluentCommands
{
    //: Create methods that share internal cache services with outside dbs (EF core and such)

    /// <summary>
    /// The class responsible for handling the assembly and processing of Telegram Bot commands.
    /// </summary>
    public sealed class CommandService
    {
        private static readonly Lazy<CommandService> _instance = new Lazy<CommandService>(() => new CommandService(_tempCfg));
        private static readonly Lazy<CommandServiceCache> _defaultCache = new Lazy<CommandServiceCache>(() => new CommandServiceCache());
        private static readonly Lazy<CommandServiceLogger> _defaultLogger = new Lazy<CommandServiceLogger>(() => new CommandServiceLogger());
        private static readonly Lazy<CommandServiceServiceCollection> _services = new Lazy<CommandServiceServiceCollection>(() => new CommandServiceServiceCollection());
        private static readonly Lazy<EmptyLogger> _emptyLogger = new Lazy<EmptyLogger>(() => new EmptyLogger());
        private static readonly IReadOnlyCollection<Type> _assemblyTypes;
        private static readonly IReadOnlyCollection<Type> _commandContexts = new HashSet<Type> { typeof(CallbackQueryContext), typeof(ChosenInlineResultContext), typeof(InlineQueryContext), typeof(MessageContext), typeof(UpdateContext) };
        private static readonly Dictionary<Type, ModuleBuilder> _tempModules = new Dictionary<Type, ModuleBuilder>();
        private static ToggleOnce _commandServiceStarted = new ToggleOnce(false);
        private static CommandServiceConfigBuilder _tempCfg = new CommandServiceConfigBuilder();
        ///////
        private readonly CommandServiceConfig _config;
        private readonly IReadOnlyDictionary<Type, IReadOnlyModule> _modules;
        private readonly IReadOnlyDictionary<Type, IReadOnlyDictionary<ReadOnlyMemory<char>, ICommand>> _commands;
        private readonly IFluentCache? _customCache;
        private readonly IFluentLogger? _customLogger;
        private readonly TelegramBotClient? _client;
        ///////
        private static IReadOnlyDictionary<Type, IReadOnlyDictionary<ReadOnlyMemory<char>, ICommand>> Commands => _instance.Value._commands;
        internal static IReadOnlyDictionary<Type, IReadOnlyModule> Modules => _instance.Value._modules;
        internal static TelegramBotClient? InternalClient => _instance.Value._client;
        internal static IFluentCache Cache
        {
            get
            {
                if (GlobalConfig.UsingCustomDatabase) return _instance.Value._customCache!; // Not null if true
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

        internal static void AddClient(string token, Type moduleType) => _services.Value.AddClient(token, moduleType);
        internal static void AddClient(ClientBuilder clientBuilder, Type moduleType) => _services.Value.AddClient(clientBuilder, moduleType);
        internal static void AddClient(TelegramBotClient client, Type moduleType) => _services.Value.AddClient(client, moduleType);
        internal static void AddLogger<TLoggerImplementation>(Type moduleType) where TLoggerImplementation : class, IFluentLogger => _services.Value.AddLogger<TLoggerImplementation>(moduleType);
        internal static void AddLogger(IFluentLogger implementationInstance, Type moduleType) => _services.Value.AddLogger(implementationInstance, moduleType);
        internal static void AddLogger(Type implementationType, Type moduleType) => _services.Value.AddLogger(implementationType, moduleType);
        internal static void AddCache<TDatabaseImplementation>(Type moduleType) where TDatabaseImplementation : class, IFluentCache => _services.Value.AddCache<TDatabaseImplementation>(moduleType);
        internal static void AddCache(Type implementationType, Type moduleType) => _services.Value.AddCache(implementationType, moduleType);


        //: desc, explain that no type definition gets the default client 
        public static TelegramBotClient? Client()
            => _instance.Value._client;
        public static TelegramBotClient? Client(Type module)
            => Modules[module]?.Client;
        public static TelegramBotClient? Client<TModule>() where TModule : CommandModule<TModule>
            => Modules[typeof(TModule)]?.Client;

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
            _config = cfg.BuildConfig();

            // Is true if the CfgBuilder added services.
            if (_services.IsValueCreated)
            {
                var provider = _services.Value.GetServices().BuildServiceProvider();
                _client = provider.GetService<TelegramBotClient>();
                _customCache = provider.GetService<IFluentCache>();
                _customLogger = provider.GetService<IFluentLogger>();
                _services.Value.GetServices().Clear();
            }

            var tempCommands = new Dictionary<Type, Dictionary<ReadOnlyMemory<char>, ICommand>>();
            var tempServicesCollection = new Dictionary<Type, (TelegramBotClient client, IFluentCache cache, IFluentLogger logger)>();

            Init_1_ModuleAssembler();
            Init_2_CommandAssembler();

            var tempModulesToReadOnly = new Dictionary<Type, IReadOnlyModule>(_tempModules.Count);
            foreach (var kvp in _tempModules.ToList())
            {
                var (client, cache, logger) = tempServicesCollection[kvp.Key];
                tempModulesToReadOnly.Add(kvp.Key, new ReadOnlyCommandModule(kvp.Value, client, cache, logger));
            }

            var tempCommandsToReadOnly = new Dictionary<Type, IReadOnlyDictionary<ReadOnlyMemory<char>, ICommand>>(tempCommands.Count);
            foreach (var kvp in tempCommands.ToList()) tempCommandsToReadOnly.Add(kvp.Key, kvp.Value);

            _modules = tempModulesToReadOnly;
            _commands = tempCommandsToReadOnly;

            if (_client is null && !_config.EnableManualConfiguration)
            {
                if(_modules.Any(o => o.Value.Client is { }))
                {
                    //: Add to logger object.

                    //foreach, add warning for each one without a client. if none, regular warning
                }
                else throw new CommandOnBuildingException("The TelegramBotClient provided to the CommandService was null without Manual Configuration enabled, and there was no suitable TelegramBotClient provided for any module. Did you mean to enable Manual Configuration? Please verify that your CommandServiceConfig and ModuleBuilderConfigs are set- up properly before starting the CommandService.");
            }

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
                    MethodInfo method_RegisterHandlers;
                    try { method_RegisterHandlers = context.GetMethod("RegisterHandlers", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new CommandOnBuildingException(); }
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
                    var moduleConfigBuilder = new ModuleConfigBuilder(commandClass);

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

                    Action<TelegramBotClient?, bool> setHandlers = (client, b) =>
                    {
                        try
                        {
                            method_RegisterHandlers.Invoke(moduleContext, new object[] { client, b });
                        }
                        catch (TargetException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                        catch (ArgumentException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                        catch (TargetInvocationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                        catch (TargetParameterCountException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                        catch (MethodAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                        catch (InvalidOperationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                        catch (NotSupportedException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    };

                    moduleBuilder.SetConfig(moduleConfigBuilder);

                    UpdateBuilderInTempModules(moduleBuilder, commandClass);

                    foreach (var commandName in moduleBuilder.ModuleCommandBases.Keys)
                    {
                        AuxiliaryMethods.CheckCommandNameValidity(commandName);
                    }

                    // Get services added in the ModuleConfigBuilder
                    var provider = _services.Value.GetServices().BuildServiceProvider();
                    var client = provider.GetService<TelegramBotClient>();
                    var cache = provider.GetService<IFluentCache>();
                    var logger = provider.GetService<IFluentLogger>();

                    // Subscribe to the client if it exists. Global client if not. Not at all if not.
                    setHandlers(client is { } ? client : _client is { } ? _client : null, moduleConfigBuilder.DisableInternalCommandEvaluation);

                    tempServicesCollection.Add(moduleBuilder.TypeStorage, (client, cache, logger));

                    _services.Value.GetServices().Clear();
                }
            }
            void Init_2_CommandAssembler()
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
                        var paramLength = @params.Length; // Update support.

                        // Checks return type for the incoming method. If it fails, it throws.
                        void CheckReturnType<TReturn>(Type returnType)
                        {
                            var invalidReturnType = new CommandOnBuildingException($"{commandInfo} method had invalid return type. (Was type: \"{returnType.Name}\". Expected type: \"{typeof(TReturn).Name}\".)");
                            if (returnType != typeof(TReturn)) throw invalidReturnType;
                        }

                        switch (commandBase.CommandType)
                        {
                            case CommandType.Default: CheckReturnType<Task>(method.ReturnType); break;
                            case CommandType.Step: CheckReturnType<Task<Step>>(method.ReturnType); break;
                            //? Add as needed.
                        }

                        // Checks the incoming method for signature validity; throws if not valid.
                        if (!method.IsStatic
                          && paramLength == 1
                          && _commandContexts.Contains(@params[1].ParameterType))
                        {
                            // Passes the method's CommandContext<T> parameter type.
                            AddCommand(commandBase, @params[1].ParameterType);

                            if(paramLength == 3 /* && @params[3].ParameterType == typeof(SomeType) */)
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
                                    var _ when t == typeof(CallbackQueryContext) => new CommandBase<CallbackQueryContext, CallbackQueryEventArgs>(c, method, module),
                                    var _ when t == typeof(ChosenInlineResultContext) => new CommandBase<ChosenInlineResultContext, ChosenInlineResultEventArgs>(c, method, module),
                                    var _ when t == typeof(InlineQueryContext) => new CommandBase<InlineQueryContext, InlineQueryEventArgs>(c, method, module),
                                    var _ when t == typeof(MessageContext) => new CommandBase<MessageContext, MessageEventArgs>(c, method, module),
                                    var _ when t == typeof(UpdateContext) => new CommandBase<UpdateContext, UpdateEventArgs>(c, method, module),
                                    _ => throw new CommandOnBuildingException($"{commandInfo} failed to build. (Could not cast to proper command type. If you encounter this error, please submit a bug report. This should never happen.)")
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
        public static void Start(string token)
        {
            if (_commandServiceStarted) { Task.Run(async () => await Logger.Warning("Attempted to start the CommandService after it was already started. This action has no effect. Please consider checking your code and restarting your application to prevent this warning.").ConfigureAwait(false)); return; }
            _tempCfg.AddClient(token);
            _ = _instance.Value;

            StartClientsReceiving();

            _commandServiceStarted.Value = true;
        }

        public static void Start(TelegramBotClient client)
        {
            if (_commandServiceStarted) { Task.Run(async () => await Logger.Warning("Attempted to start the CommandService after it was already started. This action has no effect. Please consider checking your code and restarting your application to prevent this warning.").ConfigureAwait(false)); return; }
            _tempCfg.AddClient(client);
            _ = _instance.Value;

            StartClientsReceiving();

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

            StartClientsReceiving();

            _commandServiceStarted.Value = true;
        }

        //: Create code examples for this documentation
        /// <summary>
        /// Initializes the <see cref="CommandService"/>.
        /// </summary>
        public static void Start(Action<CommandServiceConfigBuilder> buildAction)
        {
            if (_commandServiceStarted) { Task.Run(async () => await Logger.Warning("Attempted to start the CommandService after it was already started. This action has no effect; consider checking your code and restarting to prevent this warning.").ConfigureAwait(false)); return; }
            
            if (buildAction is { }) buildAction(_tempCfg);
            else throw new CommandOnBuildingException("BuildAction for the CommandServiceConfig was null.");

            if (_tempCfg is null) throw new CommandOnBuildingException("CommandServiceConfig was null. Please check your BuildAction delegate and restart your application. If this issue persists, please contact the creator of this library.");

            _ =_instance.Value;

            StartClientsReceiving();

            _commandServiceStarted.Value = true;
        }

        private static void StartClientsReceiving()
        {
            if (_commandServiceStarted) return;

            try
            {
                InternalClient?.StartReceiving(Array.Empty<UpdateType>());
            }
            catch (ApiRequestException ex)
            {
                throw new InvalidConfigSettingsException("There was an issue initializing the Default TelegramBotClient for the CommandService. Please double-check your client, token, or ConfigBuilder to make sure it has been provided correctly. If this issue persists and you believe it is in error, please submit a bug report on the FluentCommands Github page.", ex);
            }

            foreach (var key in Modules.Keys)
            {
                try
                {
                    Modules[key].Client?.StartReceiving(Array.Empty<UpdateType>());
                }
                catch (ApiRequestException ex)
                {
                    throw new InvalidConfigSettingsException($"There was an issue initializing the TelegramBotClient for the CommandModule: {key.FullName}. Please double-check your client, token, or ConfigBuilder to make sure it has been provided correctly. If this issue persists and you believe it is in error, please submit a bug report on the FluentCommands Github page.", ex);
                }
            }
        }
        #endregion

        #region Evaluate/ProcessInput Overloads
        //: Warn user that if there's a client provided for the module, they'll receive events as well. BOLD that they should only be using these methods if they're an advanced user
        public static async Task Evaluate<TModule>(TelegramBotClient clientOverride, CallbackQueryEventArgs e) where TModule : class
            => await Evaluate_Internal(typeof(TModule), e, clientOverride).ConfigureAwait(false);
        public static async Task Evaluate<TModule>(TelegramBotClient clientOverride, ChosenInlineResultEventArgs e) where TModule : class
            => await Evaluate_Internal(typeof(TModule), e, clientOverride).ConfigureAwait(false);
        public static async Task Evaluate<TModule>(TelegramBotClient clientOverride, InlineQueryEventArgs e) where TModule : class
            => await Evaluate_Internal(typeof(TModule), e, clientOverride).ConfigureAwait(false);
        public static async Task Evaluate<TModule>(TelegramBotClient clientOverride, MessageEventArgs e) where TModule : class
            => await Evaluate_Internal(typeof(TModule), e, clientOverride).ConfigureAwait(false);
        public static async Task Evaluate<TModule>(TelegramBotClient clientOverride, UpdateEventArgs e) where TModule : class
            => await Evaluate_Internal(typeof(TModule), e, clientOverride).ConfigureAwait(false);
        public static async Task Evaluate(TelegramBotClient clientOverride, CallbackQueryEventArgs e, Type commandModule)
            => await Evaluate_Internal(commandModule, e, clientOverride).ConfigureAwait(false);
        public static async Task Evaluate(TelegramBotClient clientOverride, ChosenInlineResultEventArgs e, Type commandModule)
            => await Evaluate_Internal(commandModule, e, clientOverride).ConfigureAwait(false);
        public static async Task Evaluate(TelegramBotClient clientOverride, InlineQueryEventArgs e, Type commandModule)
            => await Evaluate_Internal(commandModule, e, clientOverride).ConfigureAwait(false);
        public static async Task Evaluate(TelegramBotClient clientOverride, MessageEventArgs e, Type commandModule)
            => await Evaluate_Internal(commandModule, e, clientOverride).ConfigureAwait(false);
        public static async Task Evaluate(TelegramBotClient clientOverride, UpdateEventArgs e, Type commandModule)
            => await Evaluate_Internal(commandModule, e, clientOverride).ConfigureAwait(false);

        //! These are for internal eventhandler purposes only (within modules). Clients will automatically be evaluating with these methods.
        internal static async Task Evaluate_ToHandler<TModule>(CallbackQueryEventArgs e) where TModule : class
            => await Evaluate_Internal(typeof(TModule), e).ConfigureAwait(false);
        internal static async Task Evaluate_ToHandler<TModule>(ChosenInlineResultEventArgs e) where TModule : class
            => await Evaluate_Internal(typeof(TModule), e).ConfigureAwait(false);
        internal static async Task Evaluate_ToHandler<TModule>(InlineQueryEventArgs e) where TModule : class
            => await Evaluate_Internal(typeof(TModule), e).ConfigureAwait(false);
        internal static async Task Evaluate_ToHandler<TModule>(MessageEventArgs e) where TModule : class
            => await Evaluate_Internal(typeof(TModule), e).ConfigureAwait(false);
        internal static async Task Evaluate_ToHandler<TModule>(UpdateEventArgs e) where TModule : class
            => await Evaluate_Internal(typeof(TModule), e).ConfigureAwait(false);

        /// <summary>
        /// Processes the input for a user's given args from an Evaluate method.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        private static async Task Evaluate_Internal(Type moduleType, TelegramUpdateEventArgs e, TelegramBotClient? client = null)
        {
            if (!_commandServiceStarted) return; //? Can't log this; logging requires the service to have been started.

            if (moduleType is null) throw new NullReferenceException("The Module type was null.");
            if (!Modules.TryGetValue(moduleType, out var module)) throw new ArgumentException($"There was no module found for the provided type: {moduleType.FullName}.");
            var noClient = new NullReferenceException("Could not find a suitable TelegramBotClient to evaluate with. (Did you mean to provide a Client Override to the method?) Please register a TelegramBotClient with the AddClient method in either the CommandService, or in your command module's OnConfiguring method.");
            if (client is null) client = module.UseClient ? (module.Client ?? throw noClient) : InternalClient is { } ? InternalClient : throw noClient;
            if (e is null || e.HasNoArgs) throw new NullReferenceException("The EventArgs was null or invalid. (The evaluate method only supports 5 EventArgs: CallbackQuery, ChosenInlineResult, InlineQuery, Message, and Update EventArgs.)");

            //: When redoing the exceptions here, make sure to reflect them both in this method's XML summary as well as the ones that use this method to function
            var logger = module.Logger;
            _ = e.TryGetChat(out var chat);
            _ = e.TryGetUser(out var user);
            var state = await Cache.GetState(client.BotId, chat?.Id ?? 0, user?.Id ?? 0).ConfigureAwait(false);

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

            ICommand? command;
            if (MemoryExtensions.StartsWith(input.Span, prefix, StringComparison.OrdinalIgnoreCase))
            {
                input = input.Slice(prefix.Length);

                var commandMatch = FluentRegex.CheckCommand.Match(input.Span.ToString());

                if (commandMatch.Success)
                {
                    if (commandMatch.Groups.Count > 1)
                    {
                        var commandName = commandMatch.Groups[1].Value.AsMemory();
                        if (!TryGetCommand(commandName, moduleType, out command))
                        {
                            await Logger.Info($"{user?.ToFluentLogger() ?? "Unknown user"}");
                            return;
                            //: improve this lol im going to sleep

                        }
                        else
                        {
                            if (config.DeleteCommandAfterCall)
                            {
                                var foundMessage = e.TryGetMessage(out var msg);

                                if (foundMessage)
                                {
                                    try
                                    {
                                        await client.DeleteMessageAsync(chat?.Id, msg!.MessageId).ConfigureAwait(false);
                                    }
                                    catch
                                    {
                                        //: log
                                    }
                                }
                                else return; //: Log
                            }
                        }
                    }
                    else return; //: log
                }
                else return; //: log
            }
            else return; //: log

            try
            {
                await ProcessCommand(command).ConfigureAwait(false);
            }
            catch (ArgumentNullException) { return; } //: Catch, default error message?, Log it, re-throw (if the config says to re-throw)
            catch (ArgumentException) { return; } //: Catch, Log it, re-throw.
            catch (RegexMatchTimeoutException) { return; } //: Catch, Log it, re-throw? maybe not re-throw
            catch (Exception) { return; }

            state.CurrentlyAccessed = false;

            await Cache.AddOrUpdateState(state).ConfigureAwait(false);

            // Processes the Command<TArgs> based on the type of its TArgs.
            async Task ProcessCommand(ICommand cmd)
            {
                switch (cmd.CommandType)
                {
                    case CommandType.Default:
                    {
                        switch (cmd)
                        {
                            case var _ when cmd is CommandBase<CallbackQueryContext, CallbackQueryEventArgs> c:
                            {
                                if (e.TryGetCallbackQueryEventArgs(out var args)) await c.Invoke((moduleType, client!, args)).ConfigureAwait(false);
                                else; //: error message, log.
                                break;
                            }
                            case var _ when cmd is CommandBase<ChosenInlineResultContext, ChosenInlineResultEventArgs> c:
                            {
                                if (e.TryGetChosenInlineResultEventArgs(out var args)) await c.Invoke((moduleType, client!, args)).ConfigureAwait(false);
                                else; //: log.
                                break;
                            }
                            case var _ when cmd is CommandBase<InlineQueryContext, InlineQueryEventArgs> c:
                            {
                                if (e.TryGetInlineQueryEventArgs(out var args)) await c.Invoke((moduleType, client!, args)).ConfigureAwait(false);
                                else; //: log.
                                break;
                            }
                            case var _ when cmd is CommandBase<MessageContext, MessageEventArgs> c:
                            {
                                if (e.TryGetMessageEventArgs(out var args)) await c.Invoke((moduleType, client!, args)).ConfigureAwait(false);
                                else; //: log.
                                break;
                            }
                            case var _ when cmd is CommandBase<UpdateContext, UpdateEventArgs> c:
                            {
                                if (e.TryGetUpdateEventArgs(out var args)) await c.Invoke((moduleType, client!, args)).ConfigureAwait(false);
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
                            case var _ when cmd is CommandBase<CallbackQueryContext, CallbackQueryEventArgs> c:
                            {
                                await EvaluateStep(c).ConfigureAwait(false);
                                break;
                            }
                            case var _ when cmd is CommandBase<MessageContext, MessageEventArgs> c:
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
            async Task EvaluateStep<TContext, TArgs>(CommandBase<TContext, TArgs> c) 
                where TContext : ICommandContext<TArgs> 
                where TArgs : EventArgs
            {
                int stepNum;
                if (state.IsDefault) stepNum = 0;
                else stepNum = state.StepState.CurrentStepNumber;

                Step? stepReturn;
                var invoke = EvaluateInvoker<Step>(c.StepInfo![stepNum]?.Delegate);
                stepReturn = await invoke.ConfigureAwait(false);

                if (stepReturn is null)
                {
                    if (c.StepInfo![stepNum] is null) return; //: Log this OR throw an exception? The user may have entered a stepnum that doesnt exist for this command
                    else return; //: Log this, but it should never happen lol.
                }

                if (stepReturn.OnResult is { }) await stepReturn.OnResult().ConfigureAwait(false);
                await state.StepState.Update(c as ICommand, stepReturn).ConfigureAwait(false);
            }
            // Returns null if it fails to evaluate.
            async Task<TReturn?> EvaluateInvoker<TReturn>(Delegate? d) where TReturn : class
            {
                if (d is null) return null;

                switch (d)
                {
                    case CommandDelegate<CallbackQueryContext, CallbackQueryEventArgs, TReturn> c: 
                    {
                        if (e.TryGetCallbackQueryEventArgs(out var args)) return await c.Invoke((moduleType, client!, args)).ConfigureAwait(false);
                        else return null;
                    }
                    case CommandDelegate<ChosenInlineResultContext, ChosenInlineResultEventArgs, TReturn> c: 
                    {
                        if (e.TryGetChosenInlineResultEventArgs(out var args)) return await c.Invoke((moduleType, client!, args)).ConfigureAwait(false);
                        else return null;
                    }
                    case CommandDelegate<InlineQueryContext, InlineQueryEventArgs, TReturn> c: 
                    {
                        if (e.TryGetInlineQueryEventArgs(out var args)) return await c.Invoke((moduleType, client!, args)).ConfigureAwait(false);
                        else return null;
                    }
                    case CommandDelegate<MessageContext, MessageEventArgs, TReturn> c:
                    {
                        if (e.TryGetMessageEventArgs(out var args)) return await c.Invoke((moduleType, client!, args)).ConfigureAwait(false);
                        else return null;
                    }
                    case CommandDelegate<UpdateContext, UpdateEventArgs, TReturn> c:
                    {
                        if (e.TryGetUpdateEventArgs(out var args)) return await c.Invoke((moduleType, client!, args)).ConfigureAwait(false);
                        else return null;
                    }
                    default:
                        return null;
                }
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>Attempts to get the command from the command dictionary.</summary>
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

            ModuleConfig config;
            Func<string, Exception> keyboardException;
            string keyboardContainer;
            if (isMenu)
            {
                config = new ModuleConfig(new ModuleConfigBuilder(typeof(CommandService)));

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
