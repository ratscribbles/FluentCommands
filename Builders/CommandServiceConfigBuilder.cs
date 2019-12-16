using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommands.Builders
{
    public class CommandServiceConfigBuilder
    {
        public IServiceCollection Services { get; set; }
        public bool Logging { get; set; }
        public bool UseDefaultRules { get; set; }
        public bool UseDefaultErrorMsg { get; set; }
        public bool UseInternalStateHandlerForReplyKeyboards { get; set; }
        public bool UseGlobalLogging { get; set; }
        public bool CaptureAllLoggingEvents { get; set; }
        public bool SwallowCriticalExceptions { get; set; } = false;
        public FluentLogLevel MaximumLogLevel { get; set; } = FluentLogLevel.Fatal;
        public string DefaultPrefix { get; set; } = "/";
        public MenuMode DefaultMenuMode { get; set; } = MenuMode.NoAction;
    }
}
