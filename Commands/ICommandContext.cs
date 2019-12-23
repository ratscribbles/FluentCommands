using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace FluentCommands.Commands
{
    internal interface ICommandContext<TArgs> where TArgs : EventArgs
    {
        internal TelegramBotClient Client { get; }
        internal string[] CommandArguments { get; }
        internal TArgs EventArgs { get; }
    }
}
