using FluentCommands.Cache;
using FluentCommands.Commands;
using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace FluentCommands.Interfaces
{
    internal interface IReadOnlyModule
    {
        internal ModuleConfig Config { get; }
        internal IFluentLogger Logger { get; }
        internal Type TypeStorage { get; }
        internal IFluentCache Cache { get; }
        internal TelegramBotClient? Client { get; }
        internal bool UseModuleLogger { get; }
        internal bool UseModuleCache { get; }
        internal bool UseClient { get; }
    }
}
