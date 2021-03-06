﻿using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using FluentCommands.Extensions;

namespace FluentCommands.Commands
{
    //: Descrip
    public class UpdateContext : ICommandContext<UpdateEventArgs>
    {
        Type ICommandContext<UpdateEventArgs>.ModuleType_Internal => ModuleType;
        internal Type ModuleType { get; }
        public IReadOnlyList<string> Arguments { get; }
        public TelegramBotClient Client { get; }
        public UpdateEventArgs EventArgs { get; }

        internal UpdateContext(Type moduleType, TelegramBotClient client, UpdateEventArgs eventArgs)
        {
            ModuleType = moduleType;
            Client = client;
            EventArgs = eventArgs;
            _ = eventArgs.TryGetArgs(out var args);
            Arguments = args;
        }

        public static implicit operator UpdateContext((Type ModuleType, TelegramBotClient Client, UpdateEventArgs Args) u) => new UpdateContext(u.ModuleType, u.Client, u.Args);
    }
}
