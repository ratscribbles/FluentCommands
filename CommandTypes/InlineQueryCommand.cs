using System;
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
    internal delegate Task InlineQueryCommandDelegate(TelegramBotClient c, InlineQueryEventArgs e);
    internal delegate Task<Menu> InlineQueryCommandMenuDelegate(TelegramBotClient c, InlineQueryEventArgs e);
    internal class InlineQueryCommand : Command
    {
        internal InlineQueryCommandDelegate? Invoke { get; private set; }
        internal InlineQueryCommandMenuDelegate? InvokeWithMenuItem { get; private set; }

        internal InlineQueryCommand(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, module)
        {
            if(method.ReturnType == typeof(Task<Menu>))
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
