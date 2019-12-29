using FluentCommands.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Logging
{
    /// <summary>
    /// Represents a valid logger to use for FluentCommands logging events.
    /// </summary>
    public interface IFluentLogger
    {
        Task LogFatal(string message, Exception? e = null, TelegramUpdateEventArgs? t = null);
        Task LogError(string message, Exception? e = null, TelegramUpdateEventArgs? t = null);
        Task LogWarning(string message, Exception? e = null, TelegramUpdateEventArgs? t = null);
        Task LogInfo(string message, Exception? e = null, TelegramUpdateEventArgs? t = null);
        Task LogDebug(string message, Exception? e = null, TelegramUpdateEventArgs? t = null);
    }
}
