using FluentCommands.Cache;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Logging;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    internal class CommandServiceConfig
    {
        internal bool DisableLogging { get; }
        internal bool UseCustomDefaultHelpMsg { get; }
        internal bool UseCustomDefaultErrorMsg { get; }
        internal bool UsingCustomDatabase { get; private set; }
        internal bool UsingCustomLogger { get; private set; }
        internal bool EnableManualConfiguration { get; }
        internal FluentLogLevel MaximumLogLevel { get; }
        internal bool SwallowCriticalExceptions { get; }
        internal int PerUserRateLimit { get; } //: make this set-able, and available for the module class
        internal string DefaultPrefix { get; }
        internal MenuMode DefaultMenuMode { get; }
        public IMenu DefaultErrorMessage { get; set; } = Menu.Text("OH NO!!!!!!!!!!"); //: Make this more professional.

        internal CommandServiceConfig(Builders.CommandServiceConfigBuilder c)
        {
            DefaultMenuMode = c.DefaultMenuMode;
            DefaultPrefix = c.DefaultPrefix;
            DisableLogging = c.DisableLogging;
            MaximumLogLevel = c.MaximumLogLevel;
            SwallowCriticalExceptions = c.SwallowCriticalExceptions;
            UseCustomDefaultErrorMsg = c.UseCustomDefaultErrorMsg;
            UseCustomDefaultHelpMsg = c.UseCustomDefaultHelpMsg;
        }

        internal void UseCustomDatabase() => UsingCustomDatabase = true;
        internal void UseCustomLogger() => UsingCustomLogger = true;
    }
}
