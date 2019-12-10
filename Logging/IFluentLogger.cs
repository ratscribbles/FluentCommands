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
        Task Fatal(string message, Exception? e = null, TelegramUpdateEventArgs? t = null);
        Task Error(string message, Exception? e = null, TelegramUpdateEventArgs? t = null);
        Task Warning(string message, Exception? e = null, TelegramUpdateEventArgs? t = null);
        Task Info(string message, Exception? e = null, TelegramUpdateEventArgs? t = null);
        Task Debug(string message, Exception? e = null, TelegramUpdateEventArgs? t = null);
    }
}
