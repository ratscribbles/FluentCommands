using FluentCommands.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FluentCommands.Commands
{
    //: desc
    public class ChosenInlineResultContext : ICommandContext<ChosenInlineResultEventArgs>
    {
        private readonly Type _moduleType;
        Type ICommandContext<ChosenInlineResultEventArgs>.ModuleType => _moduleType;
        public IReadOnlyList<string> Arguments { get; }
        public TelegramBotClient Client { get; }
        public ChosenInlineResultEventArgs EventArgs { get; }

        internal ChosenInlineResultContext(Type moduleType, TelegramBotClient client, ChosenInlineResultEventArgs eventArgs)
        {
            _moduleType = moduleType;
            Client = client;
            EventArgs = eventArgs;
            _ = eventArgs.TryGetArgs(out var args);
            Arguments = args;
        }

        public static implicit operator ChosenInlineResultContext((Type ModuleType, TelegramBotClient Client, ChosenInlineResultEventArgs Args) u) => new ChosenInlineResultContext(u.ModuleType, u.Client, u.Args);
    }
}
