using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Logging
{
    /// <summary>
    /// Represents a valid logger to use for FluentCommands logging events.
    /// </summary>
    internal interface IFluentLogger
    {
        internal Task Fatal(string message);
        internal Task Fatal(string message, Exception? e);
        internal Task Fatal(string message, TelegramUpdateEventArgs? t);
        internal Task Fatal(string message, Exception? e, TelegramUpdateEventArgs? t);

        internal Task Error(string message);
        internal Task Error(string message, Exception? e);
        internal Task Error(string message, TelegramUpdateEventArgs? t);
        internal Task Error(string message, Exception? e, TelegramUpdateEventArgs? t);

        internal Task Warning(string message);
        internal Task Warning(string message, Exception? e);
        internal Task Warning(string message, TelegramUpdateEventArgs? t);
        internal Task Warning(string message, Exception? e, TelegramUpdateEventArgs? t);

        internal Task Info(string message);
        internal Task Info(string message, Exception? e);
        internal Task Info(string message, TelegramUpdateEventArgs? t);
        internal Task Info(string message, Exception? e, TelegramUpdateEventArgs? t);

        internal Task Debug(string message);
        internal Task Debug(string message, Exception? e);
        internal Task Debug(string message, TelegramUpdateEventArgs? t);
        internal Task Debug(string message, Exception? e, TelegramUpdateEventArgs? t);
    }
}
