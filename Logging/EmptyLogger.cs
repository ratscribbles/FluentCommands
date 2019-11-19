﻿using System;
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
        async Task IFluentLogger.Fatal(string message) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Fatal(string message, Exception? e) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Fatal(string message, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Fatal(string message, Exception? e, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);

        async Task IFluentLogger.Error(string message) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Error(string message, Exception? e) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Error(string message, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Error(string message, Exception? e, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);

        async Task IFluentLogger.Warning(string message) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Warning(string message, Exception? e) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Warning(string message, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Warning(string message, Exception? e, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);

        async Task IFluentLogger.Info(string message) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Info(string message, Exception? e) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Info(string message, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Info(string message, Exception? e, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);

        async Task IFluentLogger.Debug(string message) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Debug(string message, Exception? e) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Debug(string message, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
        async Task IFluentLogger.Debug(string message, Exception? e, TelegramUpdateEventArgs? t) => await Task.CompletedTask.ConfigureAwait(false);
    }
}
