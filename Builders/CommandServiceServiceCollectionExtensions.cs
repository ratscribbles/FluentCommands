using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Cache;
using FluentCommands.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommands.Builders
{
    public static class CommandServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddLogger<TLoggerImplementation>(this IServiceCollection c) where TLoggerImplementation : class, IFluentLogger
        {
            c.AddSingleton<IFluentLogger, TLoggerImplementation>();
            return c;
        }

        public static IServiceCollection AddLogger(this IServiceCollection c, IFluentLogger implementationInstance)
        {
            c.AddSingleton(implementationInstance);
            return c;
        }
        public static IServiceCollection AddLogger(this IServiceCollection c, Type implementationType)
        {
            c.AddSingleton(typeof(IFluentLogger), implementationType);
            return c;
        }

        public static IServiceCollection AddDatabase<TDatabaseImplementation>(this IServiceCollection c) where TDatabaseImplementation : class, IFluentDatabase
        {
            c.AddTransient<IFluentDatabase, TDatabaseImplementation>();
            return c;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection c, Type implementationType)
        {
            c.AddTransient(typeof(IFluentDatabase), implementationType);
            return c;
        }
    }
}
