﻿using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Builders
{
    public class CommandServiceConfigBuilder
    {
        public bool Logging { get; set; }
        public bool UseDefaultRules { get; set; }
        public bool UseDefaultErrorMsg { get; set; }
        public bool UseInternalStateHandlerForReplyKeyboards { get; set; }
        public bool UseGlobalLogging { get; set; }
        public bool CaptureAllLoggingEvents { get; set; }
        public bool SwallowExceptions { get; set; } = false;
        public LoggingEvent? UseLoggingEventHandler { get; set; }
        public FluentLogLevel MaximumLogLevel { get; set; } = FluentLogLevel.Fatal;
        public string DefaultPrefix { get; set; } = "/";
        public MenuMode DefaultMenuMode { get; set; } = MenuMode.NoAction;
    }
}