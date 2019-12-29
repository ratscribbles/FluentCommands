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
        /// <summary>Stored logger for this <see cref="CommandModule{TModule}"/>.</summary>
        private readonly ModuleConfig _config;
        private readonly Type _typeStorage;
        private readonly IFluentLogger? _logger;
        private readonly IFluentCache? _cache;
        private readonly TelegramBotClient? _client;
        private readonly bool _useModuleLogger;
        private readonly bool _useModuleCache;
        private readonly bool _useClient;

        ModuleConfig IReadOnlyModule.Config => _config;
        IFluentLogger IReadOnlyModule.Logger
        {
            get
            {
                if (_useModuleLogger) return _logger!; // Not Null if true.
                else return CommandService.Logger;
            }
        }
        IFluentCache IReadOnlyModule.Database
        {
            get
            {
                if (_useModuleCache) return _cache!; // Not Null if true.
                else return CommandService.Cache;
            }
        }
        TelegramBotClient? IReadOnlyModule.Client
        {
            get
            {
                if (_useClient) return _client!; // Not Null if true.
                else return CommandService.InternalClient;
            }
        }
        Type IReadOnlyModule.TypeStorage => _typeStorage;
        bool IReadOnlyModule.UseModuleLogger => _useModuleLogger;
        bool IReadOnlyModule.UseModuleCache => _useModuleCache;
        bool IReadOnlyModule.UseClient => _useClient;

        internal ReadOnlyCommandModule(ModuleBuilder m, TelegramBotClient? client = null, IFluentCache? cache = null, IFluentLogger? logger = null)
        {
            _config = m.BuildConfig();
            _client = client;
            _cache = cache;
            _logger = logger;

            _useModuleLogger = _logger is { };
            _useModuleCache = _cache is { };
            _useClient = _client is { };

            _typeStorage = m.TypeStorage;

            if (_config.DisableLogging)
            {
                //: might remove this
            }
        }
    }
}
