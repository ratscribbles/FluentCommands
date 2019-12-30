using System;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
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
        private static readonly Lazy<CommandServiceOnBuildingNotifier> _notifier = new Lazy<CommandServiceOnBuildingNotifier>(() => new CommandServiceOnBuildingNotifier());
        private static readonly Lazy<EmptyLogger> _emptyLogger = new Lazy<EmptyLogger>(() => new EmptyLogger());
        private static readonly IReadOnlyCollection<Type> _validEventArgs = new HashSet<Type> { typeof(CallbackQueryEventArgs), typeof(ChosenInlineResultEventArgs), typeof(InlineQueryEventArgs), typeof(MessageEventArgs), typeof(UpdateEventArgs) };
        private static readonly IReadOnlyCollection<Type> _commandContexts = new HashSet<Type> { typeof(CallbackQueryContext), typeof(ChosenInlineResultContext), typeof(InlineQueryContext), typeof(MessageContext), typeof(UpdateContext) };
        private static readonly Dictionary<Type, ModuleBuilder> _tempModules = new Dictionary<Type, ModuleBuilder>();
        private static ToggleOnce _commandServiceStarted = new ToggleOnce(false);
        private static CommandServiceConfigBuilder _tempCfg = new CommandServiceConfigBuilder();
        ///////
        private readonly CommandServiceConfig _config;
        private readonly IReadOnlyDictionary<Type, IReadOnlyModule> _modules;
        private readonly IReadOnlyDictionary<Type, (TelegramBotClient? Client, IFluentCache? Cache, IFluentLogger? Logger)> _servicesCollection;
        private readonly IReadOnlyDictionary<Type, IReadOnlyDictionary<ReadOnlyMemory<char>, ICommand>> _commands;
        private readonly IFluentCache? _customCache;
        private readonly IFluentLogger? _customLogger;
        private readonly TelegramBotClient? _client;
        ///////
        private static CommandServiceCache InternalCache => _defaultCache.Value;
        private static IReadOnlyDictionary<Type, IReadOnlyDictionary<ReadOnlyMemory<char>, ICommand>> Commands => _instance.Value._commands;
        internal static IReadOnlyDictionary<Type, (TelegramBotClient? Client, IFluentCache? Cache, IFluentLogger? Logger)> ServicesCollection => _instance.Value._servicesCollection;
        internal static IReadOnlyDictionary<Type, IReadOnlyModule> Modules => _instance.Value._modules;
        internal static TelegramBotClient? InternalClient => ServicesCollection[typeof(CommandService)].Client;
        internal static IFluentCache Cache
        {
            get
            {
                if (GlobalConfig.UsingCustomCache) return ServicesCollection[typeof(CommandService)].Cache!; // Not null if true
                else return _defaultCache.Value;
            }
        }
        internal static IFluentLogger Logger
        {
            get
            {
                if (!GlobalConfig.DisableLogging)
                {
                    if (GlobalConfig.UsingCustomLogger) return ServicesCollection[typeof(CommandService)].Logger!; // Not null if true
                    else return _defaultLogger.Value;
                }
                else return EmptyLogger;
            }
        }
        internal static IFluentLogger EmptyLogger => _emptyLogger.Value;
        internal static CommandServiceConfig GlobalConfig => _instance.Value._config;

        internal static void AddClient(string token, Type moduleType) => _services.Value.AddClient(token, moduleType);
        internal static void AddClient(TelegramBotClient client, Type moduleType) => _services.Value.AddClient(client, moduleType);
        internal static void AddLogger<TLoggerImplementation>(Type moduleType) where TLoggerImplementation : class, IFluentLogger => _services.Value.AddLogger<TLoggerImplementation>(moduleType);
        internal static void AddLogger(IFluentLogger implementationInstance, Type moduleType) => _services.Value.AddLogger(implementationInstance, moduleType);
        internal static void AddLogger(Type implementationType, Type moduleType) => _services.Value.AddLogger(implementationType, moduleType);
        internal static void AddCache<TCacheImplementation>(Type moduleType) where TCacheImplementation : class, IFluentCache => _services.Value.AddCache<TCacheImplementation>(moduleType);
        internal static void AddCache(Type implementationType, Type moduleType) => _services.Value.AddCache(implementationType, moduleType);


        //: desc, explain that no type definition gets the default client 
        public static TelegramBotClient? Client()
            => _instance.Value._client;
        public static TelegramBotClient? Client(Type module)
            => Modules[module]?.Client;
        public static TelegramBotClient? Client<TCommand>() where TCommand : CommandModule<TCommand>
            => Modules[typeof(TCommand)]?.Client;

        #region Constructors
        /// <summary>
        /// Constructor for use only with the singleton. Enforces that internal collectons are completely unable to be modified and are read-only. Populates the following:
        /// <para>- Modules readonly dictionary</para>
        /// <para>- Commands readonly dictionary</para>
        /// <para>- Services readonly dictionary</para>
        /// <para>- Global config object</para>
        /// </summary>
        private CommandService(CommandServiceConfigBuilder cfg)
        {
            //! Establish notifier.
            var notifier = _notifier.Value;

            //! Gather all assemblies.
            IReadOnlyCollection<Type> _assemblyTypes;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> assemblyTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                List<Type> internalTypes;

                try { internalTypes = assembly.GetTypes().ToList(); }
                catch (ReflectionTypeLoadException e) 
                {
                    try { internalTypes = e.Types.Where(type => !(type is null)).ToList(); }
                    catch (Exception ex)
                    {
                        notifier.AddDebug("Unexpected error occurred while beginning the CommandService on-building process.", ex);
                        continue;
                    }
                }

                assemblyTypes.AddRange(internalTypes);
            }

            _assemblyTypes = assemblyTypes;

            //! Establish config.
            _config = cfg.BuildConfig();

            //! Setup Default Services:
            var tempServicesCollection = new Dictionary<Type, (TelegramBotClient? Client, IFluentCache? Cache, IFluentLogger? Logger)>();

            // Is true if the CfgBuilder added services
            if (_services.IsValueCreated)
            {
                var provider = _services.Value.GetServices().BuildServiceProvider();
                _client = provider.GetService<TelegramBotClient>();
                _customCache = provider.GetService<IFluentCache>();
                _customLogger = provider.GetService<IFluentLogger>();
                _services.Value.GetServices().Clear();
            }
            else
            {
                notifier.AddWarning("No default client detected for CommandService.");
                notifier.AddInfo("Using default Cache and Logger.");
                _client = null;
                _customCache = null;
                _customLogger = null;
            }
            tempServicesCollection.Add(typeof(CommandService), (_client, _customCache, _customLogger));

            //! Temporary collections for use in the building process (to make them readonly once completed)
            var tempCommands = new Dictionary<Type, Dictionary<ReadOnlyMemory<char>, ICommand>>();


            Init_1_ModuleAssembler();


            // With modules assembled, can collect *every* method labeled as a Command:
            var allCommandMethods = _assemblyTypes
                .Where(type => type.IsClass && _tempModules.ContainsKey(type))
                .SelectMany(type => type.GetMethods())
                .Where(method => !(method is null) && method.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                .ToList();


            Init_2_CommandAssembler();


            // Assemble teadonly instance collections
            var tempModulesToReadOnly = new Dictionary<Type, IReadOnlyModule>(_tempModules.Count);
            foreach (var kvp in _tempModules.ToList())
            {
                var (client, cache, logger) = tempServicesCollection[kvp.Key];
                tempModulesToReadOnly.Add(kvp.Key, new ReadOnlyCommandModule(kvp.Value, client is { }, cache is { }, logger is { }));
            }

            var tempCommandsToReadOnly = new Dictionary<Type, IReadOnlyDictionary<ReadOnlyMemory<char>, ICommand>>(tempCommands.Count);
            foreach (var kvp in tempCommands.ToList()) tempCommandsToReadOnly.Add(kvp.Key, kvp.Value);

            _modules = tempModulesToReadOnly;
            _commands = tempCommandsToReadOnly;
            _servicesCollection = tempServicesCollection;


            //! Local functions...
            void Init_1_ModuleAssembler()
            {
                /* Description:
                  *
                  * Attempts to collect the user's CommandModules and assemble them based on their OnBuilding methods.
                  * Fails if any exception is thrown. Only detects modules that inherit from CommandModule<> directly.
                  */

                // Collects *every* ModuleBuilder command context (all classes that derive from CommandContext)
                var allCommandModules = _assemblyTypes
                    .Where(type => !(type.BaseType is null)
                        && type.BaseType.IsAbstract
                        && type.BaseType.IsGenericType
                        && type.BaseType.GetGenericTypeDefinition() == typeof(CommandModule<>))
                    .ToList();

                if (allCommandModules is null) throw new CommandOnBuildingException("Collection of command contexts was null. Please submit a bug report and/or contact the creator of this library if this issue persists.");

                foreach (var context in allCommandModules)
                {
                    string unexpected = $"An unexpected error occurred while building command module: {context.FullName}";

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
                    try { commandClass = (Type?)property.GetValue(moduleContext) ?? throw new CommandOnBuildingException(unexpected); }
                    catch (ArgumentException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetParameterCountException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (MethodAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetInvocationException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    if (_tempModules.ContainsKey(commandClass)) throw new CommandOnBuildingException($"More than one CommandModule<TCommand> is defined with the same Command class: {commandClass.FullName}. Please make sure each CommandModule<TCommand> is unique. (No two TCommand classes should be the same.)");

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
                    var client = provider.GetService<TelegramBotClient?>();
                    var cache = provider.GetService<IFluentCache>();
                    var logger = provider.GetService<IFluentLogger>();

                    // Check if a client was registered to this module, or if one of the same Id was already registered to another module.
                    //if (tempServicesCollection.Any(kvp => kvp.Value.client?.BotId == client?.BotId)) throw new InvalidConfigSettingsException($"Duplicate TelegramBotClient detected in module: {commandClass.FullName}. Please make sure to only use ")
                    client = client is { } ? client : _client is { } ? _client : null;

                    var existingClient = tempServicesCollection.FirstOrDefault(kvp => client is { } && kvp.Value.Client?.BotId == client.BotId).Value.Client;
                    if (existingClient is { }) client = existingClient;
                    
                    if ((client?.BotId ?? 0) == (_client?.BotId ?? 1)) notifier.AddInfo($"Using default client for CommandModule<TCommand>: {commandClass.FullName}.");
                    else notifier.AddWarning($"No client is assigned to CommandModule<TCommand>: {commandClass.FullName}. Commands from this Module will not be evaluated.");

                    // Subscribe to the client if it exists. Global client if not. Not at all if not.
                    setHandlers(client, moduleConfigBuilder.On_Building_DisableInternalCommandEvaluation);

                    // Save services to add to module later.
                    tempServicesCollection.Add(moduleBuilder.TypeStorage, (client, cache, logger));

                    _services.Value.GetServices().Clear();
                    notifier.AddDebug($"Module: {commandClass.FullName} build OK!");
                }
            }
            void Init_2_CommandAssembler()
            {
                /* Description:
                  *
                  * Attempts to create Command objects based on the CommandModule method sharing the same name.
                  * Fails if method signature is wrong. Adds completed commands to the Command dictionary.
                  */

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
                    else 
                    { 
                        thisCommandBase = new CommandBaseBuilder(attribs.Command.Name);
                        notifier.AddDebug($"No OnBuilding detected for Command \"{attribs.Command.Name}\" in Module: \"{module.FullName}\"; using default settings.");
                    }

                    #region Setters. Add to this if additional functionality needs to be created later.
                    // Permissions
                    thisCommandBase.Set_Permissions(attribs.Permissions);

                    // Steps
                    if (TrySet_Steps().Continue) continue;
                    ////
                    #endregion

                    TryAddCommand(thisCommandBase);

                    
                    //: Check for help command; if it doesn't exist, make one.


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
                            if (returnType != typeof(TReturn)) throw new CommandOnBuildingException($"{commandInfo} method had invalid return type. (Was type: \"{returnType.Name}\". Expected type: \"{typeof(TReturn).Name}\".)");
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
                          && _commandContexts.Contains(@params[0].ParameterType))
                        {
                            // Passes the method's CommandContext<T> parameter type.
                            AddCommand(commandBase, @params[0].ParameterType);

                            if (paramLength == 3 /* && @params[3].ParameterType == typeof(SomeType) */)
                            {
                                //? This conditional is an example of how to set up different method signatures in the future, if updates require different checks.
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
                        const bool _ = false; //! a "discard" to allow for the named bool
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
            if (_commandServiceStarted) { Logger.LogWarning("Attempted to start the CommandService after it was already started. This action has no effect; consider checking your code and restarting your application to prevent this warning."); return; }
            _tempCfg.AddClient(token);

            Init();
        }

        public static void Start(TelegramBotClient client)
        {
            if (_commandServiceStarted) { Logger.LogWarning("Attempted to start the CommandService after it was already started. This action has no effect; consider checking your code and restarting your application to prevent this warning."); return; }
            _tempCfg.AddClient(client);

            Init();
        }

        /// <summary>
        /// Initializes the <see cref="CommandService"/>.
        /// </summary>
        public static void Start(CommandServiceConfigBuilder cfg)
        {
            if (_commandServiceStarted) { Logger.LogWarning("Attempted to start the CommandService after it was already started. This action has no effect; consider checking your code and restarting your application to prevent this warning."); return; }
            if (cfg is null) throw new CommandOnBuildingException("CommandServiceConfig was null.");

            _tempCfg = cfg;

            Init();
        }

        //: Create code examples for this documentation
        /// <summary>
        /// Initializes the <see cref="CommandService"/>.
        /// </summary>
        public static void Start(Action<CommandServiceConfigBuilder> buildAction)
        {
            if (_commandServiceStarted) { Logger.LogWarning("Attempted to start the CommandService after it was already started. This action has no effect; consider checking your code and restarting to prevent this warning."); return; }

            if (buildAction is { }) buildAction(_tempCfg);
            else throw new CommandOnBuildingException("BuildAction for the CommandServiceConfig was null.");

            if (_tempCfg is null) throw new CommandOnBuildingException("CommandServiceConfig was null. Please check your BuildAction delegate and restart your application. If this issue persists, please contact the creator of this library.");

            Init();
        }

        /// <summary>
        /// Initializes the <see cref="CommandService"/>.
        /// </summary>
        private static void Init()
        {
            _ = _instance.Value;

            if (InternalClient is null && !GlobalConfig.EnableManualConfiguration)
            {
                if (Modules.Any(o => o.Value.Client is { }))
                {
                    foreach(var module in Modules.Values)
                    {
                        if (!module.UseClient) _notifier.Value.AddWarning($"{module.TypeStorage.FullName} not able to execute commands due to no client. (Set a default client if you want this module's commands to execute.)");
                    }
                }
                else throw new CommandOnBuildingException("The TelegramBotClient provided to the CommandService was null without Manual Configuration enabled, and there was no suitable TelegramBotClient provided for any module. Did you mean to enable Manual Configuration? Please verify that your CommandServiceConfig and ModuleBuilderConfigs are set-up properly before starting the CommandService.");
            }

            CheckCommandNameAmbiguity();
            AttemptStartClientsReceiving();

            _notifier.Value.NotifyAll();
            _tempModules.Clear();

            _commandServiceStarted.Value = true;

            //! Local functions...
            static void CheckCommandNameAmbiguity()
            {
                // Changes the format of the dictionary to exclude the ICommand, pairing type with command name (as string).
                var trueDuplicates = Commands
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Keys.Select(k => new string(k.Span.ToArray())));

                // Loops through the collection to find non-distinct elements among -all- IEnumerable<string> collections in the dictionary.
                // This reformats the dictionary into strings paired with a List of all types that use that string as a command name.
                var nonDistinct = new Dictionary<string, List<Type>>();
                foreach (var kvp in trueDuplicates)
                {
                    foreach (var name in kvp.Value)
                    {
                        if (!nonDistinct.ContainsKey(name)) nonDistinct[name] = new List<Type> { kvp.Key };
                        else nonDistinct[name].Add(kvp.Key);
                    }
                }

                // Groups by the number of types sharing that command name, enforcing that this collection of List<Type> is more than one, and returns to the same dictionary format as earlier.
                var commandDuplicates = nonDistinct
                    .GroupBy(g => g.Value.Count())
                    .Where(g => g.Key > 1)
                    .SelectMany(g => g)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                // Next bunch of declarations determine all ambiguous calls: prefixes, clients, and both.
                // If any of the "ambiguous" collections are greater than zero, the CommandServce will throw.
                var moduleMatches = Modules
                    .Where(g => commandDuplicates.Any(kvp => kvp.Value.Contains(g.Key)))
                    .ToList();

                var ambiguousPrefixesAndClients = moduleMatches
                    .GroupBy(kvp => new
                    {
                        kvp.Value.Config.Prefix,
                        kvp.Value.Client?.BotId,
                    })
                    .Where(g => g.Count() > 1)
                    .ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Key))
                    .OrderBy(g => g.Key);

                var ambiguousPrefixes = moduleMatches
                    .GroupBy(kvp => kvp.Value.Config.Prefix)
                    .Where(g => g.Count() > 1)
                    .ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Key))
                    .OrderBy(g => g.Key);

                var ambiguousClients = moduleMatches
                    .GroupBy(g => g.Value.Client?.BotId)
                    .Where(g => g.Count() > 1)
                    .ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Key))
                    .OrderBy(g => g.Key);

                bool toThrow, found_ambiguousPrefixesAndClients, found_ambiguousPrefixes, found_ambiguousClients;
                found_ambiguousPrefixesAndClients = ambiguousPrefixesAndClients.Count() > 0;
                found_ambiguousPrefixes = ambiguousPrefixes.Count() > 0;
                found_ambiguousClients = ambiguousClients.Count() > 0;
                toThrow = found_ambiguousPrefixesAndClients || found_ambiguousPrefixes || found_ambiguousClients;

                // Will throw. The rest of this is formatting the Exceptions to display.
                if (toThrow)
                {
                    List<Exception> exceptions = new List<Exception>();
                    string str_ambiguousPrefixesAndClients, str_ambiguousPrefixes, str_ambiguousClients;
                    if (found_ambiguousPrefixesAndClients)
                    {
                        str_ambiguousPrefixesAndClients = "The following command(s) were ambiguous due to sharing the same command name, command prefix, and TelegramBotClient: " + Environment.NewLine;
                        foreach (var kvp in commandDuplicates)
                        {
                            foreach (var type in kvp.Value)
                            {
                                foreach(var groupKvp in ambiguousPrefixesAndClients)
                                {
                                    if (!groupKvp.Value.Contains(type)) continue;

                                    var commandName = kvp.Key;
                                    var module = type;
                                    var prefix = groupKvp.Key.Prefix;
                                    var botId = groupKvp.Key.BotId ?? 0;

                                    str_ambiguousPrefixesAndClients += $"Command Name: \"{commandName}\", Command Class: \"{module.FullName}\", Command Prefix: \"{prefix}\", Client BotId: \"{botId}\"" + Environment.NewLine;
                                }
                            }
                        }
                        exceptions.Add(new DuplicateCommandException(str_ambiguousPrefixesAndClients));
                    }

                    if (found_ambiguousPrefixes)
                    {
                        str_ambiguousPrefixes = "The following command(s) were ambiguous due to sharing the same command name and command prefix: ";
                        foreach (var kvp in commandDuplicates)
                        {
                            foreach (var type in kvp.Value)
                            {
                                foreach (var groupKvp in ambiguousPrefixes)
                                {
                                    if (!groupKvp.Value.Contains(type)) continue;

                                    var commandName = kvp.Key;
                                    var module = type;
                                    var prefix = groupKvp.Key;

                                    str_ambiguousPrefixes += $"Command Name: \"{commandName}\", Command Class: \"{module.FullName}\", Command Prefix: \"{prefix}\"" + Environment.NewLine;
                                }
                            }
                        }
                        exceptions.Add(new DuplicateCommandException(str_ambiguousPrefixes));
                    }

                    if (found_ambiguousClients)
                    {
                        str_ambiguousClients = "";
                        foreach (var kvp in commandDuplicates)
                        {
                            foreach (var type in kvp.Value)
                            {
                                foreach (var groupKvp in ambiguousClients)
                                {
                                    if (!groupKvp.Value.Contains(type)) continue;

                                    var commandName = kvp.Key;
                                    var module = type;
                                    var botId = groupKvp.Key ?? 0;

                                    str_ambiguousClients += $"Command Name: \"{commandName}\", Command Class: \"{module.FullName}\", Client BotId: \"{botId}\"" + Environment.NewLine;
                                }
                            }
                        }
                        exceptions.Add(new DuplicateCommandException(str_ambiguousClients));
                    }

                    // Guaranteed to be at least one.
                    if (exceptions.Count() == 1) throw exceptions.First();
                    else throw new AggregateException("Multiple duplicate command name ambiguities found when attempting to build the CommandServce. Please address the following exceptions:" + Environment.NewLine, exceptions);
                }
            }
            static void AttemptStartClientsReceiving()
            {
                // All of the below attempts to start receiving updates for each client, unless full manual is enabled.
                Dictionary<int, (Type Module, ApiRequestException Exception)> exceptionModules = new Dictionary<int, (Type Module, ApiRequestException Exception)>();
                if(InternalClient is { })
                {
                    try { InternalClient.StartReceiving(Array.Empty<UpdateType>()); }
                    catch (ApiRequestException ex)
                    {
                        exceptionModules.Add(InternalClient.BotId, (typeof(CommandService), ex));
                    }
                }

                // Selects only the first instance of a client found based on its Id, so that it only receives updates -once-
                var distinctClients = ServicesCollection
                    .Where(kvp => kvp.Key != typeof(CommandService))
                    .GroupBy(kvp => kvp.Value.Client?.BotId)
                    .Select(g => g.First());

                foreach (var kvp in distinctClients)
                {
                    if(kvp.Value.Client is { })
                    {
                        try { kvp.Value.Client.StartReceiving(Array.Empty<UpdateType>()); }
                        catch (ApiRequestException ex)
                        {
                            exceptionModules.Add(kvp.Value.Client.BotId, (kvp.Key, ex));
                        }
                    }
                }

                // Exception formatting...
                if (exceptionModules.Count != 0)
                {
                    List<Exception> exceptions = new List<Exception>();
                    foreach (var keyValue in exceptionModules)
                    {
                        var clientIdsPerModule = ServicesCollection
                            .Where(kvp => kvp.Value.Client?.BotId == keyValue.Key)
                            .GroupBy(kvp => kvp.Value.Client?.BotId);
                        
                        string exceptionModuleString = "";
                        foreach (var group in clientIdsPerModule)
                        {
                            exceptionModuleString += $"TelegramBotClient (Id: {group.Key}) failed to initialize in module(s): " + Environment.NewLine;
                            var typesInCurrentClientGroup = group.Select(g => g.Key);
                            foreach(var type in typesInCurrentClientGroup)
                            {
                                string moduleAsText = type == typeof(CommandService) ? $"{type.FullName!} (default client)" : type.FullName ?? "NULL";
                                exceptionModuleString += "* " + moduleAsText + Environment.NewLine;
                            }
                        }

                        exceptionModuleString += " ... Due to an ApiRequestException: " + Environment.NewLine;
                        exceptions.Add(new InvalidConfigSettingsException(exceptionModuleString, keyValue.Value.Exception));
                    }

                    string parentExceptionString = $"There was an issue initializing one or more of the TelegramBotClient(s) for the CommandService. Please double-check your client, token, or ConfigBuilder to make sure it has been provided correctly. If this issue persists and you believe it is in error, please submit a bug report on the FluentCommands Github page.";
                    if (exceptions.Count == 1) throw new InvalidConfigSettingsException(parentExceptionString, exceptions.First());
                    else throw new InvalidConfigSettingsException(parentExceptionString, new AggregateException(exceptions));
                }
            }
        }
        #endregion

        #region Evaluate/ProcessInput Overloads
        //: BOLD that they should only be using these methods if they're an advanced user
        public static async Task Evaluate<TCommand>(TelegramBotClient clientOverride, CallbackQueryEventArgs e) where TCommand : class
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(typeof(TCommand), e, clientOverride).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming CallbackQueryEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        public static async Task Evaluate<TCommand>(TelegramBotClient clientOverride, ChosenInlineResultEventArgs e) where TCommand : class
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(typeof(TCommand), e, clientOverride).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming ChosenInlineResultEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        public static async Task Evaluate<TCommand>(TelegramBotClient clientOverride, InlineQueryEventArgs e) where TCommand : class
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(typeof(TCommand), e, clientOverride).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming InlineQueryEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        public static async Task Evaluate<TCommand>(TelegramBotClient clientOverride, MessageEventArgs e) where TCommand : class
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(typeof(TCommand), e, clientOverride).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming MessageEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        public static async Task Evaluate<TCommand>(TelegramBotClient clientOverride, UpdateEventArgs e) where TCommand : class
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(typeof(TCommand), e, clientOverride).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming UpdateEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        public static async Task Evaluate(TelegramBotClient clientOverride, CallbackQueryEventArgs e, Type commandModule)
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(commandModule, e, clientOverride).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming CallbackQueryEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        public static async Task Evaluate(TelegramBotClient clientOverride, ChosenInlineResultEventArgs e, Type commandModule)
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(commandModule, e, clientOverride).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming ChosenInlineResultEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        public static async Task Evaluate(TelegramBotClient clientOverride, InlineQueryEventArgs e, Type commandModule)
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(commandModule, e, clientOverride).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming InlineQueryEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        public static async Task Evaluate(TelegramBotClient clientOverride, MessageEventArgs e, Type commandModule)
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(commandModule, e, clientOverride).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming MessageEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        public static async Task Evaluate(TelegramBotClient clientOverride, UpdateEventArgs e, Type commandModule)
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(commandModule, e, clientOverride).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming UpdateEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }

        //! These are for internal eventhandler purposes only (within modules). Clients will automatically be evaluating with these methods.
        internal static async Task Evaluate_ToHandler<TCommand>(CallbackQueryEventArgs e) where TCommand : class
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(typeof(TCommand), e).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming CallbackQueryEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        internal static async Task Evaluate_ToHandler<TCommand>(ChosenInlineResultEventArgs e) where TCommand : class
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(typeof(TCommand), e).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming ChosenInlineResultEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        internal static async Task Evaluate_ToHandler<TCommand>(InlineQueryEventArgs e) where TCommand : class
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(typeof(TCommand), e).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming InlineQueryEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        internal static async Task Evaluate_ToHandler<TCommand>(MessageEventArgs e) where TCommand : class
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(typeof(TCommand), e).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming MessageEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }
        internal static async Task Evaluate_ToHandler<TCommand>(UpdateEventArgs e) where TCommand : class
        {
            if (e is { })
            {
                var (Success, Context, Module) = await CanEvaluate(typeof(TCommand), e).ConfigureAwait(false);
                if (Success) await Evaluate_Internal(Context!, Module!).ConfigureAwait(false); // Not null if true.
                else; //: log
            }
            else
            {
                await ThrowOrSwallow("Incoming UpdateEventArgs was null while attempting to evaluate input.", new NullReferenceException("The EventArgs was null.)"), e).ConfigureAwait(false);
                return;
            }
        }

        private static async Task ThrowOrSwallow(string s, Exception ex, TelegramUpdateEventArgs t)
        {
            if (GlobalConfig.SwallowCriticalExceptions) await Logger.LogFatal(s, ex, t);
            else throw ex;
        }

        /// <summary>Determines whether or not a command can be evaluated within this context.</summary>
        private static async Task<(bool Success, ICommandContext<TArgs>? Context, IReadOnlyModule? Module)> CanEvaluate<TArgs>(Type moduleType, TArgs e, TelegramBotClient? client = null) where TArgs : EventArgs
        {
            if (!_commandServiceStarted) return (false, null, null); //! Can't log this; logging requires the service to have been started.

            var argsType = typeof(TArgs);
            if (!_validEventArgs.Contains(argsType)) return (false, null, null); //: log
            if (!TelegramUpdateEventArgs.TryFromEventArgs(e, out var t)) return (false, null, null); //: log

            if (!Modules.TryGetValue(moduleType, out var module))
            {
                await ThrowOrSwallow($"There was no module found for the provided type: {moduleType.FullName}, during command evaluation.", new ArgumentException(), t).ConfigureAwait(false);
                return (false, null, null);
            }

            var noClient = ("Could not find a suitable TelegramBotClient to evaluate with. (Did you mean to provide a Client Override to the method?) Please register a TelegramBotClient with the AddClient method in either the CommandService, or in your command module's OnConfiguring method.", new NullReferenceException());
            if (client is null)
            {
                if (module.UseClient)
                {
                    if (module.Client is null) { await ThrowOrSwallow(noClient.Item1, noClient.Item2, t).ConfigureAwait(false); return (false, null, null); }
                    else client = module.Client;
                }
                else
                {
                    if (InternalClient is null) { await ThrowOrSwallow(noClient.Item1, noClient.Item2, t).ConfigureAwait(false); return (false, null, null); }
                    else client = InternalClient;
                }
            }

            var context = argsType switch
            {
                var _ when argsType == typeof(CallbackQueryEventArgs) => new CallbackQueryContext(moduleType, client, t.CallbackQueryEventArgs!) as ICommandContext<TArgs>,
                var _ when argsType == typeof(ChosenInlineResultContext) => new ChosenInlineResultContext(moduleType, client, t.ChosenInlineResultEventArgs!) as ICommandContext<TArgs>,
                var _ when argsType == typeof(InlineQueryEventArgs) => new InlineQueryContext(moduleType, client, t.InlineQueryEventArgs!) as ICommandContext<TArgs>,
                var _ when argsType == typeof(MessageEventArgs) => new MessageContext(moduleType, client, t.MessageEventArgs!) as ICommandContext<TArgs>,
                var _ when argsType == typeof(UpdateEventArgs) => new UpdateContext(moduleType, client, t.UpdateEventArgs!) as ICommandContext<TArgs>,
                _ => null
            };

            return (context is { }, context, module);
        }

        /// <summary>
        /// Processes the input for a user's given args from an Evaluate method.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        private static async Task Evaluate_Internal<TArgs>(ICommandContext<TArgs> context, IReadOnlyModule module) where TArgs : EventArgs
        {
            var moduleType = context.ModuleType_Internal;
            var logger = module.Logger;
            var client = context.Client;

            if (!TelegramUpdateEventArgs.TryFromEventArgs(context.EventArgs, out var e)) return; //: log
            _ = e.TryGetChat(out var chat);
            _ = e.TryGetUser(out var user);

            if (!AuxiliaryMethods.TryGetEventArgsRawInput(e, out ReadOnlyMemory<char> input)) return; //: log
            var botId = client.BotId;
            var config = module.Config;
            var prefix = config.Prefix; //! Not AsMemory() due to the possibility of this string changing elsewhere during execution

            await CheckDeleteAllUserInputs().ConfigureAwait(false);

            var state = await Cache.GetState(client.BotId, chat?.Id ?? 0, user?.Id ?? 0).ConfigureAwait(false);

            var (Success, OutCommand) = await TryProcessUserInput().ConfigureAwait(false);
            if (!Success) return; //! No logging necessary; logging was handled internally.

            //: come back to this; out the logger delegate itself. then, you dont have to worry about actual logger events happening (you can make the local functions static)

            await CheckDeleteCommandAfterCall().ConfigureAwait(false);

            try
            {
                await ProcessCommand(OutCommand!).ConfigureAwait(false); // Not null if true.
            }
            catch (ArgumentNullException) { throw; } //: Catch, default error message?, Log it, re-throw (if the config says to re-throw)
            catch (ArgumentException) { throw; } //: Catch, Log it, re-throw.
            catch (RegexMatchTimeoutException) { throw; } //: Catch, Log it, re-throw? maybe not re-throw
            catch (Exception) { throw; }

            #region Local Functions
            //// Config checks:
            async Task CheckDeleteAllUserInputs()
            {
                if (config.DeleteAllIncomingUserInputs)
                {
                    bool foundMessage = e.TryGetMessage(out var msg);
                    if (foundMessage)
                    {
                        try { await client.DeleteMessageAsync(chat?.Id, msg!.MessageId).ConfigureAwait(false); }
                        catch (Exception ex)
                        {
                            await logger.LogError("Error deleting user input on command evaluation. (Config => DeleteAllUserInputs)", ex, e).ConfigureAwait(false);
                        }
                    }
                    else; //: Log
                }
            }
            async Task CheckDeleteCommandAfterCall()
            {
                if (config.DeleteCommandAfterCall && !config.DeleteAllIncomingUserInputs)
                {
                    bool foundMessage = e.TryGetMessage(out var msg);
                    if (foundMessage)
                    {
                        try { await client.DeleteMessageAsync(chat?.Id, msg!.MessageId).ConfigureAwait(false); }
                        catch
                        {
                            //: log
                        }
                    }
                    else; //: Log
                }
            }
            //// Command processing:
            // Attempts to match the user input with a command in the dictionary.
            async Task<(bool Success, ICommand? OutCommand)> TryProcessUserInput()
            {
                ICommand? outCommand;
                if (state is { StepState: { CommandStepInfo: { IsEmpty: false } } })
                {
                    if (!TryGetCommand(state.StepState.CommandStepInfo.CurrentCommandName.AsMemory(), moduleType, out outCommand)) return (false, null);
                    else return (true, outCommand) /* check timer, activate timer */ ;
                }
                else
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
                                if (!TryGetCommand(commandName, moduleType, out outCommand))
                                {
                                    await logger.LogInfo($"{user?.ToFluentLogger() ?? "Unknown user"}").ConfigureAwait(false);
                                    return (false, null);
                                    //: improve this lol im going to sleep
                                }
                                else return (true, outCommand);
                            }
                            else return (false, null); //: log
                        }
                        else return (false, null); //: log
                    }
                    else return (false, null); //: log
                }
            }
            // Attempts to retrieve the command from the commands dictionary.
            static bool TryGetCommand(ReadOnlyMemory<char> inName, Type moduleType, out ICommand? outCommand)
            {
                if (!Commands.TryGetValue(moduleType, out var moduleDict)) { outCommand = null; return false; }
                if (!moduleDict.TryGetValue(inName, out outCommand)) return false;
                else return true;
            }
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
                                if (e.TryGetCallbackQueryEventArgs(out var args)) await c.Invoke((CallbackQueryContext)context).ConfigureAwait(false);
                                else; //: error message, log.
                                break;
                            }
                            case var _ when cmd is CommandBase<ChosenInlineResultContext, ChosenInlineResultEventArgs> c:
                            {
                                if (e.TryGetChosenInlineResultEventArgs(out var args)) await c.Invoke((ChosenInlineResultContext)context).ConfigureAwait(false);
                                else; //: log.
                                break;
                            }
                            case var _ when cmd is CommandBase<InlineQueryContext, InlineQueryEventArgs> c:
                            {
                                if (e.TryGetInlineQueryEventArgs(out var args)) await c.Invoke((InlineQueryContext)context).ConfigureAwait(false);
                                else; //: log.
                                break;
                            }
                            case var _ when cmd is CommandBase<MessageContext, MessageEventArgs> c:
                            {
                                if (e.TryGetMessageEventArgs(out var args)) await c.Invoke((MessageContext)context).ConfigureAwait(false);
                                else; //: log.
                                break;
                            }
                            case var _ when cmd is CommandBase<UpdateContext, UpdateEventArgs> c:
                            {
                                if (e.TryGetUpdateEventArgs(out var args)) await c.Invoke((UpdateContext)context).ConfigureAwait(false);
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
                        await Cache.AddOrUpdateState(state).ConfigureAwait(false);
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
            #endregion
        }
        #endregion

        #region Helper Methods

        #endregion

        #region Update Methods
        /// <summary>Exposes the private TempModules dictionary to update ModuleBuilders being built using the <see cref="Interfaces.BaseBuilderOfModule.ICommandBaseBuilderOfModule"/> format.</summary>
        internal static void UpdateBuilderInTempModules(ModuleBuilder m, Type t) => _tempModules[t] = m;


        //: Move this method to the keyboardbuilder class. Rewrite it to be less dependent on _tempModules.

        /// <summary>Updates keyboard rows by iterating through each row and checking each button for an implicitly-converted KeybaordButtonReference.</summary>
        internal static List<TButton[]> UpdateKeyboardRows<TButton>(List<TButton[]> rows, Type? parenTCommand = null, string? parentCommandName = null, bool isMenu = true)
            where TButton : IKeyboardButton
        {
            List<TButton[]> updatedKeyboardBuilder = new List<TButton[]>();
            Type buttonType = typeof(TButton);

            //: REFACTOR TO ONLY BE INLINE KEYBOARD BUTTONS.

            Func<string, Exception> keyboardException;
            string keyboardContainer;
            if (isMenu)
            {
                keyboardContainer = "Menu";

                keyboardException = (string s) =>
                {
                    return new MenuReplyMarkupException(s);
                };
            }
            else
            {
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

                                //var referencedModule = Commands
                                //    .Where(type => type.Name == moduleTextReference)
                                //    .FirstOrDefault();

                                var referencedModule = typeof(Type);

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
                                if (parenTCommand is null || !_tempModules[parenTCommand].ModuleCommandBases.ContainsKey(commandNameReference))
                                    throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command in Module: {parenTCommand?.FullName ?? "\"NULL (check stack trace)\""} that doesn't appear to exist. (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                referencedButton = _tempModules[parenTCommand].ModuleCommandBases[commandNameReference]?.InButton;
                            }

                            if (referencedButton is null || buttonType != referencedButton.GetType())
                            {
                                if (!GlobalConfig.BruteForceKeyboardReferences) throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command that doesn't have a keyboard button, and the configuration for this module ({parenTCommand?.FullName ?? "\"NULL (check stack trace)\""}) is set to terminate building when this occurs.");
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
