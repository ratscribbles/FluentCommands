using FluentCommands.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Logging
{
    /// <summary>
    /// Represents a logger that doesn't perform any logging function.
    /// </summary>
    internal class EmptyLogger : IFluentLogger
    {
        async Task IFluentLogger.LogFatal(string message, Exception? e, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.LogError(string message, Exception? e, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.LogWarning(string message, Exception? e, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.LogInfo(string message, Exception? e, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.LogDebug(string message, Exception? e, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
    }
}
