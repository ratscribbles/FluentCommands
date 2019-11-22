using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FluentCommands.Logging
{
    internal class ModuleLogger : IFluentLogger
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

        private readonly Type _loggerType;
        private readonly ModuleConfig _config;
        private LoggingEvent? _loggingEvent;
        private LoggingEvent OnLoggingEvent
        {
            get
            {
                if (_loggingEvent is null) return e => { return Task.CompletedTask; };
                else return _loggingEvent;
            }
        }

        internal ModuleLogger(Type t, ModuleConfig c)
        {
            _loggerType = t;
            _config = c;
        }

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

        private async Task Logging_Internal(FluentLogLevel l, string m, Exception? e = null, TelegramUpdateEventArgs? t = null)
        {
            if ((int)l < (int)_config.MaximumLogLevel) return;

            var args = new FluentLoggingEventArgs(l, m, e, t, _loggerType);

            if (_config.LogModuleActivities) await OnLoggingEvent(args).ConfigureAwait(false);
            if (CommandService.GlobalConfig.CaptureAllLoggingEvents) await CommandService.PassToGlobalEvent(args).ConfigureAwait(false);
        }
    }
}
