using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Logging
{
    /// <summary></summary>
    public enum FluentLogLevel { Fatal, Error, Warning, Info, Debug }

    /// <summary>h</summary>
    [Serializable]
    public class FluentLoggingEventArgs : EventArgs
    {
        /// <summary></summary>
        public DateTime DateTime { get; } = DateTime.Now;
        /// <summary></summary>
        public Exception? Exception { get; }
        /// <summary></summary>
        public FluentLogLevel LogLevel { get; }
        /// <summary></summary>
        public string Message { get; }
        /// <summary></summary>
        public Type? Module { get; }
        /// <summary></summary>
        public TelegramUpdateEventArgs? TelegramEventArgs { get; }
        /// <summary></summary>
        public StackTrace? StackTrace
        {
            get
            {
                if (CommandService.GlobalConfig.MaximumLogLevel != FluentLogLevel.Debug) return null;
                else return new StackTrace(true);
            }
        }

        internal FluentLoggingEventArgs(FluentLogLevel l, string m, Exception? e = null, TelegramUpdateEventArgs? t = null, Type? module = null)
        {
            Exception = e;
            LogLevel = l;
            Message = m;
            Module = module;
            TelegramEventArgs = t;
        }

        /// <summary></summary>
        new internal FluentLoggingEventArgs Empty() => new FluentLoggingEventArgs(FluentLogLevel.Fatal, "");
    }
}
