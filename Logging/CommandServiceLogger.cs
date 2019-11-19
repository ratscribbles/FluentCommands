using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FluentCommands.Logging
{
    public delegate Task LoggingEvent(FluentLoggingEventArgs e);

    internal class CommandServiceLogger : IFluentLogger
    {
        internal event LoggingEvent LoggingEvent
        {
            add
            {
                lock (this)
                {
                    _loggingEvent += value;
                }
            }
            remove
            {
                lock (this)
                {
                    _loggingEvent -= value;
                }
            }
        }

        private readonly Type _loggerType = typeof(CommandService);
        private LoggingEvent? _loggingEvent;
        private LoggingEvent OnLoggingEvent
        {
            get
            {
                if (_loggingEvent is null) return e => { return Task.CompletedTask; };
                else return _loggingEvent;
            }
        }

        internal CommandServiceLogger() { }

        async Task IFluentLogger.Fatal(string message) => await Logging_Internal(FluentLogLevel.Fatal, message).ConfigureAwait(false);
        async Task IFluentLogger.Fatal(string message, Exception? e) => await Logging_Internal(FluentLogLevel.Fatal, message, e).ConfigureAwait(false);
        async Task IFluentLogger.Fatal(string message, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Fatal, message, t: t).ConfigureAwait(false);
        async Task IFluentLogger.Fatal(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Fatal, message, e, t).ConfigureAwait(false);

        async Task IFluentLogger.Error(string message) => await Logging_Internal(FluentLogLevel.Error, message).ConfigureAwait(false);
        async Task IFluentLogger.Error(string message, Exception? e) => await Logging_Internal(FluentLogLevel.Error, message, e).ConfigureAwait(false);
        async Task IFluentLogger.Error(string message, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Error, message, t: t).ConfigureAwait(false);
        async Task IFluentLogger.Error(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Error, message, e, t).ConfigureAwait(false);

        async Task IFluentLogger.Warning(string message) => await Logging_Internal(FluentLogLevel.Warning, message).ConfigureAwait(false);
        async Task IFluentLogger.Warning(string message, Exception? e) => await Logging_Internal(FluentLogLevel.Warning, message, e).ConfigureAwait(false);
        async Task IFluentLogger.Warning(string message, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Warning, message, t: t).ConfigureAwait(false);
        async Task IFluentLogger.Warning(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Warning, message, e, t).ConfigureAwait(false);

        async Task IFluentLogger.Info(string message) => await Logging_Internal(FluentLogLevel.Info, message).ConfigureAwait(false);
        async Task IFluentLogger.Info(string message, Exception? e) => await Logging_Internal(FluentLogLevel.Info, message, e).ConfigureAwait(false);
        async Task IFluentLogger.Info(string message, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Info, message, t: t).ConfigureAwait(false);
        async Task IFluentLogger.Info(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Info, message, e, t).ConfigureAwait(false);

        async Task IFluentLogger.Debug(string message) => await Logging_Internal(FluentLogLevel.Debug, message).ConfigureAwait(false);
        async Task IFluentLogger.Debug(string message, Exception? e) => await Logging_Internal(FluentLogLevel.Debug, message, e).ConfigureAwait(false);
        async Task IFluentLogger.Debug(string message, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Debug, message, t: t).ConfigureAwait(false);
        async Task IFluentLogger.Debug(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Debug, message, e, t).ConfigureAwait(false);

        /// <summary>Passes args to the global event used by the <see cref="CommandService"/> Logger.</summary>
        internal async Task GlobalEvent(FluentLoggingEventArgs args)
        {
            await OnLoggingEvent(args).ConfigureAwait(false);
        }

        private async Task Logging_Internal(FluentLogLevel l, string m, Exception? e = null, TelegramUpdateEventArgs? t = null)
        {
            if ((int)l < (int)CommandService.GlobalConfig.MaximumLogLevel) return;

            var args = new FluentLoggingEventArgs(l, m, e, t, _loggerType);
            await OnLoggingEvent(args).ConfigureAwait(false);
        }
    }
}
