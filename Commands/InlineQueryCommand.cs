using System;
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
    internal delegate Task InlineQueryCommandDelegate(TelegramBotClient c, InlineQueryEventArgs e);
    internal delegate Task<MenuItem> InlineQueryCommandMenuDelegate(TelegramBotClient c, InlineQueryEventArgs e);
    internal class InlineQueryCommand : Command
    {
        internal InlineQueryCommandDelegate Invoke { get; private set; }
        internal InlineQueryCommandMenuDelegate InvokeWithMenuItem { get; private set; }

        internal InlineQueryCommand(CommandBase commandBase, MethodInfo method) : base(commandBase)
        {
            if(method.ReturnType == typeof(Task<MenuItem>))
            {
                InvokeWithMenuItem = (InlineQueryCommandMenuDelegate)Delegate.CreateDelegate(typeof(InlineQueryCommandMenuDelegate), null, method);
            }
            else if(method.ReturnType == typeof(Task))
            {
                Invoke = (InlineQueryCommandDelegate)Delegate.CreateDelegate(typeof(InlineQueryCommandDelegate), null, method);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
