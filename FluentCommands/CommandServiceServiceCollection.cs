using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Extensions;
using FluentCommands.Logging;
using FluentCommands.Cache;
using Telegram.Bot;

namespace FluentCommands
{
    internal sealed class CommandServiceServiceCollection
    {
        private readonly IServiceCollection _services = new ServiceCollection();
        private readonly Dictionary<Type, int> _serviceCacheHashcodes = new Dictionary<Type, int>();
        private readonly Dictionary<Type, int> _serviceClientHashcodes = new Dictionary<Type, int>();
        private readonly Dictionary<Type, int> _serviceLoggerHashcodes = new Dictionary<Type, int>();

        internal void AddClient(string token, Type moduleType)
        {
            if (moduleType is null) moduleType = typeof(CommandService);
            if (_serviceClientHashcodes.ContainsKey(moduleType)) return;
            else
            {
                var value = _serviceClientHashcodes.Count;
                _serviceClientHashcodes.Add(moduleType, value);
                _services.AddClient(token);
            }
        }
        internal void AddClient(TelegramBotClient client, Type moduleType)
        {
            if (moduleType is null) moduleType = typeof(CommandService);
            if (_serviceClientHashcodes.ContainsKey(moduleType)) return;
            else
            {
                var indexToIncrement = _serviceClientHashcodes.Count;
                _serviceClientHashcodes.Add(moduleType, indexToIncrement);
                _services.AddClient(client);
            }
        }
        internal void AddLogger<TLoggerImplementation>(Type moduleType) where TLoggerImplementation : class, IFluentLogger
        {
            if (moduleType is null) moduleType = typeof(CommandService);
            if (_serviceLoggerHashcodes.ContainsKey(moduleType)) return;
            else
            {
                var indexToIncrement = _serviceLoggerHashcodes.Count;
                _serviceLoggerHashcodes.Add(moduleType, indexToIncrement);
                _services.AddLogger<TLoggerImplementation>();
            }
        }
        internal void AddLogger(IFluentLogger implementationInstance, Type moduleType)
        {
            if (moduleType is null) moduleType = typeof(CommandService);
            if (_serviceLoggerHashcodes.ContainsKey(moduleType)) return;
            else
            {
                var indexToIncrement = _serviceLoggerHashcodes.Count;
                _serviceLoggerHashcodes.Add(moduleType, indexToIncrement);
                _services.AddLogger(implementationInstance);
            }
        }
        internal void AddLogger(Type implementationType, Type moduleType)
        {
            if (moduleType is null) moduleType = typeof(CommandService);
            if (_serviceLoggerHashcodes.ContainsKey(moduleType)) return;
            else
            {
                var indexToIncrement = _serviceLoggerHashcodes.Count;
                _serviceLoggerHashcodes.Add(moduleType, indexToIncrement);
                _services.AddLogger(implementationType);
            }
        }
        internal void AddCache<TCacheImplementation>(Type moduleType) where TCacheImplementation : class, IFluentCache
        {
            if (moduleType is null) moduleType = typeof(CommandService);
            if (_serviceCacheHashcodes.ContainsKey(moduleType)) return;
            else
            {
                var indexToIncrement = _serviceCacheHashcodes.Count;
                _serviceCacheHashcodes.Add(moduleType, indexToIncrement);
                _services.AddCache<TCacheImplementation>();
            }
        }
        internal void AddCache(Type implementationType, Type moduleType)
        {
            if (moduleType is null) moduleType = typeof(CommandService);
            if (_serviceCacheHashcodes.ContainsKey(moduleType)) return;
            else
            {
                var indexToIncrement = _serviceCacheHashcodes.Count;
                _serviceCacheHashcodes.Add(moduleType, indexToIncrement);
                _services.AddCache(implementationType);
            }
        }

        internal IServiceCollection GetServices() => _services;
    }
}
