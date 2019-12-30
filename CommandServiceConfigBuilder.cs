using FluentCommands.Logging;
using System;
using FluentCommands.Menus;
using Telegram.Bot;
using FluentCommands.Cache;
using FluentCommands.Interfaces;
using Telegram.Bot.Types.Enums;
using FluentCommands.Interfaces.MenuBuilders;

namespace FluentCommands
{
    //: documentation...
    public class CommandServiceConfigBuilder : IFluentInterface
    {
        public CommandServiceConfigBuilder(Action<CommandServiceConfigBuilder> buildAction) { if (buildAction is { }) buildAction(this); }
        internal CommandServiceConfigBuilder() { }
        internal CommandServiceConfig BuildConfig() => new CommandServiceConfig(this);

        #region Properties and Exposed Setters
        internal (int AmountOfMessages, TimeSpan PerTimeSpan) In_DefaultRateLimitPerUser { get; private set; } //: make this set-able, and available for the module class
        public void DefaultRateLimitPerUser(int amountOfMessages, TimeSpan perTimeSpan = default) => In_DefaultRateLimitPerUser = (amountOfMessages, perTimeSpan);

        internal bool In_BruteForceKeyboardReferences { get; private set; } //: Advanced. Consider separating advanced switches from the rest of them (but maybe consider ALL of these to be advanced features)
        public void BruteForceKeyboardReferences() => In_BruteForceKeyboardReferences = true;

        internal bool In_DisableLoggingGlobally { get; private set; }
        public void DisableLoggingGlobally() => In_DisableLoggingGlobally = true;

        internal bool In_SwallowCriticalExceptions { get; private set; }
        public void SwallowCriticalExceptions() => In_SwallowCriticalExceptions = true;

        internal bool In_EnableManualConfiguration { get; private set; }
        public void EnableManualConfiguration() => In_EnableManualConfiguration = true;

        internal FluentLogLevel In_MaximumLogLevel { get; private set; } = FluentLogLevel.Fatal;
        public void MaximumLogLevel(FluentLogLevel logLevel) => In_MaximumLogLevel = logLevel;

        internal MenuMode In_DefaultMenuMode { get; private set; }
        public void DefaultMenuMode(MenuMode menuMode) => In_DefaultMenuMode = menuMode;

        internal bool In_UsingCustomDefaultErrorMsg { get; private set; }
        internal ISendableMenu? In_CustomDefaultErrorMsg { get; private set; }
        public void UseCustomDefaultErrorMsg(string messageText, ParseMode parseMode = ParseMode.Default)
        {
            In_CustomDefaultErrorMsg = Menu.Text(messageText).ParseMode(parseMode);
            In_UsingCustomDefaultErrorMsg = true;
        }
        public void UseCustomDefaultErrorMsg(ISendableMenu menu)
        {
            In_CustomDefaultErrorMsg = menu;
            In_UsingCustomDefaultErrorMsg = true;
        }

        //: inform what the default does, and that overriding it will override this function
        internal bool In_UsingCustomDefaultHelpMsg { get; private set; }
        internal ISendableMenu? In_CustomDefaultHelpMsg { get; private set; }
        public void UseCustomDefaultHelpMsg(string messageText, ParseMode parseMode = ParseMode.Default)
        {
            In_CustomDefaultHelpMsg = Menu.Text(messageText).ParseMode(parseMode).Done();
            In_UsingCustomDefaultHelpMsg = true;
        }
        public void UseCustomDefaultHelpMsg(ISendableMenu menu)
        {
            In_CustomDefaultHelpMsg = menu;
            In_UsingCustomDefaultHelpMsg = true;
        }

        internal bool In_UsingCustomCache { get; private set; }
        internal bool In_UsingCustomLogger { get; private set; }
        public void AddCache<TCacheImplementation>() where TCacheImplementation : class, IFluentCache { CommandService.AddCache<TCacheImplementation>(typeof(CommandService)); In_UsingCustomCache = true; }
        public void AddCache(Type implementationType) { CommandService.AddCache(implementationType, typeof(CommandService)); In_UsingCustomCache = true; }
        public void AddClient(string token) => CommandService.AddClient(token, typeof(CommandService));
        public void AddClient(TelegramBotClient client) => CommandService.AddClient(client, typeof(CommandService));
        public void AddLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger { CommandService.AddLogger<TLoggerImplementation>(typeof(CommandService)); In_UsingCustomLogger = true; }
        public void AddLogger(IFluentLogger implementationInstance) { CommandService.AddLogger(implementationInstance, typeof(CommandService)); In_UsingCustomLogger = true; }
        public void AddLogger(Type implementationType) { CommandService.AddLogger(implementationType, typeof(CommandService)); In_UsingCustomLogger = true; }
        #endregion
    }
}
