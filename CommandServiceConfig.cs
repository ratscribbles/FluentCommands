using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    internal class CommandServiceConfig
    {
        internal bool Logging { get; }
        internal bool UseDefaultRules { get; }
        internal bool UseDefaultErrorMsg { get; }
        internal bool UseInternalStateHandlerForReplyKeyboards { get; }
        internal bool UseGlobalLogging { get; }
        internal bool CaptureAllLoggingEvents { get; }
        internal LoggingEvent? UseLoggingEventHandler { get; }
        internal FluentLogLevel MaximumLogLevel { get; }
        internal bool SwallowExceptions { get; }
        internal string DefaultPrefix { get; }
        internal MenuMode DefaultMenuMode { get; }

        internal CommandServiceConfig(Builders.CommandServiceConfigBuilder c)
        {
            CaptureAllLoggingEvents = c.CaptureAllLoggingEvents;
            DefaultMenuMode = c.DefaultMenuMode;
            DefaultPrefix = c.DefaultPrefix;
            Logging = c.Logging;
            MaximumLogLevel = c.MaximumLogLevel;
            SwallowExceptions = c.SwallowExceptions;
            UseDefaultErrorMsg = c.UseDefaultErrorMsg;
            UseDefaultRules = c.UseDefaultRules;
            UseGlobalLogging = c.UseGlobalLogging;
            UseInternalStateHandlerForReplyKeyboards = c.UseInternalStateHandlerForReplyKeyboards;
            UseLoggingEventHandler = c.UseLoggingEventHandler;
        }
    }
}
