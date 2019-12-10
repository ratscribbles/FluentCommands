using FluentCommands.Cache;
using FluentCommands.Logging;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    public enum DbProviderPreset { None, LiteDb, EFCore }
    internal class CommandServiceConfig
    {
        internal bool EnableLogging { get; }
        internal bool UseDefaultHelpMsg { get; }
        internal bool UseDefaultErrorMsg { get; }
        internal bool UsingCustomDatabase { get; private set; }
        internal bool UsingCustomLogger { get; private set; }
        internal FluentLogLevel MaximumLogLevel { get; }
        internal bool DontSwallowExceptions { get; }
        internal int PerUserRateLimit { get; } //: make this set-able, and available for the module class
        internal string DefaultPrefix { get; }
        internal MenuMode DefaultMenuMode { get; }
        public Menu DefaultErrorMessage { get; set; } = MenuItem.As().Text().TextSource("ERROR OCCURRED.").Done();

        internal CommandServiceConfig(Builders.CommandServiceConfigBuilder c)
        {
            DefaultMenuMode = c.DefaultMenuMode;
            DefaultPrefix = c.DefaultPrefix;
            EnableLogging = c.Logging;
            MaximumLogLevel = c.MaximumLogLevel;
            DontSwallowExceptions = c.SwallowExceptions;
            UseDefaultErrorMsg = c.UseDefaultErrorMsg;
            UseDefaultHelpMsg = c.UseDefaultRules;
        }

        internal void UseCustomDatabase() => UsingCustomDatabase = true;
        internal void UseCustomLogger() => UsingCustomLogger = true;
    }
}
