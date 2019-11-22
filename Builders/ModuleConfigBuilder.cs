using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentCommands.Exceptions;
using FluentCommands.Logging;
using FluentCommands.Menus;
using FluentCommands.Helper;

namespace FluentCommands
{
    public enum MenuMode
    {
        Default = 0,
        NoAction,
        EditLastMessage,
        EditOrDeleteLastMessage,
    }

    public class ModuleConfigBuilder
    {
        public bool UseInternalKeyboardStateHandler { get; set; } = false;
        public bool UseDefaultErrorMessage { get; set; } = false;
        public bool BruteForceKeyboardReferences { get; set; } = false;
        public bool DeleteCommandAfterCall { get; set; } = false;
        public bool LogModuleActivities { get; set; } = false;
        public FluentLogLevel MaximumLogLevel { get; set; } = FluentLogLevel.Fatal;
        public string Prefix { get; set; } = "/";
        public LoggingEvent? UseLoggingEventHandler { get; set; }
        public Menu DefaultErrorMessage { get; set; } = MenuItem.As().Text().TextSource("ERROR OCCURRED.").Done();
        public MenuMode MenuModeOverride { get; set; } = MenuMode.Default;
    }
}
