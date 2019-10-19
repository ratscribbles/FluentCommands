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
        private static readonly Dictionary<Type, Dictionary<string, Command>> _commands = new Dictionary<Type, Dictionary<string, Command>>();
        private static readonly Dictionary<Type, CommandModuleConfig> _moduleConfigs = new Dictionary<Type, CommandModuleConfig>();
        private static readonly Dictionary<int, Dictionary<long, Message>> _messageBotCache = new Dictionary<int, Dictionary<long, Message>>();
        private static readonly Dictionary<int, Dictionary<long, Message>> _messageUserCache = new Dictionary<int, Dictionary<long, Message>>();
        private static readonly Type[] _telegramEventArgs = { typeof(CallbackQueryEventArgs), typeof(ChosenInlineResultEventArgs), typeof(InlineQueryEventArgs), typeof(MessageEventArgs), typeof(UpdateEventArgs) };
        private static bool _commandsArePopulated = false;
        internal static readonly Dictionary<Type, List<CommandBase>> RawCommands = new Dictionary<Type, List<CommandBase>>();
        internal static readonly List<Type> Modules = new List<Type>();

        /// <summary>
        /// Builds a <see cref="Command"/> module.
        /// <para>Provided type is the class that contains the commands being built.</para>
        /// </summary>  
        /// <typeparam name="TModule">The class that contains the commands being built.</typeparam>
        /// <param name="buildAction">The "build action" is an <see cref="Action"/> that allows the user to configure the builder object—an alternate format to construct the <see cref="CommandModuleBuilder{TModule}"/>.</param>
        public static void Module<TModule>(Action<ICommandModuleBuilder<TModule>> buildAction) where TModule : class
        {
            //? This method could stay generic for the purposes of quick bots that only need a small model in the main 
            //? method of the program class, and the actual command implementations in a separate class

            var module = new CommandModuleBuilder<TModule>();

            buildAction(module);
            
            if (!Modules.Contains(typeof(TModule))) Modules.Add(typeof(TModule));
            if (!RawCommands.ContainsKey(typeof(TModule))) RawCommands.TryAdd(typeof(TModule), new List<CommandBase>());

            foreach (var item in module.BaseBuilderDictionary)
            {
                CheckCommandNameValidity(item.Key);
                var thisBase = item.Value.ConvertToBase();  
                RawCommands[thisBase.Module].Add(thisBase);
            }
        }

        /// <summary>
        /// Builds a <see cref="Command"/> module.
        /// <para>Provided type is the class that contains the commands being built.</para>
        /// </summary>
        /// <typeparam name="TModule">The class that contains the commands being built.</typeparam>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> to continue the fluent building process.</returns>
        public static ICommandModuleBuilder<TModule> Module<TModule>() where TModule : class
        {
            return new CommandModuleBuilder<TModule>();
        }

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
        /// This is the logic necessary to initialize the CommandService.
        /// </summary>
        /// <param name="cfg"></param>
        internal static void Init(CommandServiceConfig cfg = default)
        {
            // Force-Exits the method if it has successfully completed before.
            if (_commandsArePopulated) return;

            // Global configuration as provided by the user.
            if (cfg == default) cfg = new CommandServiceConfig();
            _globalConfig = cfg;

            Init_1_ModuleAssembler();

            // With modules assembled, can collect *every* method labeled as a Command:
            var allCommandMethods = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && Modules.Contains(type))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                .ToList();

            Init_2_KeyboardAssembler();
            Init_3_CommandAssembler();

            _commandsArePopulated = true;

            void Init_1_ModuleAssembler()
            {
                //// Module Builders ////
                ///
                // Collects *every* method labeled as ModuleBuilder...
                var allOnBuildingMethods = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type.IsClass)
                    .SelectMany(type => type.GetMethods())
                    .Where(method => method.GetCustomAttributes(typeof(ModuleBuilderAttribute), false).Length > 0)
                    .ToList();

                //... And invokes each one. Hopefully the user filled them properly.
                foreach (var method in allOnBuildingMethods)
                {
                    try { method.Invoke(new object(), null); }
                    catch
                    {
                        throw new CommandOnBuildingException($"ModuleBuilder method: \"{method.ToString()}\" in class: \"{method.DeclaringType}\" had an invalid signature. Please make sure all ModuleBuilder methods are static, void, and have no parameters.");
                    }
                }
            }
            void Init_2_KeyboardAssembler()
            {
                //! Keyboard assmbly required!!! replace the button references where available
                //? check for if the button exists, and then check the name and module for reference, and then replace the keyboard.
                //? requires a check of each commandbase's keyboard sadly

                //: todo: need to make sure to override the callback for commandbuilder command buttons, and to lmfao my bff jill (perform checks for inline/reply buttons)
                foreach (var key in RawCommands.Keys.ToList())
                {
                    var basesToUpdate = RawCommands[key];

                    foreach (var @base in basesToUpdate)
                    {
                        if (@base.KeyboardInfo != null)
                        {
                            if (@base.KeyboardInfo.Inline != null) @base.KeyboardInfo.Inline = UpdateKeyboardRows<InlineKeyboardBuilder, InlineKeyboardButton>(@base.KeyboardInfo.Inline.Rows) as InlineKeyboardBuilder;
                            if (@base.KeyboardInfo.Reply != null) @base.KeyboardInfo.Reply = UpdateKeyboardRows<ReplyKeyboardBuilder, KeyboardButton>(@base.KeyboardInfo.Reply.Rows) as ReplyKeyboardBuilder;
                        }
                        else continue;

                        IKeyboardBuilder<TBuilder, TButton> UpdateKeyboardRows<TBuilder, TButton>(List<TButton[]> list)
                            where TBuilder : IKeyboardBuilder<TBuilder, TButton>
                            where TButton : IKeyboardButton
                        {
                            IKeyboardBuilder<TBuilder, TButton> updatedKeyboardBuilder;

                            if (typeof(TButton) == typeof(InlineKeyboardButton)) updatedKeyboardBuilder = new InlineKeyboardBuilder(key) as IKeyboardBuilder<TBuilder, TButton>;
                            else if (typeof(TButton) == typeof(KeyboardButton)) updatedKeyboardBuilder = new ReplyKeyboardBuilder(key) as IKeyboardBuilder<TBuilder, TButton>;
                            else throw new CommandOnBuildingException("Unknown error occurred while assembling command keyboards."); // ABSOLUTELY should NEVER happen.

                            foreach (var row in list)
                            {
                                var updatedKeyboardButtons = new List<TButton>();

                                foreach (var button in row)
                                {
                                    if (button != null
                                     && button.Text != null
                                     && button.Text.Contains("COMMANDBASEBUILDERREFERENCE")
                                     && (Regex.IsMatch(button.Text, "COMMANDBASEBUILDERREFERENCE::(.{1,32767})::(.{1,255})")))
                                    {
                                        var match = Regex.Match(button.Text, "COMMANDBASEBUILDERREFERENCE::(.{1,32767})::(.{1,255})");
                                        string thisModuleTextReference = match.Groups[1].Value;
                                        string thisCommandNameReference = match.Groups[2].Value;

                                        var thisReferencedModule = key.Assembly.GetType(thisModuleTextReference);

                                        var thisReferencedButton = RawCommands[thisReferencedModule].Where(commandBase => commandBase.Name == thisCommandNameReference).FirstOrDefault().Button;

                                        //! change the callback data of the button here.
                                        //? check if the button is an inline button, then change the callback.
                                        // if(thisReferencedButton)
                                        if (thisReferencedButton == null) throw new CommandOnBuildingException($"Command \"{@base.Name}\" references command \"{thisCommandNameReference}\" as a Keyboard Button, but \"{thisCommandNameReference}\" doesn't have a Button assigned to it. (Please check the builder to make sure it exists, or remove the reference to \"{thisCommandNameReference}\" in the keyboard builder.)");
                                        else { updatedKeyboardButtons.Add((TButton)thisReferencedButton); continue; }
                                    }
                                    else { updatedKeyboardButtons.Add(button); continue; }
                                }
                                updatedKeyboardBuilder.AddRow(updatedKeyboardButtons.ToArray());
                            }
                            return updatedKeyboardBuilder;
                        }
                    }
                    RawCommands[key] = basesToUpdate;
                }
            }
            void Init_3_CommandAssembler()
            {
                foreach (var method in allCommandMethods)
                {
                    var thisModule = method.DeclaringType;
                    var methodCommandAttributeName = method.GetCustomAttribute<CommandAttribute>().Name;

                    if (!_commands.ContainsKey(thisModule)) _commands[thisModule] = new Dictionary<string, Command>();

                    var commandBases = RawCommands[thisModule].Where(x => x.Name == methodCommandAttributeName).ToList();

                    if (commandBases.Count() > 1) throw new DuplicateCommandException($"There was more than one command detected in module: {thisModule.Name}, with the command name: \"{methodCommandAttributeName}\"");
                    else if (commandBases.Count() == 0) { TryAddCommand(new CommandBase(methodCommandAttributeName)); }
                    else if (commandBases.Count() == 1) { TryAddCommand(commandBases[0]); }

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
                var thisConfig = _moduleConfigs[typeof(TModule)]; //: Add a null check here
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
                    if (string.IsNullOrWhiteSpace(commandName)) throw new InvalidCommandNameException($"Command name AND alias was null, empty, or whitespace.");
                }
                else CheckName(aliasName, isAlias);
            }
            else CheckName(commandName);

            void CheckName(string name, bool alias = false)
            {
                string nullOrWhitespace;
                string tooLong;
                string containsWhitespaceCharacters;
                string regexTimeout;

                if (alias)
                {
                    nullOrWhitespace = $"Command \"{commandName}\": Command had alias that was null, empty, or whitespace.";
                    tooLong = $"Command \"{commandName}\": Alias \"{name}\" was too long — Command names and aliases may only be a maximum of 255 characters.";
                    containsWhitespaceCharacters = $"Command \"{commandName}\": Alias \"{name}\" — Command names and aliases cannot contain whitespace characters.";
                    regexTimeout = $"Command \"{commandName}\": Alias \"{name}\" caused a Regex Timeout while checking if the command's name was valid: ";
                }
                else
                {
                    nullOrWhitespace = $"Command name was null, empty, or whitespace.";
                    tooLong = $"Command \"{name}\": Command names may only be a maximum of 255 characters.";
                    containsWhitespaceCharacters = $"Command \"{name}\": Command names cannot contain whitespace characters.";
                    regexTimeout = $"Command \"{name}\": caused a Regex Timeout while checking if the command's name was valid: ";
                }

                if (string.IsNullOrWhiteSpace(name)) throw new InvalidCommandNameException(nullOrWhitespace);
                if (name.Length > 255) throw new InvalidCommandNameException(tooLong);
                try
                {
                    if (FluentRegex.CheckForWhiteSpaces.IsMatch(name)) throw new InvalidCommandNameException(containsWhitespaceCharacters);
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

            var config = _moduleConfigs[typeof(TModule)];
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
