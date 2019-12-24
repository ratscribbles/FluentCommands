using FluentCommands.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FluentCommands.Commands
{
    //: desc
    public class InlineQueryContext : ICommandContext<InlineQueryEventArgs>
    {
        private readonly Type _moduleType;
        Type ICommandContext<InlineQueryEventArgs>.ModuleType => _moduleType;
        public IReadOnlyList<string> Arguments { get; }
        public TelegramBotClient Client { get; }
        public InlineQueryEventArgs EventArgs { get; }

        internal InlineQueryContext(Type moduleType, TelegramBotClient client, InlineQueryEventArgs eventArgs)
        {
            _moduleType = moduleType;
            Client = client;
            EventArgs = eventArgs;
            _ = eventArgs.TryGetArgs(out var args);
            Arguments = args;
        }

        public static implicit operator InlineQueryContext((Type ModuleType, TelegramBotClient Client, InlineQueryEventArgs Args) u) => new InlineQueryContext(u.ModuleType, u.Client, u.Args);
    }
}
