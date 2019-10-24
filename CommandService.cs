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
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FluentCommands.Tests.Unit")]

namespace FluentCommands
{
    //: Create methods that share internal cache services with outside dbs (EF core and such)

    /// <summary>
    /// The class responsible for handling the assembly and processing of <see cref="Command"/> objects.
    /// </summary>
    public static class CommandService
    {
        //! ENFORCE that ALL commands that have buttons have callbacks that reference that command and that command only.

        private static CommandServiceConfig _globalConfig;
        private static bool _lastMessageIsMenu;
        private static bool _commandsArePopulated = false;
        private static readonly Dictionary<Type, Dictionary<string, Command>> _commands = new Dictionary<Type, Dictionary<string, Command>>();
        private static readonly Dictionary<int, Dictionary<long, Message>> _messageBotCache = new Dictionary<int, Dictionary<long, Message>>();
        private static readonly Dictionary<int, Dictionary<long, Message>> _messageUserCache = new Dictionary<int, Dictionary<long, Message>>();
        private static readonly Type[] _telegramEventArgs = { typeof(CallbackQueryEventArgs), typeof(ChosenInlineResultEventArgs), typeof(InlineQueryEventArgs), typeof(MessageEventArgs), typeof(UpdateEventArgs) };
        internal static readonly Dictionary<Type, ModuleBuilder> Modules = new Dictionary<Type, ModuleBuilder>();

        /// <summary>
        /// Initializes the <see cref="CommandService"/> with a default <see cref="CommandServiceConfig"/>.
        /// </summary>
        public static void Start() => Init();

        /// <summary>
        /// Initializes the <see cref="CommandService"/>.
        /// </summary>
        public static void Start(CommandServiceConfig cfg) => Init(cfg);

        //: Create code examples for this documentation
        /// <summary>
        /// Initializes the <see cref="CommandService"/>.
        /// </summary>
        public static void Start(Action<CommandServiceConfig> buildAction)
        {
            CommandServiceConfig cfg = new CommandServiceConfig();
            buildAction(cfg);
            Init(cfg);
        }

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

            if (!Modules.ContainsKey(moduleType)) Modules.Add(moduleType, module);
            else throw new CommandOnBuildingException($"This module, {moduleType.Name}, appears to be a duplicate of another module with the same class type. You may only have one ModuleBuilder per class.");
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
            if (!Modules.ContainsKey(moduleType)) Modules.Add(moduleType, module);
            else throw new CommandOnBuildingException($"This module, {moduleType.Name}, appears to be a duplicate of another module with the same class type. You may only have one ModuleBuilder per class.");
            return Modules[moduleType];
        }

        /// <summary>
        /// This is the logic necessary to initialize the CommandService.
        /// </summary>
        /// <param name="cfg"></param>
        private static void Init(CommandServiceConfig cfg = default)
        {
            // Force-Exits the method if it has successfully completed before.
            if (_commandsArePopulated) return; //! Possibly log that it was started more than once

            // Global configuration as provided by the user.
            if (cfg == default) cfg = new CommandServiceConfig();
            _globalConfig = cfg;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> assemblyTypes = new List<Type>();

            foreach(var assembly in assemblies)
            {
                List<Type> internalTypes;

                try
                {
                    internalTypes = assembly.GetTypes().ToList();
                }
                catch(ReflectionTypeLoadException e)
                {
                    internalTypes = e.Types.Where(type => type != null).ToList();
                }

                assemblyTypes.AddRange(internalTypes);
            }

            Init_1_ModuleAssembler();
            Init_2_KeyboardAssembler();
            Init_3_CommandAssembler();

            _commandsArePopulated = true;

            void Init_1_ModuleAssembler()
            {
                // Collects *every* ModuleBuilder command context (all classes that derive from CommandContext)
                var allCommandContexts = assemblyTypes
                    .Where(type => type.BaseType != null && type.BaseType.IsAbstract && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(CommandModule<>))
                    .ToList();

                string unexpected = "An unexpected error occurred while building command module: ";

                if (allCommandContexts == null) throw new CommandOnBuildingException(unexpected + "Collection of command contexts was null. Please submit a bug report and/or contact the creator of this library if this issue persists.");

                foreach (var context in allCommandContexts)
                {
                    var moduleBuilder = new ModuleBuilder();

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
                    try { commandClass = (Type)property.GetValue(moduleContext, null) ?? throw new CommandOnBuildingException(); }
                    catch (ArgumentException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetParameterCountException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (MethodAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetInvocationException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    try
                    {
                        ModuleBuilderConfig moduleConfig = new ModuleBuilderConfig();

                        method_OnBuilding.Invoke(moduleContext, new object[] { moduleBuilder });
                        method_OnConfiguring.Invoke(moduleContext, new object[] { moduleConfig });

                        if(moduleConfig == null) moduleConfig = new ModuleBuilderConfig();
                        moduleBuilder.SetConfig(moduleConfig);
                    }
                    catch (TargetException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (ArgumentException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetInvocationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (TargetParameterCountException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (MethodAccessException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (InvalidOperationException ex) { throw new CommandOnBuildingException(unexpected, ex); }
                    catch (NotSupportedException ex) { throw new CommandOnBuildingException(unexpected, ex); }

                    foreach(var commandName in moduleBuilder.ModuleCommandBases.Keys)
                    {
                        CheckCommandNameValidity(commandName);
                    }

                    if (commandClass == null) throw new CommandOnBuildingException();
                    if (Modules.ContainsKey(commandClass)) throw new CommandOnBuildingException();
                    Modules.Add(commandClass, moduleBuilder);
                }
            }
            void Init_2_KeyboardAssembler()
            {
                foreach(var kvp in Modules.ToList())
                {
                    var commandClass = kvp.Key;
                    var module = kvp.Value;

                    foreach(var moduleKvp in module.ModuleCommandBases.ToList())
                    {
                        var commandName = moduleKvp.Key;
                        var commandBase = moduleKvp.Value;

                        if (commandBase.KeyboardInfo == null) continue;
                        else
                        {
                            if (commandBase.KeyboardInfo.InlineRows.Any()) commandBase.KeyboardInfo.UpdateInline(UpdateKeyboardRows<InlineKeyboardButton>(commandClass, commandName, commandBase.KeyboardInfo.InlineRows));
                            if (commandBase.KeyboardInfo.ReplyRows.Any()) commandBase.KeyboardInfo.UpdateReply(UpdateKeyboardRows<KeyboardButton>(commandClass, commandName, commandBase.KeyboardInfo.ReplyRows));

                            Modules[commandClass][commandName] = commandBase;
                        }
                    }
                }

                // Updates keyboard rows by iterating through each row and checking each button for an implicitly-converted KeybaordButtonReference.
                List<TButton[]> UpdateKeyboardRows<TButton>(Type parentModule, string parentCommandName, List<TButton[]> rows)
                    where TButton : IKeyboardButton
                {
                    List<TButton[]> updatedKeyboardBuilder = new List<TButton[]>();

                    foreach (var row in rows)
                    {
                        var updatedKeyboardButtons = new List<TButton>();

                        foreach (var button in row)
                        {
                            if (button != null
                             && button.Text != null
                             && button.Text.Contains("COMMANDBASEBUILDERREFERENCE"))
                            {
                                var match = FluentRegex.CheckButtonReference.Match(button.Text);
                                if (!match.Success)
                                {
                                    match = FluentRegex.CheckButtonLinkedReference.Match(button.Text);
                                    if(!match.Success) throw new CommandOnBuildingException($"Unknown error occurred while building command keyboards: button contained reference text \"{button.Text}\"");
                                    else UpdateButton(match, true);
                                }
                                else UpdateButton(match);
                                        
                                // Locates the reference being pointed to by this TButton and updates it.
                                void UpdateButton(Match m, bool isLinked = false)
                                {
                                    IKeyboardButton referencedButton;

                                    string commandNameReference = m.Groups[1].Value ?? throw new CommandOnBuildingException("An unknown error occurred while building command keyboards (command Name Reference was null).");

                                    if (isLinked)
                                    {
                                        string moduleTextReference = match.Groups[2].Value ?? throw new CommandOnBuildingException("An unknown error occurred while building command keyboards (module text reference was null).");

                                        var referencedModule = assemblyTypes
                                           .Where(type => type.Name == moduleTextReference)
                                           .FirstOrDefault();

                                        if (referencedModule == null) throw new CommandOnBuildingException($"Command \"{parentCommandName}\" has a KeyboardBuilder that references a module that doesn't appear to exist.");

                                        if (!(referencedModule.BaseType != null
                                            && referencedModule.BaseType.IsAbstract
                                            && referencedModule.BaseType.IsGenericType
                                            && referencedModule.BaseType.GetGenericTypeDefinition() == typeof(CommandModule<>)))
                                            throw new CommandOnBuildingException($"Command \"{parentCommandName}\" has a KeyboardBuilder that references a module that doesn't appear to be a valid command context: {referencedModule.FullName} (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                        if (!Modules[referencedModule].ModuleCommandBases.ContainsKey(commandNameReference))
                                            throw new CommandOnBuildingException($"Command \"{parentCommandName}\" has a KeyboardBuilder that references a command that doesn't exist in linked module: {referencedModule.FullName} (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                        referencedButton = Modules[referencedModule].ModuleCommandBases[commandNameReference]?.InButton;
                                    }
                                    else
                                    {
                                        if (!Modules[parentModule].ModuleCommandBases.ContainsKey(commandNameReference))
                                            throw new CommandOnBuildingException($"Command \"{parentCommandName}\" has a KeyboardBuilder that references a command that doesn't exist. (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                                        referencedButton = Modules[parentModule].ModuleCommandBases[commandNameReference]?.InButton;
                                    }

                                    if (referencedButton == null || (referencedButton != null && typeof(TButton) != referencedButton.GetType()))
                                    {
                                        //: Dependency: check the moduleconfig and see if it should throw here.

                                        if (false) throw new CommandOnBuildingException($"Command \"{parentCommandName}\" has a KeyboardBuilder that references a command that doesn't have a keyboard button, and the configuration for this module ({parentModule.FullName}) is set to terminate building when this occurs.");
                                        else
                                        {
                                            // Attempts to create a reference to the command when a button reference isn't available.

                                            switch (typeof(TButton))
                                            {
                                                case var t when typeof(TButton) == typeof(InlineKeyboardButton):
                                                    referencedButton = InlineKeyboardButton.WithCallbackData(commandNameReference, $"BUTTONREFERENCEDCOMMAND::{commandNameReference}::");
                                                    break;
                                                case var t when typeof(TButton) == typeof(KeyboardButton):
                                                    referencedButton = new KeyboardButton(commandNameReference);
                                                    break;
                                                default: // Should NEVER happen, ever
                                                    throw new CommandOnBuildingException($"An unknown exception occurred while building the keyboards for command {parentCommandName} (no type detected for TButton)");
                                            }

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
                // With modules assembled, can collect *every* method labeled as a Command:
                var allCommandMethods = assemblyTypes
                    .Where(type => type.IsClass && Modules.ContainsKey(type))
                    .SelectMany(type => type.GetMethods())
                    .Where(method => method.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                    .ToList();

                foreach (var method in allCommandMethods)
                {
                    var thisModule = method.DeclaringType;
                    var methodCommandAttributeName = method.GetCustomAttribute<CommandAttribute>().Name;

                    if (!_commands.ContainsKey(thisModule)) _commands[thisModule] = new Dictionary<string, Command>();

                    var thisCommandBase = Modules[thisModule].ModuleCommandBases?[methodCommandAttributeName];

                    if (thisCommandBase == null) TryAddCommand(new CommandBase(methodCommandAttributeName));
                    else TryAddCommand(thisCommandBase.ConvertToBase());

                    // Local function; attempts to add the Command to the dictionary. Throws on failure.
                    void TryAddCommand(CommandBase commandBase)
                    {
                        foreach (var alias in commandBase.Aliases) CheckCommandNameValidity(commandBase.Name, true, alias);

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
                                    AddCallbackQueryCommand();
                                    break;
                                case Type t when t == typeof(ChosenInlineResultEventArgs):
                                    AddChosenInlineResultCommand();
                                    break;
                                case Type t when t == typeof(InlineQueryEventArgs):
                                    AddInlineQueryCommand();
                                    break;
                                case Type t when t == typeof(MessageEventArgs):
                                    AddMessageCommand();
                                    break;
                                case Type t when t == typeof(UpdateEventArgs):
                                    AddUpdateCommand();
                                    break;
                            }

                            // Local functions for each command type.
                            void AddCallbackQueryCommand()
                            {
                                try
                                {
                                    var newCommand = new CallbackQueryCommand(commandBase, method);
                                    _commands[thisModule][methodCommandAttributeName] = newCommand;
                                    AddAliases(newCommand, commandBase.Aliases);
                                }
                                catch (ArgumentNullException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method was null.", e); }
                                catch (ArgumentException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: ", e); }
                                catch (MissingMethodException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method not found.", e); }
                                catch (MethodAccessException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method MUST be marked public.", e); }
                            }
                            void AddChosenInlineResultCommand()
                            {
                                try
                                {
                                    var newCommand = new ChosenInlineResultCommand(commandBase, method);
                                    _commands[thisModule][methodCommandAttributeName] = newCommand;
                                    AddAliases(newCommand, commandBase.Aliases);
                                }
                                catch (ArgumentNullException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method was null.", e); }
                                catch (ArgumentException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: ", e); }
                                catch (MissingMethodException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method not found.", e); }
                                catch (MethodAccessException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method MUST be marked public.", e); }
                            }
                            void AddInlineQueryCommand()
                            {
                                try
                                {
                                    var newCommand = new InlineQueryCommand(commandBase, method);
                                    _commands[thisModule][methodCommandAttributeName] = newCommand;
                                    AddAliases(newCommand, commandBase.Aliases);
                                }
                                catch (ArgumentNullException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method was null.", e); }
                                catch (ArgumentException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: ", e); }
                                catch (MissingMethodException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method not found.", e); }
                                catch (MethodAccessException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method MUST be marked public.", e); }
                            }
                            void AddMessageCommand()
                            {
                                try
                                {
                                    var newCommand = new MessageCommand(commandBase, method);
                                    _commands[thisModule][methodCommandAttributeName] = newCommand;
                                    AddAliases(newCommand, commandBase.Aliases);
                                }
                                catch (ArgumentNullException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method was null.", e); }
                                catch (ArgumentException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: ", e); }
                                catch (MissingMethodException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method not found.", e); }
                                catch (MethodAccessException e) { throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method MUST be marked public.", e); }
                            }
                            void AddUpdateCommand()
                            {
                                try
                                {
                                    var newCommand = new UpdateCommand(commandBase, method);
                                    _commands[thisModule][methodCommandAttributeName] = newCommand;
                                    AddAliases(newCommand, commandBase.Aliases);
                                }
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
                                    _commands[thisModule][alias] = commandToReference;
                                }
                            }
                        }
                        else throw new CommandOnBuildingException($"Command {methodCommandAttributeName}: method had invalid signature.");
                    }
                }
            }
        }

        public static async Task Evaluate(Type module, TelegramBotClient client, MessageEventArgs e) =>
            await Evaluate<module>(client, e);

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
        /// <returns></returns>
        public static async Task Evaluate<TModule>(TelegramBotClient client, MessageEventArgs e) where TModule : class
        {
            try
            {
                var botId = client.BotId;
                if (!_messageBotCache.ContainsKey(botId)) _messageBotCache.TryAdd(botId, new Dictionary<long, Message>());
                if (!_messageUserCache.ContainsKey(botId)) _messageUserCache.TryAdd(botId, new Dictionary<long, Message>());

                var messageChatId = e.Message.Chat.Id;
                if (!_messageBotCache[botId].ContainsKey(messageChatId)) _messageBotCache[botId].TryAdd(messageChatId, new Message());
                if (!_messageUserCache[botId].ContainsKey(messageChatId)) _messageUserCache[botId].TryAdd(messageChatId, new Message());

                //! last message received by bot (will be user)


                var rawInput = e.Message.Text ?? throw new ArgumentNullException();
                var thisConfig = Modules[typeof(TModule)].Config;
                var prefix = thisConfig.Prefix ?? throw new ArgumentNullException();
                try
                {
                    if (rawInput.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        string input = rawInput.Remove(rawInput.IndexOf(prefix, StringComparison.OrdinalIgnoreCase), rawInput.Length);
                        var commandMatch = FluentRegex.CheckCommand.Match(input);

                        if (!commandMatch.Success) return;
                        else
                        {
                            // This message *should* be a user attempting to interact with the bot; add it to the cache.
                            if (!e.Message.From.IsBot) _messageUserCache[botId][messageChatId] = e.Message;

                            var thisCommandName = commandMatch.Groups[1].Value;
                            var thisCommand = _commands[typeof(TModule)][thisCommandName];

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

            Command EvaluateCommand()
            {
                return null; //: Refactoring...
            }
            async Task ProcessCommand(Command c)
            {
                if(c is MessageCommand)
                {
                    var command = c as MessageCommand;
                    if (command.InvokeWithMenuItem != null)
                    {
                        var menu = await command.InvokeWithMenuItem(client, e);
                        //: check keyboards.
                        await SendMenu<TModule>(menu, command.ReplyKeyboard, client, e);
                    }
                    else if (command.Invoke != null) await command.Invoke(client, e);
                }
                else
                {
                    //switch (c)
                    //{
                    //    var c 
                    //}
                }
            }
        }

        public static async Task Evaluate<TModule>(TelegramBotClient client, CallbackQueryEventArgs e) where TModule : class
        {
            if (e.CallbackQuery.Message.MessageId != 0) _lastMessageIsMenu = false;
        }

        /// <summary>
        /// Checks a string to see if it successfully clears the conditions for a <see cref="Command"/> name.
        /// <para>Throws if it doesn't.</para>
        /// </summary>
        /// <exception cref="CommandOnBuildingException"></exception>
        /// <exception cref="InvalidCommandNameException"></exception>
        internal static void CheckCommandNameValidity(string commandName, bool isAlias = false, string aliasName = null)
        {
            if (isAlias)
            {
                if (string.IsNullOrWhiteSpace(aliasName))
                {
                    if (string.IsNullOrWhiteSpace(commandName)) throw new InvalidCommandNameException($"Command name AND alias were both null, empty, or whitespace.");
                }
                else CheckName(aliasName, isAlias);
            }
            else CheckName(commandName);

            void CheckName(string name, bool alias = false)
            {
                string nullOrWhitespace;
                string tooLong;
                string containsWhitespaceCharacters;
                string containsNonAlphanumericCharacters;
                string regexTimeout;
                string tempCommandName;
                string tempName;

                if (string.IsNullOrWhiteSpace(commandName)) tempCommandName = "NULL";
                else tempCommandName = commandName;
                if (string.IsNullOrWhiteSpace(name)) tempName = "NULL";
                else tempName = name;

                if (alias)
                {
                    nullOrWhitespace = $"Command \"{tempCommandName}\": Command had alias that was null, empty, or whitespace.";
                    tooLong = $"Command \"{tempCommandName}\": Alias \"{tempName}\" was too long — Command names and aliases may only be a maximum of 255 characters.";
                    containsWhitespaceCharacters = $"Command \"{tempCommandName}\": Alias \"{tempName}\" — Command names and aliases cannot contain whitespace characters.";
                    containsNonAlphanumericCharacters = $"Command \"{tempCommandName}\": Alias \"{tempName}\" — Command names and aliases cannot contain non-alphanumeric characters.";
                    regexTimeout = $"Command \"{tempCommandName}\": Alias \"{tempName}\" caused a Regex Timeout while checking if the command's name was valid: ";
                }
                else
                {
                    nullOrWhitespace = $"Command name was null, empty, or whitespace.";
                    tooLong = $"Command \"{tempName}\": Command names may only be a maximum of 255 characters.";
                    containsWhitespaceCharacters = $"Command \"{tempName}\": Command names cannot contain whitespace characters.";
                    containsNonAlphanumericCharacters = $"Command \"{tempName}\": — Command names and aliases cannot contain non-alphanumeric characters.";
                    regexTimeout = $"Command \"{tempName}\": caused a Regex Timeout while checking if the command's name was valid: ";
                }

                if (string.IsNullOrWhiteSpace(name)) throw new InvalidCommandNameException(nullOrWhitespace);
                if (name.Length > 255) throw new InvalidCommandNameException(tooLong);
                try
                {
                    if (FluentRegex.CheckForWhiteSpaces.IsMatch(name)) throw new InvalidCommandNameException(containsWhitespaceCharacters);
                    if (FluentRegex.CheckNonAlphanumeric.IsMatch(name)) throw new InvalidCommandNameException(containsNonAlphanumericCharacters);
                }
                catch (RegexMatchTimeoutException e)
                {
                    throw new CommandOnBuildingException(regexTimeout, e);
                }
            }
        }

        #region BotLastMessage Method Overloads
        /// <summary>
        /// Returns the last <see cref="Message"/> sent by the this <see cref="TelegramBotClient"/> for the <see cref="Chat"/> instance indicated by this <see cref="CallbackQueryEventArgs"/>. 
        /// <para>Returns <code>null</code> if not found.</para>
        /// </summary>
        /// <param name="client">The <see cref="TelegramBotClient"/> that sent the <see cref="Message"/> being retrieved in this method.</param>
        /// <param name="e">The <see cref="CallbackQueryEventArgs"/> being used to locate the last <see cref="Message"/>.</param>
        /// <returns>Returns the last <see cref="Message"/> sent by the bot in this <see cref="Chat"/> instance.</returns>
        public static Message BotLastMessage(TelegramBotClient client, CallbackQueryEventArgs e)
        {
            return _messageBotCache[client.BotId]?[(long)e?.CallbackQuery?.Message?.Chat?.Id];
        }

        /// <summary>
        /// Returns the last <see cref="Message"/> sent by the this <see cref="TelegramBotClient"/> for the <see cref="Chat"/> instance indicated by this <see cref="MessageEventArgs"/>. 
        /// <para>Returns <code>null</code> if not found.</para>
        /// </summary>
        /// <param name="client">The <see cref="TelegramBotClient"/> that sent the <see cref="Message"/> being retrieved in this method.</param>
        /// <param name="e">The <see cref="MessageEventArgs"/> being used to locate the last <see cref="Message"/>.</param>
        /// <returns>Returns the last <see cref="Message"/> sent by the bot in this <see cref="Chat"/> instance.</returns>
        public static Message BotLastMessage(TelegramBotClient client, MessageEventArgs e)
        {
            return _messageBotCache[client.BotId]?[(long)e?.Message?.Chat?.Id];
        }

        /// <summary>
        /// Returns the last <see cref="Message"/> sent by the this <see cref="TelegramBotClient"/> for the <see cref="Chat"/> instance indicated by this <see cref="UpdateEventArgs"/>. 
        /// <para>Returns <code>null</code> if not found.</para>
        /// </summary>
        /// <param name="client">The <see cref="TelegramBotClient"/> that sent the <see cref="Message"/> being retrieved in this method.</param>
        /// <param name="e">The <see cref="UpdateEventArgs"/> being used to locate the last <see cref="Message"/>.</param>
        /// <returns>Returns the last <see cref="Message"/> sent by the bot in this <see cref="Chat"/> instance.</returns>
        public static Message BotLastMessage(TelegramBotClient client, UpdateEventArgs e)
        {
            return _messageBotCache[client.BotId]
                ?[(long)(e?.Update?.CallbackQuery?.Message?.Chat?.Id
                      ?? e?.Update?.Message?.Chat?.Id)];
        }
        #endregion

        private static async Task SendMenu<TModule>(Menu menu, IReplyMarkup replyMarkup, TelegramBotClient client, MessageEventArgs e) where TModule : class
        {
            //? should this method be public? aimed at transforming MenuItems into replacements for the weird client methods
            //: additionally, please fix the signature of this method
            //? possibly duplicate this method to happen with menu items on their own, and rename this one to be "send menu internal handler" or something

            //! "SEND TO THIS" property should only accept an int or long and no enum
            //! the aim should be to only provide a SPECIFIC Id to send the menuitem to
            //! otherwise the default should ALWAYS be the chat id

            //? note, all of these things are purely for MenuItem objects. what happens within the methods that arent the returned MenuItem from the Command method are not of any concern.
            //? if the user wants to do weird junk, they can. ONLY be concerned about the RETURNED MENUITEM phase of the message sending process.

            var m = menu.MenuItem;

            //: Check if editable
            await client.SendAnimationAsync(m.SendToThis, m.Source, m.Duration, m.Width, m.Height, m.Thumbnail, m.Caption, m.ParseMode, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token);

            var config = Modules[typeof(TModule)].Config;
            if (m.SendToThis == default) m.SendToThis = (long)e?.Message?.Chat?.Id;

            switch (config.MenuMode)
            {
                case MenuMode.NoAction:
                    await NoAction();
                    break;
                case MenuMode.EditLastMessage:
                    //await EditLastMessage();
                    //!!! FOR REPLYKEYBOARDS, YOU _CANNOT_ EDIT THE REPLYMARKUP. 
                    //! THE OPTION COULD BE TO EDIT THE INLINEMARKUP OF THE PREVIOUS MESSAGE TO BE EMPTY, AND THEN SEND THE NEW MESSAGE
                    //! ALTERNATIVELY, THERE COULD BE NO EDIT, BUT THAT SEEMS KINDA BAD
                    break;
                case MenuMode.EditOrDeleteLastMessage:
                    await EditOrDeleteLastMessage();
                    break;
            }

            async Task NoAction()
            {
                //: need to figure out how to send reply keyboards appropriately.

                //: attempt sendmsg (with replykeyboard) then deleting the message, seeing if the keyboard stays


                Message msg;

                switch (m.MenuType)
                {
                    case MenuType.Animation:
                        msg = await client.SendAnimationAsync(m.SendToThis, m.Source, m.Duration, m.Width, m.Height, m.Thumbnail, m.Caption, m.ParseMode, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token);
                        break;
                    case MenuType.Audio:
                        msg = await client.SendAudioAsync(m.SendToThis, m.Source, m.Caption, m.ParseMode, m.Duration, m.Performer, m.Title, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.Thumbnail);
                        break;
                    case MenuType.Contact:
                        msg = await client.SendContactAsync(m.SendToThis, m.PhoneNumber, m.FirstName, m.LastName, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.VCard);
                        break;
                    case MenuType.Document:
                        msg = await client.SendDocumentAsync(m.SendToThis, m.Source, m.Caption, m.ParseMode, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.Thumbnail);
                        break;
                    case MenuType.Game:
                        //? How to send reply keyboard lmao
                        if(replyMarkup is InlineKeyboardMarkup || replyMarkup == null) msg = await client.SendGameAsync(m.SendToThis, m.ShortName, m.DisableNotification, m.ReplyToMessage.MessageId, (InlineKeyboardMarkup)replyMarkup, m.Token);
                        else msg = await client.SendGameAsync(m.SendToThis, m.ShortName, m.DisableNotification, m.ReplyToMessage.MessageId, cancellationToken: m.Token); //? Log this?
                        break;
                    case MenuType.Invoice:
                        if (_messageUserCache[client.BotId][e.Message.Chat.Id].Chat.Type != Telegram.Bot.Types.Enums.ChatType.Private) msg = new Message(); //? throw? log it? idk
                        else
                        {
                            if (replyMarkup is InlineKeyboardMarkup || replyMarkup == null)
                                msg = await client.SendInvoiceAsync((int)m.SendToThis, m.Title, m.Description, m.Payload, m.ProviderToken, m.StartParameter, m.Currency, m.Prices, m.ProviderData, m.PhotoUrl, m.PhotoSize, m.PhotoWidth, m.PhotoHeight, m.NeedsName, m.NeedsPhoneNumber, m.NeedsEmail, m.NeedsShippingAddress, m.IsFlexibile, m.DisableNotification, m.ReplyToMessage.MessageId, (InlineKeyboardMarkup)replyMarkup);
                            else
                                //: Log this
                                msg = await client.SendInvoiceAsync((int)m.SendToThis, m.Title, m.Description, m.Payload, m.ProviderToken, m.StartParameter, m.Currency, m.Prices, m.ProviderData, m.PhotoUrl, m.PhotoSize, m.PhotoWidth, m.PhotoHeight, m.NeedsName, m.NeedsPhoneNumber, m.NeedsEmail, m.NeedsShippingAddress, m.IsFlexibile, m.DisableNotification, m.ReplyToMessage.MessageId);
                        }
                        break;
                    case MenuType.MediaGroup:
                        msg = (await client.SendMediaGroupAsync(m.Media, m.SendToThis, m.DisableNotification, m.ReplyToMessage.MessageId, m.Token)).LastOrDefault();
                        break;
                    case MenuType.Photo:
                        msg = await client.SendPhotoAsync(m.SendToThis, m.Source, m.Caption, m.ParseMode, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token);
                        break;
                    case MenuType.Poll:
                        msg = await client.SendPollAsync(m.SendToThis, m.Question, m.Options, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token);
                        break;
                    case MenuType.Sticker:
                        msg = await client.SendStickerAsync(m.SendToThis, m.Source, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token);
                        break;
                    case MenuType.Text:
                        msg = await client.SendTextMessageAsync(m.SendToThis, m.TextString, m.ParseMode, m.DisableWebPagePreview, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token);
                        break;
                    case MenuType.Venue:
                        msg = await client.SendVenueAsync(m.SendToThis, m.Latitude, m.Longitude, m.Title, m.Address, m.FourSquareId, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.FourSquareType);
                        break;
                    case MenuType.Video:
                        msg = await client.SendVideoAsync(m.SendToThis, m.Source, m.Duration, m.Width, m.Height, m.Caption, m.ParseMode, m.SupportsStreaming, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.Thumbnail);
                        break;
                    case MenuType.VideoNote:
                        msg = await client.SendVideoNoteAsync(m.SendToThis, m.SourceVideoNote, m.Duration, m.Length, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.Thumbnail);
                        break;
                    case MenuType.Voice:
                        msg = await client.SendVoiceAsync(m.SendToThis, m.Source, m.Caption, m.ParseMode, m.Duration, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token);
                        break;
                    default:
                        msg = new Message();
                        return;
                }

                _messageBotCache[client.BotId][e.Message.Chat.Id] = msg;
            }

            //async Task EditLastMessage()
            //{
            //    Message msgToEdit;

            //    switch (m.MenuType)
            //    {
            //        case MenuType.Animation:
            //            msgToEdit = _messageBotCache[client.BotId][e.Message.Chat.Id];
            //            if(replyMarkup is InlineKeyboardMarkup || replyMarkup == null)
            //            {
            //                await client.EditMessageCaptionAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, m.Caption, (InlineKeyboardMarkup)replyMarkup);
            //                await client.EditMessageMediaAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, new InputMediaAnimation(m.Source.Url), (InlineKeyboardMarkup)replyMarkup, m.Token);
            //            }
            //            else
            //            {
            //                //? how to send keyboard properly lmao???
            //                await client.EditMessageMediaAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, new InputMediaAnimation(m.Source.Url), null, m.Token);
            //                await client.EditMessageCaptionAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, m.Caption, (InlineKeyboardMarkup)replyMarkup);
            //            }
            //            break;
            //        case MenuType.Audio:
            //            await client.EditMessageMediaAsync();
            //            await client.EditMessageCaptionAsync();
            //            break;
            //        case MenuType.Contact:
            //            await NoAction();
            //            break;
            //        case MenuType.Document:
            //            await client.EditMessageMediaAsync();
            //            await client.EditMessageCaptionAsync();
            //            break;
            //        case MenuType.Game:
            //            await client.EditMessageTextAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, )
            //            break;
            //        case MenuType.Invoice:
            //            await NoAction();
            //            break;
            //        case MenuType.MediaGroup:
            //            break;
            //        case MenuType.Photo:
            //            await client.EditMessageMediaAsync();
            //            await client.EditMessageCaptionAsync();
            //            break;
            //        case MenuType.Poll:
            //            await NoAction();
            //            break;
            //        case MenuType.Sticker:
            //            await NoAction();
            //            break;
            //        case MenuType.Text:
            //            await client.EditMessageTextAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, )
            //            break;
            //        case MenuType.Venue:
            //            await NoAction();
            //            break;
            //        case MenuType.Video:
            //            await client.EditMessageMediaAsync();
            //            await client.EditMessageCaptionAsync();
            //            break;
            //        case MenuType.VideoNote:
            //            await NoAction();
            //            break;
            //        case MenuType.Voice:
            //            await NoAction();
            //            break;
            //    }
            //}
            async Task EditOrDeleteLastMessage()
            {
                switch (m.MenuType)
                {
                    case MenuType.Animation:
                        break;
                    case MenuType.Audio:
                        break;
                    case MenuType.Contact:
                        break;
                    case MenuType.Document:
                        break;
                    case MenuType.Game:
                        break;
                    case MenuType.Invoice:
                        break;
                    case MenuType.MediaGroup:
                        break;
                    case MenuType.Photo:
                        break;
                    case MenuType.Poll:
                        break;
                    case MenuType.Sticker:
                        break;
                    case MenuType.Text:
                        break;
                    case MenuType.Venue:
                        break;
                    case MenuType.Video:
                        break;
                    case MenuType.VideoNote:
                        break;
                    case MenuType.Voice:
                        break;
                }
            }
        }
    }
}
