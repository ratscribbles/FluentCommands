﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using FluentCommands.Menus;
using FluentCommands.Builders;

namespace FluentCommands.CommandTypes
{
    internal delegate Task MessageCommandDelegate(TelegramBotClient c, MessageEventArgs e);
    internal delegate Task<Menu> MessageCommandMenuDelegate(TelegramBotClient c, MessageEventArgs e);
    internal class MessageCommand : Command
    {
        internal MessageCommandDelegate? Invoke { get; }
        internal MessageCommandMenuDelegate? InvokeWithMenuItem { get; }

        internal MessageCommand(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, module)
        {
            if(method.ReturnType == typeof(Task<Menu>))
            {
                InvokeWithMenuItem = (MessageCommandMenuDelegate)Delegate.CreateDelegate(typeof(MessageCommandMenuDelegate), null, method);
            }
            else if(method.ReturnType == typeof(Task))
            {
                Invoke = (MessageCommandDelegate)Delegate.CreateDelegate(typeof(MessageCommandDelegate), null, method);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
