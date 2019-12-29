using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Commands;
using FluentCommands.Logging;
using FluentCommands.Utility;

namespace FluentCommands
{
    /// <summary>
    /// Collects logging information from the <see cref="CommandService"/> building process, and ships this information to the logger.
    /// </summary>
    internal class CommandServiceOnBuildingNotifier
    {
        private readonly ToggleOnce _wasStarted = new ToggleOnce(false);
        private readonly List<(FluentLogLevel l, string m, Exception? e, TelegramUpdateEventArgs? t)> _notifyingArgs = new List<(FluentLogLevel l, string m, Exception? e, TelegramUpdateEventArgs? t)>();

        internal void AddDebug(string message, Exception? e = null, TelegramUpdateEventArgs? t = null)
            => _notifyingArgs.Add((FluentLogLevel.Debug, message, e, t));
        internal void AddInfo(string message, Exception? e = null, TelegramUpdateEventArgs? t = null)
            => _notifyingArgs.Add((FluentLogLevel.Info, message, e, t));
        internal void AddWarning(string message, Exception? e = null, TelegramUpdateEventArgs? t = null)
            => _notifyingArgs.Add((FluentLogLevel.Warning, message, e, t));
        internal void AddError(string message, Exception? e = null, TelegramUpdateEventArgs? t = null)
            => _notifyingArgs.Add((FluentLogLevel.Error, message, e, t));
        internal void AddFatal(string message, Exception? e = null, TelegramUpdateEventArgs? t = null)
            => _notifyingArgs.Add((FluentLogLevel.Fatal, message, e, t));

        /// <summary>
        /// Deploys all collected notifications to the logger.
        /// </summary>
        internal void NotifyAll()
        {
            foreach(var (l, m, e, t) in _notifyingArgs)
            {
                switch (l)
                {
                    case FluentLogLevel.Debug:
                        CommandService.Logger.LogDebug(m, e, t);
                        break;
                    case FluentLogLevel.Info:
                        CommandService.Logger.LogInfo(m, e, t);
                        break;
                    case FluentLogLevel.Warning:
                        CommandService.Logger.LogWarning(m, e, t);
                        break;
                    case FluentLogLevel.Error:
                        CommandService.Logger.LogError(m, e, t);
                        break;
                    case FluentLogLevel.Fatal:
                        CommandService.Logger.LogFatal(m, e, t);
                        break;
                }
            }
        }
    }
}
