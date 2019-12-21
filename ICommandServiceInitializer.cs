using FluentCommands.Builders;
using FluentCommands.Cache;
using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace FluentCommands
{
    public interface ICommandServiceInitializer
    {
        ICommandServiceInitializer WithClient(string token);
        ICommandServiceInitializer WithClient(ClientBuilder clientBuilder);
        ICommandServiceInitializer WithClient(TelegramBotClient client);
        ICommandServiceInitializer WithLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger;
        ICommandServiceInitializer WithLogger(IFluentLogger implementationInstance);
        ICommandServiceInitializer WithLogger(Type implementationType);
        ICommandServiceInitializer WithDatabase<TDatabaseImplementation>() where TDatabaseImplementation : class, IFluentDatabase;
        ICommandServiceInitializer WithDatabase(Type implementationType);
    }
}
