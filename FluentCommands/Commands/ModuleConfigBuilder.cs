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

namespace FluentCommands.Commands
{
    //: doc...
    public class ModuleConfigBuilder
    {
        //: documentation...
        internal ModuleConfigBuilder(Type m) => ModuleType = m;
        internal Type ModuleType { get; }

        //: add a warning onto this setting.
        internal bool In_DeleteAllIncomingUserInputs { get; private set; }
        public void DeleteAllIncomingUserInputs() => In_DeleteAllIncomingUserInputs = true;

        internal bool In_DeleteCommandAfterCall { get; private set; }
        public void DeleteCommandAfterCall() => In_DeleteCommandAfterCall = true;

        internal bool In_DisableLogging { get; private set; }
        public void DisableLogging() => In_DisableLogging = true;

        internal bool On_Building_DisableInternalCommandEvaluation { get; private set; }
        public void DisableInternalCommandEvaluation() => On_Building_DisableInternalCommandEvaluation = true;

        internal FluentLogLevel In_MaximumLogLevelOverride { get; private set; }
        public void MaximumLogLevelOverride(FluentLogLevel logLevel) => In_MaximumLogLevelOverride = logLevel;

        internal string In_Prefix { get; private set; } = "/";
        public void Prefix(string prefix) => In_Prefix = prefix;

        internal ISendableMenu? In_DefaultErrorMessageOverride { get; private set; }
        public void DefaultErrorMessageOverride(string errorMessage, ParseMode parseMode) => In_DefaultErrorMessageOverride = Menu.Text(errorMessage).ParseMode(parseMode);
        public void DefaultErrorMessageOverride(ISendableMenu menu) => In_DefaultErrorMessageOverride = menu;

        internal MenuMode In_MenuModeOverride { get; private set; } = MenuMode.NoAction;
        public void MenuModeOverride(MenuMode menuMode) => In_MenuModeOverride = menuMode;

        internal (int AmountOfMessages, TimeSpan PerTimeSpan) In_RateLimitPerUser { get; private set; }
        public void RateLimitPerUser(int amountOfMessages, TimeSpan perTimeSpan = default) => In_RateLimitPerUser = (amountOfMessages, perTimeSpan);


        internal bool In_UsingBotClient { get; private set; }
        internal bool In_UsingCustomCacheOverride { get; private set; }
        internal bool In_UsingCustomLoggerOverride { get; private set; }

        //: Documentation
        public void AddCache<TCacheImplementation>() where TCacheImplementation : class, IFluentCache { CommandService.AddCache<TCacheImplementation>(ModuleType); In_UsingCustomCacheOverride = true; }
        public void AddCache(Type implementationType) { CommandService.AddCache(implementationType, ModuleType); In_UsingCustomCacheOverride = true; }
        public void AddClient(string token) { CommandService.AddClient(token, ModuleType); In_UsingBotClient = true; }
        public void AddClient(TelegramBotClient client) { CommandService.AddClient(client, ModuleType); In_UsingBotClient = true; }
        public void AddLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger { CommandService.AddLogger<TLoggerImplementation>(ModuleType); In_UsingCustomLoggerOverride = true; }
        public void AddLogger(IFluentLogger implementationInstance) { CommandService.AddLogger(implementationInstance, ModuleType); In_UsingCustomLoggerOverride = true; }
        public void AddLogger(Type implementationType) { CommandService.AddLogger(implementationType, ModuleType); In_UsingCustomLoggerOverride = true; }

        internal ModuleConfig BuildConfig() => new ModuleConfig(this);
    }
}
