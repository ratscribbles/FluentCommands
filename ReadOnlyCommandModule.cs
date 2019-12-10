using FluentCommands.Builders;
using FluentCommands.Cache;
using FluentCommands.Exceptions;
using FluentCommands.Interfaces;
using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    internal class ReadOnlyCommandModule : IReadOnlyModule
    {
        /// <summary>Stored logger for this <see cref="CommandModule{TModule}"/>.</summary>
        private readonly ModuleConfig _config;
        private readonly Type _typeStorage;
        private readonly IFluentLogger _logger;
        private readonly IFluentDatabase _database;
        private readonly bool _useModuleLogger;
        private readonly bool _useModuleDb;

        ModuleConfig IReadOnlyModule.Config => _config;
        IFluentLogger IReadOnlyModule.Logger
        {
            get
            {
                if (_useModuleLogger) return _logger;
                else return CommandService.Logger;
            }
        }
        IFluentDatabase IReadOnlyModule.Database
        {
            get
            {
                return _database;
            }
        }

        Type IReadOnlyModule.TypeStorage => _typeStorage;

        internal ReadOnlyCommandModule(ModuleBuilder m)
        {
            _config = m.Config;
            //_database = m.DatabaseOverride;
            //_logger = m.LoggerOverride;
            _typeStorage = m.TypeStorage;

            if (_config.LogModuleActivities)
            {
            }
        }
    }
}
