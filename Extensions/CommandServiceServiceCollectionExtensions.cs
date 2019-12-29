using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Cache;
using FluentCommands.Logging;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace FluentCommands.Extensions
{
    internal static class CommandServiceServiceCollectionExtensions
    {
        internal static IServiceCollection AddClient(this IServiceCollection c, string token)
        {
            c.AddSingleton(new TelegramBotClient(token));
            return c;
        }
        internal static IServiceCollection AddClient(this IServiceCollection c, TelegramBotClient client)
        {
            c.AddSingleton(client);
            return c;
        }
        internal static IServiceCollection AddCache<TDatabaseImplementation>(this IServiceCollection c) where TDatabaseImplementation : class, IFluentCache
        {
            c.AddTransient<IFluentCache, TDatabaseImplementation>();
            return c;
        }

        internal static IServiceCollection AddCache(this IServiceCollection c, Type implementationType)
        {
            c.AddTransient(typeof(IFluentCache), implementationType);
            return c;
        }
        internal static IServiceCollection AddLogger<TLoggerImplementation>(this IServiceCollection c) where TLoggerImplementation : class, IFluentLogger
        {
            c.AddSingleton<IFluentLogger, TLoggerImplementation>();
            return c;
        }

        internal static IServiceCollection AddLogger(this IServiceCollection c, IFluentLogger implementationInstance)
        {
            c.AddSingleton(implementationInstance);
            return c;
        }
        internal static IServiceCollection AddLogger(this IServiceCollection c, Type implementationType)
        {
            c.AddSingleton(typeof(IFluentLogger), implementationType);
            return c;
        }
    }
}
