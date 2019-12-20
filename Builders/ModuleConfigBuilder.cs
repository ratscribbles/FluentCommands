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
using FluentCommands.Builders;

namespace FluentCommands
{
    public class ModuleConfigBuilder
    {
        public bool UseInternalKeyboardStateHandler { get; set; } = false;
        public bool UseDefaultErrorMessage { get; set; } = false;
        public bool BruteForceKeyboardReferences { get; set; } = false;
        public bool DeleteCommandAfterCall { get; set; } = false;
        public bool LogModuleActivities { get; set; } = false;
        public FluentLogLevel MaximumLogLevelOverride { get; set; } = FluentLogLevel.Fatal;
        public string Prefix { get; set; } = "/";
        public IMenu? DefaultErrorMessageOverride { get; set; } = null;
        public MenuMode MenuModeOverride { get; set; } = MenuMode.NoAction;
        public int PerUserRateLimitOverride { get; private set; }

        internal ClientBuilder? BotClient { get; set; }
        internal bool UsingBotClient { get; private set; }
        internal IFluentDatabase? CustomDatabase { get; private set; }
        internal bool UsingCustomDatabaseOverride { get; private set; }
        internal IFluentLogger? CustomLogger { get; private set; }
        internal bool UsingCustomLoggerOverride { get; private set; }

        //: Documentation
        public void AddClient(string token) { BotClient = token; UsingBotClient = true; }
        public void AddClient(ClientBuilder client) { BotClient = client; UsingBotClient = true; }
        public void AddClient(TelegramBotClient client) { BotClient = client; UsingBotClient = true; }
        public void AddDatabase(IFluentDatabase db) { CustomDatabase = db; UsingCustomDatabaseOverride = true; }
        public void AddLogger(IFluentLogger l) { CustomLogger = l; UsingCustomLoggerOverride = true; }

        internal ModuleConfig BuildConfig() => new ModuleConfig(this);
        internal TelegramBotClient? BuildClient()
        {
            var client = BotClient?.Build();
            if (client is null) return null;
            else { UsingBotClient = true; return client; }
        }
    }
}
