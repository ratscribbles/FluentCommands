﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using FluentCommands.Menu;

namespace FluentCommands.Commands
{
    internal delegate Task MessageCommandDelegate(TelegramBotClient c, MessageEventArgs e);
    internal delegate Task<MenuItem> MessageCommandMenuDelegate(TelegramBotClient c, MessageEventArgs e);
    internal class MessageCommand : Command
    {
        internal MessageCommandDelegate Invoke { get; private set; }
        internal MessageCommandMenuDelegate InvokeWithMenuItem { get; private set; }

        internal MessageCommand(CommandBase commandBase, MethodInfo method) : base(commandBase)
        {
            if(method.ReturnType == typeof(Task<MenuItem>))
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