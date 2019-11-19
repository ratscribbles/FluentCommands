using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    public class CommandServiceConfig
    {
        public bool Logging { get; set; }
        public bool UseDefaultRules { get; set; }
        public bool UseDefaultErrorMsg { get; set; }
        public bool UseInternalStateHandlerForReplyKeyboards { get; set; }
        public bool UseGlobalLogging { get; set; }
        public bool CaptureAllLoggingEvents { get; set; }
        public LoggingEvent UseLoggingEventHandler { get; set; }
        public FluentLogLevel MaximumLogLevel { get; set; }
        public bool SwallowExceptions { get; set; }
        public string DefaultPrefix { get; set; } = "/";
        public MenuMode DefaultMenuMode { get; set; } = MenuMode.NoAction;

        public CommandServiceConfig() { }
    }
}
