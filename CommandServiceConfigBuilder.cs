using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using FluentCommands.Menus;
using Telegram.Bot;
using FluentCommands.Exceptions;
using FluentCommands.Extensions;
using FluentCommands.Cache;
using FluentCommands.Interfaces;

namespace FluentCommands.Builders
{
    //: documentation...
    public class CommandServiceConfigBuilder : IFluentInterface
    {
        public bool BruteForceKeyboardReferences { get; set; } //: Advanced. Consider separating advanced switches from the rest of them (but maybe consider ALL of these to be advanced features)
        public bool DisableLogging { get; set; }
        public bool DisableDefaultRules { get; set; }
        public bool UseCustomDefaultErrorMsg { get; set; }
        public bool UseCustomDefaultHelpMsg { get; set; }
        public bool CaptureAllLoggingEvents { get; set; }
        public bool SwallowCriticalExceptions { get; set; }
        public bool DisableInternalEventHandlers { get; set; }
        public bool EnableManualSetup { get; }
        public FluentLogLevel MaximumLogLevel { get; set; } = FluentLogLevel.Fatal;
        public string DefaultPrefix { get; set; } = "/";
        public MenuMode DefaultMenuMode { get; set; } = MenuMode.NoAction;

        internal CommandServiceConfig BuildConfig() => new CommandServiceConfig(this);

        public void AddCache<TDatabaseImplementation>() where TDatabaseImplementation : class, IFluentCache => CommandService.AddCache<TDatabaseImplementation>(typeof(CommandService));
        public void AddCache(Type implementationType) => CommandService.AddCache(implementationType, typeof(CommandService));
        public void AddClient(string token) => CommandService.AddClient(token, typeof(CommandService));
        public void AddClient(ClientBuilder clientBuilder) => CommandService.AddClient(clientBuilder, typeof(CommandService));
        public void AddClient(TelegramBotClient client) => CommandService.AddClient(client, typeof(CommandService));
        public void AddLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger => CommandService.AddLogger<TLoggerImplementation>(typeof(CommandService));
        public void AddLogger(IFluentLogger implementationInstance) => CommandService.AddLogger(implementationInstance, typeof(CommandService));
        public void AddLogger(Type implementationType) => CommandService.AddLogger(implementationType, typeof(CommandService));
    }
}
