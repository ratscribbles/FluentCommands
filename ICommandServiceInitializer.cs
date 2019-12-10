using FluentCommands.Builders;
using FluentCommands.Cache;
using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    public interface ICommandServiceInitializer
    {
        ICommandServiceInitializer AddLogger<TLoggerImplementation>() where TLoggerImplementation : class, IFluentLogger;
        ICommandServiceInitializer AddLogger(IFluentLogger implementationInstance);
        ICommandServiceInitializer AddLogger(Type implementationType);
        ICommandServiceInitializer AddDatabase<TDatabaseImplementation>() where TDatabaseImplementation : class, IFluentDatabase;
        ICommandServiceInitializer AddDatabase(Type implementationType);
        void Start();
        void Start(CommandServiceConfigBuilder cfg);
        void Start(Action<CommandServiceConfigBuilder> buildAction);
    }
}
