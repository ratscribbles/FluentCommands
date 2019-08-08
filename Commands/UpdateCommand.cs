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
    internal delegate Task UpdateCommandDelegate(TelegramBotClient c, UpdateEventArgs e);
    internal delegate Task<MenuItem> UpdateCommandMenuDelegate(TelegramBotClient c, UpdateEventArgs e);
    internal class UpdateCommand : Command
    {
        internal UpdateCommandDelegate Invoke { get; private set; }
        internal UpdateCommandMenuDelegate InvokeWithMenuItem { get; private set; }

        internal UpdateCommand(CommandBase commandBase, MethodInfo method) : base(commandBase)
        {
            if(method.ReturnType == typeof(Task<MenuItem>))
            {
                InvokeWithMenuItem = (UpdateCommandMenuDelegate)Delegate.CreateDelegate(typeof(UpdateCommandMenuDelegate), null, method);
            }
            else if(method.ReturnType == typeof(Task))
            {
                Invoke = (UpdateCommandDelegate)Delegate.CreateDelegate(typeof(UpdateCommandDelegate), null, method);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
