using FluentCommands.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FluentCommands.Commands
{
    //: desc
    public class CallbackQueryContext : ICommandContext<CallbackQueryEventArgs>
    {
        private readonly Type _moduleType;
        Type ICommandContext<CallbackQueryEventArgs>.ModuleType => _moduleType;
        public IReadOnlyList<string> Arguments { get; }
        public TelegramBotClient Client { get; }
        public CallbackQueryEventArgs EventArgs { get; }

        internal CallbackQueryContext(Type moduleType, TelegramBotClient client, CallbackQueryEventArgs eventArgs)
        {
            _moduleType = moduleType;
            Client = client;
            EventArgs = eventArgs;
            _ = eventArgs.TryGetArgs(out var args);
            Arguments = args;
        }

        public static implicit operator CallbackQueryContext((Type ModuleType, TelegramBotClient Client, CallbackQueryEventArgs Args) u) => new CallbackQueryContext(u.ModuleType, u.Client, u.Args);
    }
}
