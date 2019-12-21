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

namespace FluentCommands.Builders
{
    //: documentation...
    public class CommandServiceConfigBuilder
    {
        private readonly Lazy<IServiceCollection> _services = new Lazy<IServiceCollection>(() => new ServiceCollection());
        public bool Logging { get; set; }
        public bool UseDefaultRules { get; set; }
        public bool UseDefaultErrorMsg { get; set; }
        public bool UseInternalStateHandlerForReplyKeyboards { get; set; }
        public bool UseGlobalLogging { get; set; }
        public bool CaptureAllLoggingEvents { get; set; }
        public bool SwallowCriticalExceptions { get; set; }
        public FluentLogLevel MaximumLogLevel { get; set; } = FluentLogLevel.Fatal;
        public string DefaultPrefix { get; set; } = "/";
        public MenuMode DefaultMenuMode { get; set; } = MenuMode.NoAction;

        internal CommandServiceConfig BuildConfig() => new CommandServiceConfig(this);
        internal Lazy<IServiceCollection> GetServices() => _services;

        public void AddClient(string token) => _services.Value.AddClient(token);
        public void AddClient(ClientBuilder clientBuilder) => _services.Value.AddClient(clientBuilder);
        public void AddClient(TelegramBotClient client) => _services.Value.AddClient(client);
        public void AddLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger => _services.Value.AddLogger<TLoggerImplementation>();
        public void AddLogger(IFluentLogger implementationInstance) => _services.Value.AddLogger(implementationInstance);
        public void AddLogger(Type implementationType) => _services.Value.AddLogger(implementationType);
        public void AddDatabase<TDatabaseImplementation>() where TDatabaseImplementation : class, IFluentDatabase => _services.Value.AddDatabase<TDatabaseImplementation>();
        public void AddDatabase(Type implementationType) => _services.Value.AddDatabase(implementationType);
    }
}
