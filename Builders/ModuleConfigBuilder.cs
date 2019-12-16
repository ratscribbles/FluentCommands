using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentCommands.Exceptions;
using FluentCommands.Logging;
using FluentCommands.Menus;
using FluentCommands.Helper;
using FluentCommands.Cache;
using FluentCommands.Interfaces.MenuBuilders;

namespace FluentCommands
{
    public class ModuleConfigBuilder
    {
        public bool UseInternalKeyboardStateHandler { get; set; } = false;
        public bool UseDefaultErrorMessage { get; set; } = false;
        public bool BruteForceKeyboardReferences { get; set; } = false;
        public bool DeleteCommandAfterCall { get; set; } = false;
        public bool LogModuleActivities { get; set; } = false;
        public FluentLogLevel MaximumLogLevelOverride { get; set; } = FluentLogLevel.Fatal;
        public string Prefix { get; set; } = "/";
        public IMenu? DefaultErrorMessageOverride { get; set; } = null;
        public MenuMode MenuModeOverride { get; set; } = MenuMode.NoAction;
        internal int PerUserRateLimitOverride { get; private set; }
        
        internal IFluentDatabase? CustomDatabase { get; private set; }
        internal bool UsingCustomDatabaseOverride { get; private set; }
        internal IFluentLogger? CustomLogger { get; private set; }
        internal bool UsingCustomLoggerOverride { get; private set; }

        public void AddDatabase(IFluentDatabase db) { CustomDatabase = db; UsingCustomDatabaseOverride = true; }
        public void AddLogger(IFluentLogger l) { CustomLogger = l; UsingCustomLoggerOverride = true; }

        internal ModuleConfig Build() => new ModuleConfig(this);
    }
}
