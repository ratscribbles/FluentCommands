using FluentCommands.Builders;
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
        private readonly Lazy<ModuleLogger> _logger;
        private readonly Type _typeStorage;

        ModuleConfig IReadOnlyModule.Config => _config;
        IFluentLogger IReadOnlyModule.Logger
        {
            get
            {
                if (_config.LogModuleActivities) return _logger.Value;
                else
                {
                    if (CommandService.GlobalConfig.CaptureAllLoggingEvents) return _logger.Value;
                    else return CommandService.EmptyLogger;
                }
            }
        }
        Type IReadOnlyModule.TypeStorage => _typeStorage;

        internal ReadOnlyCommandModule(ModuleBuilder m)
        {
            _config = m.Config;
            _logger = new Lazy<ModuleLogger>(() => new ModuleLogger(m.TypeStorage, m.Config));
            _typeStorage = m.TypeStorage;

            if (_config.LogModuleActivities)
            {
                if (_config.UseLoggingEventHandler is { }) _logger.Value.LoggingEvent += _config.UseLoggingEventHandler;
                else throw new InvalidConfigSettingsException($"Module {_typeStorage.FullName} has logging enabled, but it has no event handler set. Please double check your code and make sure to add an event handler in your OnConfiguring method for this module through the UseLoggingEventHandler property.");
            }
        }
    }
}
