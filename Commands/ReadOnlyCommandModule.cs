using FluentCommands.Cache;
using FluentCommands.Commands;
using FluentCommands.Exceptions;
using FluentCommands.Interfaces;
using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace FluentCommands.Commands
{
    internal class ReadOnlyCommandModule : IReadOnlyModule
    {
        /// <summary>Stored logger for this <see cref="CommandModule{TCommand}"/>.</summary>
        private readonly ModuleConfig _config;
        private readonly Type _typeStorage;
        private readonly bool _useModuleLogger;
        private readonly bool _useModuleCache;
        private readonly bool _useClient;

        ModuleConfig IReadOnlyModule.Config => _config;
        TelegramBotClient? IReadOnlyModule.Client
        {
            get
            {
                if (_useClient) return CommandService.ServicesCollection[_typeStorage].Client!; // Not Null if true.
                else return CommandService.InternalClient;
            }
        }
        IFluentCache IReadOnlyModule.Cache
        {
            get
            {
                if (_useModuleCache) return CommandService.ServicesCollection[_typeStorage].Cache!; // Not Null if true.
                else return CommandService.Cache;
            }
        }
        IFluentLogger IReadOnlyModule.Logger
        {
            get
            {
                if (_config.DisableLogging) return CommandService.EmptyLogger;
                
                if (_useModuleLogger) return CommandService.ServicesCollection[_typeStorage].Logger!; // Not Null if true.
                else return CommandService.Logger;
            }
        }
        Type IReadOnlyModule.TypeStorage => _typeStorage;
        bool IReadOnlyModule.UseModuleLogger => _useModuleLogger;
        bool IReadOnlyModule.UseModuleCache => _useModuleCache;
        bool IReadOnlyModule.UseClient => _useClient;

        internal ReadOnlyCommandModule(ModuleBuilder m, bool hasClient, bool hasCache, bool hasLogger)
        {
            _config = m.BuildConfig();

            _useClient = hasClient;
            _useModuleCache = hasCache;
            _useModuleLogger = hasLogger;

            _typeStorage = m.TypeStorage;
        }
    }
}
