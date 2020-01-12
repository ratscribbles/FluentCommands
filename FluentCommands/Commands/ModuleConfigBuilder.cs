using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentCommands.Exceptions;
using FluentCommands.Logging;
using FluentCommands.Menus;
using FluentCommands.Utility;
using FluentCommands.Cache;
using FluentCommands.Interfaces.MenuBuilders;
using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using FluentCommands.Interfaces;
using System.Linq;

namespace FluentCommands.Commands
{
    /// <summary>
    /// This class constructs a <see cref="ModuleConfig"/>, which is used when processing the commands of this <see cref="CommandModule{TCommand}"/>.
    /// </summary>
    public class ModuleConfigBuilder : IFluentInterface
    {
        internal ModuleConfigBuilder(Type m) => ModuleType = m;
        internal Type ModuleType { get; }

        internal bool In_DeleteAllIncomingUserInputs { get; private set; }
        /// <summary>
        /// Sets the <see cref="TelegramBotClient"/> for this module to delete <strong>all</strong> user inputs, regardless of command calls.
        /// <para><strong>Warning: This setting will attempt to delete ALL user inputs based on the <see cref="ChatTypeRestriction(TelegramChatType)"/> setting for this module.</strong> It is recommended to set this to <see cref="TelegramChatType.Private"/> when using this feature, but not required.</para>
        /// </summary>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder DeleteAllIncomingUserInputs()
        { 
            In_DeleteAllIncomingUserInputs = true;
            return this;
        }

        internal bool In_DeleteCommandAfterCall { get; private set; }
        /// <summary>
        /// Sets the <see cref="TelegramBotClient"/> for this module to delete <strong>all</strong> inputs that appear to be a command call.
        /// <para>This setting will attempt to delete any message prefixed with "/", as well as the custom prefix for this module.</para>
        /// </summary>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder DeleteCommandAfterCall()
        {
            In_DeleteCommandAfterCall = true;
            return this;
        }

        internal bool In_DisableLogging { get; private set; }
        /// <summary>
        /// Disables all logging for this module.
        /// </summary>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder DisableLogging() 
        {
            In_DisableLogging = true;
            return this;
        }

        internal bool On_Building_DisableInternalCommandEvaluation { get; private set; }
        /// <summary>
        /// Sets the <see cref="TelegramBotClient"/> for this module to not automatically evaluate commands.
        /// <para>By using this setting, you must use the <see cref="CommandService.Evaluate()"/> methods in order for commands to be recognized.</para>
        /// <para><strong>This setting is intended for advanced users.</strong></para>
        /// </summary>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder DisableInternalCommandEvaluation()
        {
            On_Building_DisableInternalCommandEvaluation = true;
            return this;
        }

        internal FluentLogLevel In_MaximumLogLevelOverride { get; private set; }
        /// <summary>
        /// Overrides the maximum <see cref="FluentLogLevel"/> set in the <see cref="CommandServiceConfig"/>, and uses this setting for this module.
        /// </summary>
        /// <param name="logLevel">The maximum <see cref="FluentLogLevel"/> for this module.</param>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder MaximumLogLevelOverride(FluentLogLevel logLevel)
        {
            In_MaximumLogLevelOverride = logLevel;
            return this;
        }

        internal string In_Prefix { get; private set; } = "/";
        /// <summary>
        /// Sets the string the <see cref="TelegramBotClient"/> will use to recognize an input as a command call.
        /// <para>The default is "/".</para>
        /// </summary>
        /// <param name="prefix">The string the bot recognizes as the beginning of a command call.</param>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder Prefix(string prefix)
        {
            In_Prefix = prefix;
            return this;
        }

        internal ISendableMenu? In_DefaultErrorMessageOverride { get; private set; }
        /// <summary>
        /// Overrides the Default Error Message set in the <see cref="CommandServiceConfig"/>, using this one for this module.
        /// </summary>
        /// <param name="errorMessage">The error message string to send when an error occurs.</param>
        /// <param name="parseMode">The <see cref="ParseMode"/> to use when sending the error message string.</param>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder DefaultErrorMessageOverride(string errorMessage, ParseMode parseMode)
        {
            In_DefaultErrorMessageOverride = string.IsNullOrWhiteSpace(errorMessage)
                ? Menu.Text(errorMessage).ParseMode(parseMode)
                : CommandService.GlobalConfig.CustomDefaultErrorMsg;
            return this;
        }
        /// <summary>
        /// Overrides the Default Error Message set in the <see cref="CommandServiceConfig"/>, using this setting for this module.
        /// </summary>
        /// <param name="menu">The error message to send when an error occurs.</param>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder DefaultErrorMessageOverride(ISendableMenu menu)
        {
            In_DefaultErrorMessageOverride = menu;
            return this;
        }

        internal MenuMode In_MenuModeOverride { get; private set; } = MenuMode.NoAction;
        /// <summary>
        /// Overrides the <see cref="MenuMode"/> that was set in the <see cref="CommandServiceConfig"/>, using this setting for this module.
        /// <para>The <see cref="MenuMode"/> is used to determine how to send <see cref="Menu"/> objects through the <see cref="TelegramBotClient"/>.</para>
        /// </summary>
        /// <param name="menuMode">The <see cref="MenuMode"/> to use for this module.</param>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder MenuModeOverride(MenuMode menuMode)
        {
            In_MenuModeOverride = menuMode;
            return this;
        }

        internal (int AmountOfMessages, TimeSpan PerTimeSpan) In_RateLimitPerUser { get; private set; }
        /// <summary>
        /// Sets the rate limit for command inputs on a per-user basis.
        /// <para>Users will only be able to send a certain amount of commands within a specified <see cref="TimeSpan"/>. <strong>There is no limit by default.</strong></para>
        /// </summary>
        /// <param name="amountOfMessages">The amount of commands a user can send within the <see cref="TimeSpan"/> specified.</param>
        /// <param name="perTimeSpan">The <see cref="TimeSpan"/> that users can send a certain amount of commands over.</param>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder RateLimitPerUser(int amountOfMessages, TimeSpan perTimeSpan)
        {
            In_RateLimitPerUser = (amountOfMessages, perTimeSpan);
            return this;
        }

        internal TelegramChatType In_ChatType { get; private set; }
        /// <summary>
        /// The <see cref="ChatType"/> the <see cref="TelegramBotClient"/> will recognize and evaluate this module's commands in.
        /// <para>This is represented by the <see cref="TelegramChatType"/> enum. If you want to recognize more than one <see cref="ChatType"/>, <strong>use the Bitwise OR operator "|"</strong> between the types you want the client to use.</para>
        /// </summary>
        /// <param name="chatType">The <see cref="ChatType"/> that will be recognized by the <see cref="TelegramBotClient"/>.</param>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder ChatTypeRestriction(TelegramChatType chatType)
        {
            In_ChatType = chatType;
            return this;
        }

        internal bool In_UsingBotClient { get; private set; }
        internal bool In_UsingCustomCacheOverride { get; private set; }
        internal bool In_UsingCustomLoggerOverride { get; private set; }

        /// <summary>
        /// Adds an <see cref="IFluentCache"/> to this module, overriding the one set in the <see cref="CommandServiceConfig"/> (if it exists).
        /// <para><typeparamref name="TCacheImplementation"/> is the class you wish to register that implements <see cref="IFluentCache"/>.</para>
        /// <para><strong>You may only add one <see cref="IFluentCache"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <typeparam name="TCacheImplementation">The implementation of <see cref="IFluentCache"/>.</typeparam>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        /// <exception cref="InvalidConfigSettingsException">Thrown when attempting to add more than one <see cref="IFluentCache"/>.</exception>
        public ModuleConfigBuilder AddCache<TCacheImplementation>() where TCacheImplementation : class, IFluentCache 
        {
            if (In_UsingCustomCacheOverride) throw new InvalidConfigSettingsException("Attempted to add more than one cache for this module. (You can only add one Cache per CommandModule<TCommand>.)");
            CommandService.AddCache<TCacheImplementation>(ModuleType); 
            In_UsingCustomCacheOverride = true;
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IFluentCache"/> to this module, overriding the one set in the <see cref="CommandServiceConfig"/> (if it exists).
        /// <para>The <paramref name="implementationType"/> parameter is the class you wish to register that implements <see cref="IFluentCache"/>.</para>
        /// <para><strong>You may only add one <see cref="IFluentCache"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <param name="implementationType">The <see cref="Type"/> that implements <see cref="IFluentCache"/>.</param>
        /// <exception cref="InvalidConfigSettingsException">Thrown when attempting to add more than one <see cref="IFluentCache"/>.</exception>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder AddCache(Type implementationType)
        { 
            if (In_UsingCustomCacheOverride) throw new InvalidConfigSettingsException("Attempted to add more than one cache for this module. (You can only add one Cache per CommandModule<TCommand>.)");
            CommandService.AddCache(implementationType, ModuleType);
            In_UsingCustomCacheOverride = true; 
            return this;
        }
        /// <summary>
        /// Adds a <see cref="TelegramBotClient"/> to this module, overriding the one set in the <see cref="CommandServiceConfig"/> (if it exists).
        /// <para>The provided <paramref name="token"/> will generate a <see cref="TelegramBotClient"/> with default settings. <em>Please remember to never store your token in plain-sight!</em></para>
        /// <para><strong>You may only add one <see cref="TelegramBotClient"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <param name="token">The token used to create a <see cref="TelegramBotClient"/>.</param>
        /// <exception cref="InvalidConfigSettingsException">Thrown when attempting to add more than one <see cref="TelegramBotClient"/>.</exception>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder AddClient(string token)
        { 
            if (In_UsingBotClient) throw new InvalidConfigSettingsException("Attempted to add more than one TelegramBotClient for this module. (You can only add one TelegramBotClient per CommandModule<TCommand>.)");
            CommandService.AddClient(token, ModuleType);
            In_UsingBotClient = true; 
            return this;
        }
        /// <summary>
        /// Adds a <see cref="TelegramBotClient"/> to this module, overriding the one set in the <see cref="CommandServiceConfig"/> (if it exists).
        /// <para><strong>You may only add one <see cref="TelegramBotClient"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <param name="client">The <see cref="TelegramBotClient"/> being registered.</param>
        /// <exception cref="InvalidConfigSettingsException">Thrown when attempting to add more than one <see cref="TelegramBotClient"/>.</exception>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder AddClient(TelegramBotClient client) 
        { 
            if (In_UsingBotClient) throw new InvalidConfigSettingsException("Attempted to add more than one TelegramBotClient for this module. (You can only add one TelegramBotClient per CommandModule<TCommand>.)");
            CommandService.AddClient(client, ModuleType);
            In_UsingBotClient = true; 
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IFluentLogger"/> to this module, overriding the one set in the <see cref="CommandServiceConfig"/> (if it exists).
        /// <para><typeparamref name="TLoggerImplementation"/> is the class you wish to register that implements <see cref="IFluentLogger"/>.</para>
        /// <para><strong>You may only add one <see cref="IFluentLogger"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <typeparam name="TLoggerImplementation">The implementation of <see cref="IFluentLogger"/>.</typeparam>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder AddLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger 
        { 
            if (In_UsingCustomLoggerOverride) throw new InvalidConfigSettingsException("Attempted to add more than one logger for this module. (You can only add one logger per CommandModule<TCommand>.)");
            CommandService.AddLogger<TLoggerImplementation>(ModuleType);
            In_UsingCustomLoggerOverride = true; 
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IFluentLogger"/> to this module, overriding the one set in the <see cref="CommandServiceConfig"/> (if it exists).
        /// <para>The <paramref name="implementationInstance"/> is an instance of <see cref="IFluentLogger"/> being registered.</para>
        /// <para><strong>You may only add one <see cref="IFluentLogger"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <param name="implementationInstance">The implementation of <see cref="IFluentLogger"/>.</param>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder AddLogger(IFluentLogger implementationInstance) 
        { 
            if (In_UsingCustomLoggerOverride) throw new InvalidConfigSettingsException("Attempted to add more than one logger for this module. (You can only add one logger per CommandModule<TCommand>.)");
            CommandService.AddLogger(implementationInstance, ModuleType);
            In_UsingCustomLoggerOverride = true; 
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IFluentLogger"/> to this module, overriding the one set in the <see cref="CommandServiceConfig"/> (if it exists).
        /// <para>The <paramref name="implementationType"/> parameter is the class you wish to register that implements <see cref="IFluentLogger"/>.</para>
        /// <para><strong>You may only add one <see cref="IFluentLogger"/> per module.</strong> Attempting to add more than one will result in an <see cref="InvalidConfigSettingsException"/>.</para>
        /// </summary>
        /// <param name="implementationType">The <see cref="Type"/> that implements <see cref="IFluentLogger"/>.</param>
        /// <returns>Returns this <see cref="ModuleConfigBuilder"/> to continue the building process.</returns>
        public ModuleConfigBuilder AddLogger(Type implementationType) 
        {
            if (In_UsingCustomLoggerOverride) throw new InvalidConfigSettingsException("Attempted to add more than one logger for this module. (You can only add one logger per CommandModule<TCommand>.)");
            CommandService.AddLogger(implementationType, ModuleType);
            In_UsingCustomLoggerOverride = true; 
            return this;
        }

        internal ModuleConfig BuildConfig() => new ModuleConfig(this);
    }
}
