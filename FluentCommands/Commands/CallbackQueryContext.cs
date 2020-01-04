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
        Type ICommandContext<CallbackQueryEventArgs>.ModuleType_Internal => ModuleType;
        internal Type ModuleType { get; }
        public IReadOnlyList<string> Arguments { get; }
        public TelegramBotClient Client { get; }
        public CallbackQueryEventArgs EventArgs { get; }

        internal CallbackQueryContext(Type moduleType, TelegramBotClient client, CallbackQueryEventArgs eventArgs)
        {
            ModuleType = moduleType;
            Client = client;
            EventArgs = eventArgs;
            _ = eventArgs.TryGetArgs(out var args);
            Arguments = args;
        }

        public static implicit operator CallbackQueryContext((Type ModuleType, TelegramBotClient Client, CallbackQueryEventArgs Args) u) => new CallbackQueryContext(u.ModuleType, u.Client, u.Args);
    }
}
