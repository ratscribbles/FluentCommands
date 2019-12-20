using FluentCommands.Builders;
using FluentCommands.Cache;
using FluentCommands.Exceptions;
using FluentCommands.Interfaces;
using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace FluentCommands
{
    internal class ReadOnlyCommandModule : IReadOnlyModule
    {
        /// <summary>Stored logger for this <see cref="CommandModule{TModule}"/>.</summary>
        private readonly ModuleConfig _config;
        private readonly Type _typeStorage;
        private readonly IFluentLogger? _logger;
        private readonly IFluentDatabase? _database;
        private readonly TelegramBotClient? _client;
        private readonly bool _useModuleLogger;
        private readonly bool _useModuleDb;
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
        IFluentDatabase IReadOnlyModule.Database
        {
            get
            {
                if (_useModuleDb) return _database!; // Not Null if true.
                else return CommandService.Cache;
            }
        }
        TelegramBotClient? IReadOnlyModule.Client
        {
            get
            {
                if (_useClient) return _client!;
                else return CommandService.InternalClient;
            }
        }
        Type IReadOnlyModule.TypeStorage => _typeStorage;
        bool IReadOnlyModule.UseModuleLogger => _useModuleLogger;
        bool IReadOnlyModule.UseModuleDb => _useModuleDb;
        bool IReadOnlyModule.UseClient => _useClient;

        internal ReadOnlyCommandModule(ModuleBuilder m)
        {
            _config = m.BuildConfig();
            _client = m.BuildClient();
            _database = m.ConfigBuilder.CustomDatabase;
            _logger = m.ConfigBuilder.CustomLogger;

            _useModuleLogger = _logger is { };
            _useModuleDb = _database is { };
            _useClient = _client is { };

            _typeStorage = m.TypeStorage;

            if (_config.LogModuleActivities)
            {
                //: might remove this
            }
        }
    }
}
