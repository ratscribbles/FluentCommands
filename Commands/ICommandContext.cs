using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace FluentCommands.Commands
{
    internal interface ICommandContext<TArgs> where TArgs : EventArgs
    {
        internal Type ModuleType { get; }
        IReadOnlyList<string> Arguments { get; }
        TelegramBotClient Client { get; }
        TArgs EventArgs { get; }
    }
}
