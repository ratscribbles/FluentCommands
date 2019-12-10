using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FluentCommands.Logging
{
    public enum FluentLogLevel { Fatal, Error, Warning, Info, Debug }

    internal class CommandServiceLogger : IFluentLogger
    {
        internal CommandServiceLogger() { }

        async Task IFluentLogger.Fatal(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Fatal, message, e, t).ConfigureAwait(false);
        async Task IFluentLogger.Error(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Error, message, e, t).ConfigureAwait(false);
        async Task IFluentLogger.Warning(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Warning, message, e, t).ConfigureAwait(false);
        async Task IFluentLogger.Info(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Info, message, e, t).ConfigureAwait(false);
        async Task IFluentLogger.Debug(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Debug, message, e, t).ConfigureAwait(false);

        private async Task Logging_Internal(FluentLogLevel l, string m, Exception? e = null, TelegramUpdateEventArgs? t = null)
        {
            if ((int)l < (int)CommandService.GlobalConfig.MaximumLogLevel) return;

            //var args = new FluentLoggingEventArgs(l, m, e, t, _loggerType);
            //: Make this print to console by default.
        }
    }
}
