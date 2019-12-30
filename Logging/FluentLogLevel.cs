using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Logging
{
    /// <summary>
    /// Determines the LogLevel for a logging event in the FluentCommands library.
    /// </summary>
    public enum FluentLogLevel 
    {
        /// <summary>LogLevel for critical events. <para>Usually results in a crash unless <see cref="CommandServiceConfig.SwallowCriticalExceptions"/> is enabled, in some cases.</para></summary>
        Fatal, 
        /// <summary>LogLevel for events that cause an error.</summary>
        Error,
        /// <summary>LogLevel for events that cause a warning.</summary>
        Warning,
        /// <summary>LogLevel for events that provide additional information about internal processes, such as command evaluations.</summary>
        Info,
        /// <summary>LogLevel for events that otherwise are unnecessary to record unless debugging your current FluentCommands setup.</summary>
        Debug
    }
}
