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
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommands
{
    public class ModuleConfigBuilder
    {
        internal Type ModuleType { get; }
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

        internal bool UsingBotClient { get; private set; }
        internal bool UsingCustomCacheOverride { get; private set; }
        internal bool UsingCustomLoggerOverride { get; private set; }

        internal ModuleConfigBuilder(Type m) => ModuleType = m;

        //: Documentation
        public void AddCache<TCacheImplementation>() where TCacheImplementation : class, IFluentCache { CommandService.AddCache<TCacheImplementation>(ModuleType); UsingCustomCacheOverride = true; }
        public void AddCache(Type implementationType) { CommandService.AddCache(implementationType, ModuleType); UsingCustomCacheOverride = true; }
        public void AddClient(string token) { CommandService.AddClient(token, ModuleType); UsingBotClient = true; }
        public void AddClient(ClientBuilder clientBuilder) { CommandService.AddClient(clientBuilder, ModuleType); UsingBotClient = true; }
        public void AddClient(TelegramBotClient client) { CommandService.AddClient(client, ModuleType); UsingBotClient = true; }
        public void AddLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger { CommandService.AddLogger<TLoggerImplementation>(ModuleType); UsingCustomLoggerOverride = true; }
        public void AddLogger(IFluentLogger implementationInstance) { CommandService.AddLogger(implementationInstance, ModuleType); UsingCustomLoggerOverride = true; }
        public void AddLogger(Type implementationType) { CommandService.AddLogger(implementationType, ModuleType); UsingCustomLoggerOverride = true; }

        internal ModuleConfig BuildConfig() => new ModuleConfig(this);
    }
}
