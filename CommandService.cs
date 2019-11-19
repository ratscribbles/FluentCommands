using System;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using FluentCommands.Builders;
using FluentCommands.CommandTypes;
using FluentCommands.Interfaces;
using FluentCommands.Attributes;
using FluentCommands.Exceptions;
using FluentCommands.Menus;
using FluentCommands.Helper;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using FluentCommands.Logging;
using System.Diagnostics;

[assembly: InternalsVisibleTo("FluentCommands.Tests.Unit")]

namespace FluentCommands
{
    //: Create methods that share internal cache services with outside dbs (EF core and such)

    /// <summary>
    /// The class responsible for handling the assembly and processing of <see cref="Command"/> objects.
    /// </summary>
    public sealed class CommandService
    {
        private static readonly Lazy<CommandService> _instance = new Lazy<CommandService>(() => new CommandService(_tempCfg));
        private static readonly Lazy<CommandServiceLogger> _logger = new Lazy<CommandServiceLogger>(() => new CommandServiceLogger());
        private static readonly Lazy<EmptyLogger> _emptyLogger = new Lazy<EmptyLogger>(() => new EmptyLogger());
        private static readonly IReadOnlyCollection<Type> _assemblyTypes;
        private static readonly IReadOnlyCollection<Type> _telegramEventArgs = new[] { typeof(CallbackQueryEventArgs), typeof(ChosenInlineResultEventArgs), typeof(InlineQueryEventArgs), typeof(MessageEventArgs), typeof(UpdateEventArgs) };
        /// <summary>Last message(s) sent by the bot.<para>int is botId, long is chatId.</para></summary>
        private static readonly Dictionary<int, ConcurrentDictionary<long, Message[]>> _botLastMessages = new Dictionary<int, ConcurrentDictionary<long, Message[]>>();
        private static readonly Dictionary<int, Dictionary<long, Message>> _messageUserCache = new Dictionary<int, Dictionary<long, Message>>();
        private static readonly Dictionary<Type, ModuleBuilder> _tempModules = new Dictionary<Type, ModuleBuilder>();
        private static Toggle _lastMessageIsMenu = new Toggle(false);
        private static ToggleOnce _commandServiceStarted = new ToggleOnce(false);
        private static CommandServiceConfig _tempCfg = new CommandServiceConfig();
        ///////
        private readonly CommandServiceConfig _config;
        private readonly IReadOnlyDictionary<Type, IReadOnlyModule> _modules;
        private readonly IReadOnlyDictionary<Type, IReadOnlyDictionary<string, Command>> _commands;
        ///////
        private static CommandService Instance { get { return _instance.Value; } }
        private static IReadOnlyDictionary<Type, IReadOnlyDictionary<string, Command>> Commands { get { return _instance.Value._commands; } }
        internal static IReadOnlyDictionary<Type, IReadOnlyModule> Modules { get { return _instance.Value._modules; } }
        internal static IFluentLogger Logger { get { return _logger.Value; } }
        internal static IFluentLogger EmptyLogger { get { return _emptyLogger.Value; } }
        internal static CommandServiceConfig GlobalConfig { get { return _instance.Value._config; } }

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
        /// Constructor for use only with the singleton. Enforces that these objects are completely unable to be modified and are read-only. Populates the following:
        /// <para>- Modules readonly dictionary</para>
        /// <para>- Commands readonly dictionary</para>
        /// <para>- Global config object</para>
        /// </summary>
        private CommandService(CommandServiceConfig cfg) 
        {
            _config = cfg;

            if (cfg.UseLoggingEventHandler is { }) _logger.Value.LoggingEvent += cfg.UseLoggingEventHandler;

            var tempCommands = new Dictionary<Type, Dictionary<string, Command>>();

            Init_1_ModuleAssembler();
            Init_2_KeyboardAssembler();
            Init_3_CommandAssembler();

            var tempCommandsToReadOnly = new Dictionary<Type, IReadOnlyDictionary<string, Command>>();
            foreach (var kvp in tempCommands.ToList()) tempCommandsToReadOnly.Add(kvp.Key, kvp.Value);

            _modules = _tempModules.ToDictionary(kvp => kvp.Key, kvp => (IReadOnlyModule)kvp.Value);
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
                    try { commandClass = (Type?)property.GetValue(moduleContext, null) ?? throw new CommandOnBuildingException(); }
                    catch (ArgumentException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetParameterCountException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (MethodAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetInvocationException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    var moduleBuilder = new ModuleBuilder(commandClass);

                    try
                    {
                        ModuleBuilderConfig moduleConfig = new ModuleBuilderConfig();

                        method_OnBuilding.Invoke(moduleContext, new object[] { moduleBuilder });
                        if (moduleBuilder is null) throw new CommandOnBuildingException(); //: describe in detail

                        method_OnConfiguring.Invoke(moduleContext, new object[] { moduleConfig });
                        if (moduleConfig is null) throw new CommandOnBuildingException(); //: describe in detail

                        moduleBuilder.SetConfig(moduleConfig);

                        if (moduleConfig.LogModuleActivities)
                        {
                            if (moduleConfig.UseLoggingEventHandler is { }) moduleBuilder.SetHandler(moduleConfig.UseLoggingEventHandler);
                            else throw new CommandOnBuildingException($"Module {moduleBuilder.TypeStorage.FullName} has logging enabled, but it has no event handler set. Please double check your code and make sure to add an event handler in your OnConfiguring method for this module through the UseLoggingEventHandler property.");
                        }
                    }
                    catch (TargetException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (ArgumentException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetInvocationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetParameterCountException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (MethodAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (InvalidOperationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (NotSupportedException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    foreach (var commandName in moduleBuilder.AsReadOnly().ModuleCommandBases.Keys)
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

                    foreach (var moduleKvp in module.AsReadOnly().ModuleCommandBases.ToList())
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
                    ModuleBuilderConfig? config = _tempModules[parentModule ?? throw new InvalidKeyboardRowException("Error updating keyboard rows. (Keyboard belonged to a Command, but the Command's module type was null.")]?.Config
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

                                        if (!_tempModules[referencedModule].AsReadOnly().ModuleCommandBases.ContainsKey(commandNameReference))
                                            throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command that doesn't exist in linked module: {referencedModule?.FullName ?? "NULL"} (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                        referencedButton = _tempModules[referencedModule].AsReadOnly().ModuleCommandBases[commandNameReference]?.InButton;
                                    }
                                    else
                                    {
                                        if (parentModule is null || !_tempModules[parentModule].AsReadOnly().ModuleCommandBases.ContainsKey(commandNameReference))
                                            throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command in Module: {parentModule?.FullName ?? "\"NULL (check stack trace)\""} that doesn't appear to exist. (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                        referencedButton = _tempModules[parentModule].AsReadOnly().ModuleCommandBases[commandNameReference]?.InButton;
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

                foreach (var method in allCommandMethods)
                {
                    var thisModule = method.DeclaringType ?? throw new CommandOnBuildingException("Error getting the DeclaringType (module) of a method while command building. (Returned null.)");
                    var methodCommandAttributeName = method.GetCustomAttribute<CommandAttribute>()?.Name ?? throw new CommandOnBuildingException($"Error determining the Command Attribute name of a method in module: {thisModule.FullName ?? "NULL"} while command building. (Returned null.)");

                    if (!tempCommands.ContainsKey(thisModule)) tempCommands[thisModule] = new Dictionary<string, Command>();

                    var thisCommandBase = _tempModules[thisModule].AsReadOnly().ModuleCommandBases[methodCommandAttributeName];

                    if (thisCommandBase is null) TryAddCommand(new CommandBaseBuilder(methodCommandAttributeName));
                    else TryAddCommand(thisCommandBase);

                    // Local function; attempts to add the Command to the dictionary. Throws on failure.
                    void TryAddCommand(CommandBaseBuilder commandBase)
                    {
                        foreach (var alias in commandBase.InAliases) AuxiliaryMethods.CheckCommandNameValidity(commandBase.Name, true, alias);

                        // Checks the incoming method for signature validity; throws if not valid.
                        if (!method.IsStatic
                          && (method.ReturnType == typeof(Task) || method.ReturnType == typeof(Task<Message>))
                          && method.GetParameters().Length == 2
                          && (_telegramEventArgs.Contains(method.GetParameters()[1].ParameterType)))
                        {
                            // Filters based on method's EventArgs parameter type; adds the Command type that matches.
                            switch (method.GetParameters()[1].ParameterType)
                            {
                                case Type t when t == typeof(CallbackQueryEventArgs):
                                    AddCommand<CallbackQueryCommand>(commandBase);
                                    break;
                                case Type t when t == typeof(ChosenInlineResultEventArgs):
                                    AddCommand<ChosenInlineResultCommand>(commandBase);
                                    break;
                                case Type t when t == typeof(InlineQueryEventArgs):
                                    AddCommand<InlineQueryCommand>(commandBase);
                                    break;
                                case Type t when t == typeof(MessageEventArgs):
                                    AddCommand<MessageCommand>(commandBase);
                                    break;
                                case Type t when t == typeof(UpdateEventArgs):
                                    AddCommand<UpdateCommand>(commandBase);
                                    break;
                            }

                            // Adds the finished command to the command list.
                            void AddCommand<T>(CommandBaseBuilder c) where T : Command
                            {
                                try
                                {
                                    T? newCommand = (typeof(T)) switch
                                    {
                                        var _ when typeof(T) == typeof(CallbackQueryCommand) => new CallbackQueryCommand(c, method, thisModule) as T,
                                        var _ when typeof(T) == typeof(ChosenInlineResultCommand) => new ChosenInlineResultCommand(c, method, thisModule) as T,
                                        var _ when typeof(T) == typeof(InlineQueryCommand) => new InlineQueryCommand(c, method, thisModule) as T,
                                        var _ when typeof(T) == typeof(MessageCommand) => new MessageCommand(c, method, thisModule) as T,
                                        var _ when typeof(T) == typeof(UpdateCommand) => new UpdateCommand(c, method, thisModule) as T,
                                        _ => throw new CommandOnBuildingException($"Command {c?.Name ?? "NULL"} failed to build. (Could not cast to proper command type. If you encounter this error, please notify the creator of the library. This should never happen.)")
                                    };

                                    if (newCommand is null) throw new CommandOnBuildingException($"Command {c?.Name ?? "NULL"} failed to build. (Attempt to add command resulted in a null command.)");

                                    tempCommands[thisModule][methodCommandAttributeName] = newCommand;
                                    AddAliases(newCommand, commandBase.InAliases);
                                }
                                catch (CommandOnBuildingException e) { throw e; } // For exceptional cases outlined above
                                catch (ArgumentNullException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method was null.", e); }
                                catch (ArgumentException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: ", e); }
                                catch (MissingMethodException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method not found.", e); }
                                catch (MethodAccessException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method MUST be marked public.", e); }
                            }

                            // Adds aliases for the command being added to the dictionary.
                            void AddAliases(Command commandToReference, string[] aliases)
                            {
                                foreach (string alias in aliases)
                                {
                                    tempCommands[thisModule][alias] = commandToReference;
                                }
                            }
                        }
                        else throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method had invalid signature.");
                    }
                }
            }
        }

        #region Start Overloads
        /// <summary>
        /// Initializes the <see cref="CommandService"/> with a default <see cref="CommandServiceConfig"/>.
        /// </summary>
        public static void Start() 
        {
            if (_commandServiceStarted) { Task.Run(async () => await Logger.Warning("Attempted to start the CommandService after it was already started. This action has no effect. Please consider checking your code and restarting your application to prevent this warning.").ConfigureAwait(false)); return; }
            _ = Instance;
            _commandServiceStarted.Value = true;
        }

        /// <summary>
        /// Initializes the <see cref="CommandService"/>.
        /// </summary>
        public static void Start(CommandServiceConfig cfg)
        {
            if (_commandServiceStarted) { Task.Run(async () => await Logger.Warning("Attempted to start the CommandService after it was already started. This action has no effect; consider checking your code and restarting your application to prevent this warning.").ConfigureAwait(false)); return; }
            if (cfg is null) throw new CommandOnBuildingException("CommandServiceConfig was null.");

            _tempCfg = cfg;
            _ = Instance;
            _commandServiceStarted.Value = true;
        }

        //: Create code examples for this documentation
        /// <summary>
        /// Initializes the <see cref="CommandService"/>.
        /// </summary>
        public static void Start(Action<CommandServiceConfig> buildAction)
        {
            if (_commandServiceStarted) { Task.Run(async () => await Logger.Warning("Attempted to start the CommandService after it was already started. This action has no effect; consider checking your code and restarting to prevent this warning.").ConfigureAwait(false)); return; }
            
            CommandServiceConfig cfg = new CommandServiceConfig();

            if (buildAction is { }) buildAction(cfg);
            else throw new CommandOnBuildingException("BuildAction for the CommandServiceConfig was null.");

            if (cfg is null) throw new CommandOnBuildingException("CommandServiceConfig was null. Please check your BuildAction delegate and restart your application. If this issue persists, please contact the creator of this library.");

            _tempCfg = cfg;
            _ = Instance;
            _commandServiceStarted.Value = true;
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
        /// <summary>
        /// Evaluates the user's message to check and execute a command if a command was issued.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="client"></param>
        /// <param name="e"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        /// <exception cref="Exception"></exception>
        public static async Task Eval_Oldlogic<TModule>(TelegramBotClient client, MessageEventArgs e) where TModule : class
        {
            try
            {
                var botId = client.BotId;
                if (!_botLastMessages.ContainsKey(botId)) _botLastMessages.TryAdd(botId, new ConcurrentDictionary<long, Message[]>());
                if (!_messageUserCache.ContainsKey(botId)) _messageUserCache.TryAdd(botId, new Dictionary<long, Message>());

                var messageChatId = e.Message.Chat.Id;
                if (!_botLastMessages[botId].ContainsKey(messageChatId)) _botLastMessages[botId].TryAdd(messageChatId, new Message[] { });
                if (!_messageUserCache[botId].ContainsKey(messageChatId)) _messageUserCache[botId].TryAdd(messageChatId, new Message());

                //! last message received by bot (will be user)


                var rawInput = e.Message.Text ?? throw new ArgumentNullException();
                var thisConfig = _tempModules[typeof(TModule)].Config;
                var prefix = thisConfig.Prefix ?? throw new ArgumentNullException();
                try
                {
                    if (rawInput.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        string input = rawInput.Substring(prefix.Length);
                        var commandMatch = FluentRegex.CheckCommand.Match(input);

                        if (!commandMatch.Success) return;
                        else
                        {
                            // This message *should* be a user attempting to interact with the bot; add it to the cache.
                            if (!e.Message.From.IsBot) _messageUserCache[botId][messageChatId] = e.Message;

                            var thisCommandName = commandMatch.Groups[1].Value;
                            var thisCommand = Commands[typeof(TModule)][thisCommandName];

                            await ProcessCommand(thisCommand);
                        }
                    }
                }
                catch (ArgumentNullException) { return; } // Catch, default error message?, Log it, re-throw.
                catch (ArgumentException) { return; } // Catch, Log it, re-throw.
                catch (RegexMatchTimeoutException) { return; } // Catch, Log it, re-throw.
            }
            catch (NullReferenceException ex) { return; } // Catch, Log it, re-throw.
            catch (ArgumentNullException ex) { return; } // Catch, Log it, re-throw.
            catch (Exception ex) { return; } // Catch, Log it, re-throw.

            async Task ProcessCommand(Command c)
            {
                if (c is MessageCommand)
                {
                    var command = c as MessageCommand;
                    if (command is { })
                    {
                        if (command.InvokeWithMenuItem != null)
                        {
                            var menu = await command.InvokeWithMenuItem(client, e);
                            //: check keyboards.
                            //await SendMenu<TModule>(menu, command.ReplyMarkup, client, e)
                        }
                        else if (command.Invoke != null) await command.Invoke(client, e);
                    }
                }
                // Do nothing if it's not the right type.
            }
        }
        public static async Task Eval_oldLogic<TModule>(TelegramBotClient client, CallbackQueryEventArgs e) where TModule : class
        {
            if (e.CallbackQuery.Message.MessageId != 0)
            {
                if (_lastMessageIsMenu == false) _lastMessageIsMenu.Flip();
            }
        }


        public static async Task Evaluate<TModule>(TelegramBotClient client, CallbackQueryEventArgs e) where TModule : CommandModule<TModule> =>
            await ProcessInput(typeof(TModule), client, e);
        public static async Task Evaluate(Type module, TelegramBotClient client, CallbackQueryEventArgs e) =>
            await ProcessInput(module, client, e);

        public static async Task Evaluate<TModule>(TelegramBotClient client, ChosenInlineResultEventArgs e) where TModule : CommandModule<TModule> =>
            await ProcessInput(typeof(TModule), client, e);
        public static async Task Evaluate(Type module, TelegramBotClient client, ChosenInlineResultEventArgs e) =>
            await ProcessInput(module, client, e);

        public static async Task Evaluate<TModule>(TelegramBotClient client, InlineQueryEventArgs e) where TModule : CommandModule<TModule> =>
            await ProcessInput(typeof(TModule), client, e);
        public static async Task Evaluate(Type module, TelegramBotClient client, InlineQueryEventArgs e) =>
            await ProcessInput(module, client, e);

        public static async Task Evaluate<TModule>(TelegramBotClient client, MessageEventArgs e) where TModule : CommandModule<TModule> =>
            await ProcessInput(typeof(TModule), client, e);
        public static async Task Evaluate(Type module, TelegramBotClient client, MessageEventArgs e) =>
            await ProcessInput(module, client, e);

        public static async Task Evaluate<TModule>(TelegramBotClient client, UpdateEventArgs e) where TModule : CommandModule<TModule> =>
            await ProcessInput(typeof(TModule), client, e);
        public static async Task Evaluate(Type module, TelegramBotClient client, UpdateEventArgs e) =>
            await ProcessInput(module, client, e);

        /// <summary>
        /// Processes the input for a user's given args from an Evaluate method.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        /// <exception cref="Exception"></exception>
        private static async Task ProcessInput(Type module, TelegramBotClient client, TelegramUpdateEventArgs e)
        {
            if (!_commandServiceStarted) return; // Log


            //: When redoing the exceptions here, make sure to reflect them both in this method's XML summary as well as the ones that use this method to function

            var logger = Modules[module].Logger;
            await logger.Debug("");

            if (module is null) throw new NullReferenceException("The Module was null."); //? Log?
            if (client is null) throw new NullReferenceException("The TelegramBotClient was null."); //? Log?
            if (e is null) throw new NullReferenceException("The EventArgs was null."); //? Log?

            if (!Modules.ContainsKey(module)) throw new CommandOnBuildingException(); //: Create a new exception for this case

            if (!AuxiliaryMethods.TryGetEventArgsRawInput(e, out string input)) return;
            var botId = client.BotId;
            var config = _tempModules[module]?.Config ?? new ModuleBuilderConfig();
            var prefix = config.Prefix;

            Command command;

            try
            {
                if (input.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(prefix.Length);
                    var commandMatch = FluentRegex.CheckCommand.Match(input);

                    if (!commandMatch.Success) return;
                    else
                    {
                        var commandName = commandMatch?.Groups[1]?.Value ?? "";
                        if (Commands.ContainsKey(module) && Commands[module].ContainsKey(commandName)) command = Commands[module][commandName];
                        else return;
                    }
                }
                else return;
            }
            catch (ArgumentNullException) { return; } // Catch, default error message?, Log it, re-throw.
            catch (ArgumentException) { return; } // Catch, Log it, re-throw.
            catch (RegexMatchTimeoutException) { return; } // Catch, Log it, re-throw.

            await ProcessCommand(command);

            //!
            //!
            //! YOU NEED TO PERFORM A CHECK TO SEE IF THE COMMAND TYPE AND EVENTARGS TYPE ARE THE SAME "BASE" (eg: MessageCommand matches with MessageEventArgs)
            //? evaluate what to do in the event this fails either with a global config property or by throwing or by doing nothing. pick one
            //!
            //!

            async Task ProcessCommand(Command cmd)
            {
                switch (cmd)
                {
                    case var _ when cmd is CallbackQueryCommand:
                        {
                            var c = cmd as ChosenInlineResultCommand;
                            var args = e.CallbackQueryEventArgs;

                            if (c is null || args is null) goto default;
                        }
                        return;
                    case var _ when cmd is ChosenInlineResultCommand:
                        {
                            var c = cmd as ChosenInlineResultCommand;
                            var args = e.ChosenInlineResultEventArgs;

                            if (c is null || args is null) goto default;
                        }
                        return;
                    case var _ when cmd is InlineQueryCommand:
                        {
                            var c = cmd as InlineQueryCommand;
                            var args = e.InlineQueryEventArgs;

                            if (c is null || args is null) goto default;

                        }
                        return;
                    case var _ when cmd is MessageCommand:
                        {
                            var c = cmd as MessageCommand;
                            var args = e.MessageEventArgs;

                            if (c is null || args is null) goto default;

                            if (c.InvokeWithMenuItem is { })
                            {
                                //var lmao = MenuItem.WithChatAction(Telegram.Bot.Types.Enums.ChatAction.RecordAudio).Audio().Source("").DoneAndSendTo(40).Send();
                                var menu = await c.InvokeWithMenuItem(client, args);
                                //: check keyboards.
                                //: Figure this out. lol. may want to remove the generics for the internal implementation
                                //: consider an extension method, or just a method added to the class itself
                                // await SendMenu<TModule>(menu, command.ReplyKeyboard, client, e);
                            }
                            else if (c.Invoke is { }) await c.Invoke(client, args);
                        }
                        return;
                    case var _ when cmd is UpdateCommand:
                        {
                            var c = cmd as UpdateCommand;
                            var args = e.UpdateEventArgs;

                            if (c is null || args is null) goto default;
                        }
                        return;
                    default:
                        // Perform logging.
                        return;
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
            long chatId;
            if (AuxiliaryMethods.TryGetEventArgsChatId(e, out long c_id)) chatId = c_id;
            else if (AuxiliaryMethods.TryGetEventArgsUserId(e, out int u_id)) chatId = u_id;
            else throw new Exception();

            bool success;
            byte attempts = 0;
            Message[]? m;
            do
            {
                success = _botLastMessages[client.BotId].TryGetValue(chatId, out m);
                attempts++;
            } 
            while (!success && attempts < 10);

            if (m is null) m = new Message[] { };

            return m;
        }

        /// <summary>
        /// Attempts to update the internal cache of the bot's last message(s).
        /// <para>Clears the history of all last messages, and adds the new one(s). Adds an empty collection if it fails.</para>
        /// </summary>
        internal static void UpdateBotLastMessages(TelegramBotClient client, long chatId, params Message[] messages)
        {
            bool success;
            byte attempts = 0;
            try
            {
                _botLastMessages[client.BotId].TryRemove(chatId, out _);

                do
                {
                    success = _botLastMessages[client.BotId].TryAdd(chatId, messages);
                    attempts++;
                } 
                while (!success && attempts < 10);

                if (!success && attempts == 10) _botLastMessages[client.BotId].TryAdd(chatId, new Message[] { });
            }
            catch (ArgumentNullException e) { return; } //: log it, check global cfg if should throw
            catch (OverflowException e) { return; } //: log it, check global cfg if should throw
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

            ModuleBuilderConfig? config;
            Func<string, Exception> keyboardException;
            string keyboardContainer;
            if (isMenu)
            {
                config = new ModuleBuilderConfig();

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

                                if (!Modules[referencedModule].ModuleCommandBases.ContainsKey(commandNameReference))
                                    throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command that doesn't exist in linked module: {referencedModule?.FullName ?? "NULL"} (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                referencedButton = Modules[referencedModule].ModuleCommandBases[commandNameReference]?.InButton;
                            }
                            else
                            {
                                if (parentModule is null || !Modules[parentModule].ModuleCommandBases.ContainsKey(commandNameReference))
                                    throw keyboardException($"{keyboardContainer} has a KeyboardBuilder that references a command in Module: {parentModule?.FullName ?? "\"NULL (check stack trace)\""} that doesn't appear to exist. (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                referencedButton = Modules[parentModule].ModuleCommandBases[commandNameReference]?.InButton;
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

        /// <summary>Passes args to the global event used by the <see cref="CommandService"/> Logger.</summary>
        internal static async Task PassToGlobalEvent(FluentLoggingEventArgs args)
        {
            await _logger.Value.GlobalEvent(args).ConfigureAwait(false);
        }
    }
}
