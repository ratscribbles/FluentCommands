using FluentCommands.Logging;
using System;
using FluentCommands.Menus;
using Telegram.Bot;
using FluentCommands.Cache;
using FluentCommands.Interfaces;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Commands.KeyboardTypes;
using FluentCommands.Commands;
using FluentCommands.Exceptions;

namespace FluentCommands
{
    /// <summary>
    /// This class constructs a <see cref="CommandServiceConfig"/>, which determines the settings for the <see cref="CommandService"/>.
    /// </summary>
    public class CommandServiceConfigBuilder : IFluentInterface
    {
        /// <summary>
        /// Creates a builder that constructs a <see cref="CommandServiceConfig"/>, which determines the settings for the <see cref="CommandService"/>.
        /// <para>Use this object with the <see cref="CommandService.Start(CommandServiceConfigBuilder)"/> method(s).</para>
        /// </summary>
        public CommandServiceConfigBuilder() { }
        internal CommandServiceConfig BuildConfig() => new CommandServiceConfig(this);

        #region Properties and Exposed Setters
        internal (int AmountOfMessages, TimeSpan PerTimeSpan) In_DefaultRateLimitPerUser { get; private set; }
        /// <summary>
        /// Sets the default rate limit (across all modules) for command inputs on a per-user basis.
        /// <para>Users will only be able to send a certain amount of commands within a specified <see cref="TimeSpan"/>. <strong>There is no limit by default.</strong></para>
        /// <para>This setting can be overridden in your <see cref="CommandModule{TCommand}.OnConfiguring(ModuleConfigBuilder)"/> method.</para>
        /// </summary>
        /// <param name="amountOfMessages"></param>
        /// <param name="perTimeSpan"></param>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder DefaultRateLimitPerUser(int amountOfMessages, TimeSpan perTimeSpan) 
        {
            In_DefaultRateLimitPerUser = (amountOfMessages, perTimeSpan);
            return this;
        } 

        internal bool In_BruteForceKeyboardReferences { get; private set; }
        /// <summary>
        /// Sets the <see cref="CommandService"/> to force <strong><em>all</em></strong> instances of a <see cref="KeyboardButtonLinkedReference"/> (found when using the <see cref="KeyboardBuilder"/> class) to create an <see cref="InlineKeyboardButton"/> for <strong><em>all</em></strong> command references, even if that command has no button reference set (in the <see cref="CommandModule{TCommand}.OnBuilding(ModuleBuilder)"/> method).
        /// <para>Without this setting enabled, any <see cref="KeyboardButtonLinkedReference"/> to a command without an <see cref="InlineKeyboardButton"/> reference will result in a <see cref="InvalidKeyboardRowException"/> by default (recommnded to prevent issues).</para>
        /// <para><strong>This setting is for advanced <em>FluentCommands</em> users and is <em>not</em> recommended for normal use.</strong></para>
        /// </summary>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder BruteForceKeyboardReferences()
        {
            In_BruteForceKeyboardReferences = true;
            return this;
        }

        internal bool In_DisableLoggingGlobally { get; private set; }
        /// <summary>
        /// Disables logging in <strong><em>all</em></strong> modules.
        /// <para>This setting cannot be overridden by modules when enabled.</para>
        /// </summary>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder DisableLoggingGlobally()
        {
            In_DisableLoggingGlobally = true;
            return this;
        }

        internal bool In_SwallowCriticalExceptions { get; private set; }
        /// <summary>
        /// Sets the <see cref="CommandService"/> to catch <strong><em>all</em></strong> recoverable exceptions regardless of severity.
        /// <para><strong>This setting is for advanced <em>FluentCommands</em> users and is not recommended for normal use.</strong></para>
        /// </summary>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder SwallowCriticalExceptions()
        {
            In_SwallowCriticalExceptions = true;
            return this;
        }

        internal bool In_EnableManualConfiguration { get; private set; }
        /// <summary>
        /// <strong>WARNING: This setting disables <em>ALL</em> <see cref="TelegramBotClient"/> integration from the <see cref="CommandService"/>.</strong>
        /// <para>If enabled, all command evaluation must be done manually via the <see cref="CommandService.Evaluate()"/> methods, and <see cref="TelegramBotClient"/> instances <strong><em>cannot</em></strong> be registered.</para>
        /// <para><strong>This setting is for advanced <em>FluentCommands</em> users, and is <strong>not</strong> recommended for normal use.</strong></para>
        /// </summary>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder EnableManualConfiguration()
        {
            In_EnableManualConfiguration = true;
            return this;
        }

        internal FluentLogLevel In_MaximumLogLevel { get; private set; } = FluentLogLevel.Fatal;
        /// <summary>
        /// Sets the maximum <see cref="FluentLogLevel"/> for the <see cref="CommandService"/>.
        /// <para>Any logging event higher than this setting will not be logged.</para>
        /// </summary>
        /// <param name="logLevel">The maximum <see cref="FluentLogLevel"/> that will be logged.</param>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder MaximumLogLevel(FluentLogLevel logLevel)
        {
            In_MaximumLogLevel = logLevel;
            return this;
        }

        internal MenuMode In_DefaultMenuMode { get; private set; }
        /// <summary>
        /// Sets the default <see cref="MenuMode"/> for <see cref="Menu"/> objects sent across all modules.
        /// <para>This setting can be overridden in your <see cref="CommandModule{TCommand}.OnConfiguring(ModuleConfigBuilder)"/> method.</para>
        /// </summary>
        /// <param name="menuMode">The default <see cref="MenuMode"/> for <see cref="Menu"/> objects sent across all modules.</param>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder DefaultMenuMode(MenuMode menuMode)
        {
            In_DefaultMenuMode = menuMode;
            return this;
        }

        internal bool In_UsingCustomDefaultErrorMsg { get; private set; }
        internal ISendableMenu? In_CustomDefaultErrorMsg { get; private set; }
        /// <summary>
        /// Sets the default error message for <strong><em>all</em></strong> commands across <strong><em>all</em></strong> modules.
        /// <para>This message can be overridden per-module in your <see cref="CommandModule{TCommand}.OnConfiguring(ModuleConfigBuilder)"/> and <see cref="CommandModule{TCommand}.OnBuilding(ModuleBuilder)"/> methods.</para>
        /// </summary>
        /// <param name="messageText">The error message string.</param>
        /// <param name="parseMode">The <see cref="ParseMode"/> used to send with the error message string.</param>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder DefaultErrorMessage(string messageText, ParseMode parseMode = ParseMode.Default)
        {
            In_CustomDefaultErrorMsg = Menu.Text(messageText).ParseMode(parseMode);
            In_UsingCustomDefaultErrorMsg = true;
            return this;
        }
        /// <summary>
        /// Sets the default error message for <strong><em>all</em></strong> commands across <strong><em>all</em></strong> modules.
        /// <para>This message can be overridden per-module in your <see cref="CommandModule{TCommand}.OnConfiguring(ModuleConfigBuilder)"/> and <see cref="CommandModule{TCommand}.OnBuilding(ModuleBuilder)"/> methods.</para>
        /// </summary>
        /// <param name="menu">The <see cref="ISendableMenu"/> to use as the default error message.</param>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder DefaultErrorMessage(ISendableMenu menu)
        {
            In_CustomDefaultErrorMsg = menu;
            In_UsingCustomDefaultErrorMsg = true;
            return this;
        }

        internal bool In_UsingCustomDefaultHelpMsg { get; private set; }
        internal ISendableMenu? In_CustomDefaultHelpMsg { get; private set; }
        /// <summary>
        /// Sets the help message for <strong><em>all</em></strong> commands across <strong><em>all</em></strong> modules.
        /// <para>The default behavior when <c>help</c> is called alone (with no arguments) is to list all available commands. <strong>If you do not wish to override this behavior, do not use this setting.</strong></para>
        /// <para>This message can be overridden per-module in your <see cref="CommandModule{TCommand}.OnConfiguring(ModuleConfigBuilder)"/> and <see cref="CommandModule{TCommand}.OnBuilding(ModuleBuilder)"/> methods.</para>
        /// </summary>
        /// <param name="messageText">The help message string.</param>
        /// <param name="parseMode">The <see cref="ParseMode"/> used to send with the help message string.</param>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder DefaultHelpMessage(string messageText, ParseMode parseMode = ParseMode.Default)
        {
            In_CustomDefaultHelpMsg = Menu.Text(messageText).ParseMode(parseMode).Done();
            In_UsingCustomDefaultHelpMsg = true;
            return this;
        }
        /// <summary>
        /// Sets the help message for <strong><em>all</em></strong> commands across <strong><em>all</em></strong> modules.
        /// <para>The default behavior when <c>help</c> is called alone (with no arguments) is to list all available commands. <strong>If you do not wish to override this behavior, do not use this setting.</strong></para>
        /// <para>This message can be overridden per-module in your <see cref="CommandModule{TCommand}.OnConfiguring(ModuleConfigBuilder)"/> and <see cref="CommandModule{TCommand}.OnBuilding(ModuleBuilder)"/> methods.</para>
        /// </summary>
        /// <param name="menu">The <see cref="ISendableMenu"/> to use as the default help message.</param>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder DefaultHelpMessage(ISendableMenu menu)
        {
            In_CustomDefaultHelpMsg = menu;
            In_UsingCustomDefaultHelpMsg = true;
            return this;
        }

        internal bool In_UsingBotClient { get; private set; }
        internal bool In_UsingCustomCache { get; private set; }
        internal bool In_UsingCustomLogger { get; private set; }
        /// <summary>
        /// Adds an <see cref="IFluentCache"/> to the <see cref="CommandService"/>.
        /// <para><typeparamref name="TCacheImplementation"/> is the class you wish to register that implements <see cref="IFluentCache"/>.</para>
        /// <para><strong>You may only add one <see cref="IFluentCache"/> to the <see cref="CommandService"/>. </strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <typeparam name="TCacheImplementation">The implementation of <see cref="IFluentCache"/>.</typeparam>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        /// <exception cref="InvalidConfigSettingsException">Thrown when attempting to add more than one <see cref="IFluentCache"/>.</exception>
        public CommandServiceConfigBuilder AddCache<TCacheImplementation>() where TCacheImplementation : class, IFluentCache
        { 
            if (In_UsingCustomCache) throw new InvalidConfigSettingsException("Attempted to add more than one cache to the CommandService. (You can only add one.)");
            CommandService.AddCache<TCacheImplementation>(typeof(CommandService));
            In_UsingCustomCache = true;
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IFluentCache"/> to the <see cref="CommandService"/>.
        /// <para><strong>You may only add one <see cref="IFluentCache"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <param name="implementationType">The class you wish to register that implements <see cref="IFluentCache"/>.</param>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        /// <exception cref="InvalidConfigSettingsException">Thrown when attempting to add more than one <see cref="IFluentCache"/>.</exception>
        public CommandServiceConfigBuilder AddCache(Type implementationType)
        { 
            if (In_UsingCustomCache) throw new InvalidConfigSettingsException("Attempted to add more than one cache to the CommandService. (You can only add one.)");
            CommandService.AddCache(implementationType, typeof(CommandService));
            In_UsingCustomCache = true; 
            return this;
        }
        /// <summary>
        /// Adds a <see cref="TelegramBotClient"/> to the <see cref="CommandService"/>.
        /// <para>The provided <paramref name="token"/> will generate a <see cref="TelegramBotClient"/> with default settings. <em>Please remember to never store your token in plain-sight!</em></para>
        /// <para><strong>You may only add one <see cref="TelegramBotClient"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <param name="token">The token used to create a <see cref="TelegramBotClient"/>.</param>
        /// <exception cref="InvalidConfigSettingsException">Thrown when attempting to add more than one <see cref="TelegramBotClient"/>.</exception>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder AddClient(string token) 
        {
            if (In_UsingBotClient) throw new InvalidConfigSettingsException("Attempted to add more than one TelegramBotClient to the CommandService. (You can only add one.)");
            CommandService.AddClient(token, typeof(CommandService));
            In_UsingBotClient = true;
            return this;
        }
        /// <summary>
        /// Adds a <see cref="TelegramBotClient"/> to the <see cref="CommandService"/>.
        /// <para><strong>You may only add one <see cref="TelegramBotClient"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <param name="client">The <see cref="TelegramBotClient"/> being registered.</param>
        /// <exception cref="InvalidConfigSettingsException">Thrown when attempting to add more than one <see cref="TelegramBotClient"/>.</exception>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder AddClient(TelegramBotClient client)
        {
            if (In_UsingBotClient) throw new InvalidConfigSettingsException("Attempted to add more than one TelegramBotClient to the CommandService. (You can only add one.)");
            CommandService.AddClient(client, typeof(CommandService));
            In_UsingBotClient = true;
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IFluentLogger"/> to the <see cref="CommandService"/>.
        /// <para><typeparamref name="TLoggerImplementation"/> is the class you wish to register that implements <see cref="IFluentLogger"/>.</para>
        /// <para><strong>You may only add one <see cref="IFluentLogger"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <typeparam name="TLoggerImplementation">The implementation of <see cref="IFluentLogger"/>.</typeparam>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder AddLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger 
        { 
            if (In_UsingCustomLogger) throw new InvalidConfigSettingsException("Attempted to add more than one logger to the CommandService. (You can only add one.)");
            CommandService.AddLogger<TLoggerImplementation>(typeof(CommandService));
            In_UsingCustomLogger = true;
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IFluentLogger"/> to the <see cref="CommandService"/>.
        /// <para>The <paramref name="implementationInstance"/> is an instance of <see cref="IFluentLogger"/> being registered.</para>
        /// <para><strong>You may only add one <see cref="IFluentLogger"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <param name="implementationInstance">The implementation of <see cref="IFluentLogger"/>.</param>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder AddLogger(IFluentLogger implementationInstance) 
        { 
            if (In_UsingCustomLogger) throw new InvalidConfigSettingsException("Attempted to add more than one logger to the CommandService. (You can only add one.)");
            CommandService.AddLogger(implementationInstance, typeof(CommandService));
            In_UsingCustomLogger = true;
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IFluentLogger"/> to the <see cref="CommandService"/>.
        /// <para>The <paramref name="implementationType"/> parameter is the class you wish to register that implements <see cref="IFluentLogger"/>.</para>
        /// <para><strong>You may only add one <see cref="IFluentLogger"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <param name="implementationType">The <see cref="Type"/> that implements <see cref="IFluentLogger"/>.</param>
        /// <returns>Returns this <see cref="CommandServiceConfigBuilder"/> to continue the building process.</returns>
        public CommandServiceConfigBuilder AddLogger(Type implementationType) 
        { 
            if (In_UsingCustomLogger) throw new InvalidConfigSettingsException("Attempted to add more than one logger to the CommandService. (You can only add one.)");
            CommandService.AddLogger(implementationType, typeof(CommandService));
            In_UsingCustomLogger = true;
            return this;
        }
        #endregion
    }
}
