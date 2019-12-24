using FluentCommands.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FluentCommands.Commands
{
    //: desc
    public class MessageContext : ICommandContext<MessageEventArgs>
    {
        private readonly Type _moduleType;
        Type ICommandContext<MessageEventArgs>.ModuleType => _moduleType;
        public IReadOnlyList<string> Arguments { get; }
        public TelegramBotClient Client { get; }
        public MessageEventArgs EventArgs { get; }

        internal MessageContext(Type moduleType, TelegramBotClient client, MessageEventArgs eventArgs)
        {
            _moduleType = moduleType;
            Client = client;
            EventArgs = eventArgs;
            _ = eventArgs.TryGetArgs(out var args);
            Arguments = args;
        }

        public static implicit operator MessageContext((Type ModuleType, TelegramBotClient Client, MessageEventArgs Args) u) => new MessageContext(u.ModuleType, u.Client, u.Args);
    }
}
