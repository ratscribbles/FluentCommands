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
    internal delegate Task ChosenInlineResultCommandDelegate(TelegramBotClient c, ChosenInlineResultEventArgs e);
    internal delegate Task<Menu> ChosenInlineResultCommandMenuDelegate(TelegramBotClient c, ChosenInlineResultEventArgs e);
    internal class ChosenInlineResultCommand : Command
    {
        internal ChosenInlineResultCommandDelegate? Invoke { get; }
        internal ChosenInlineResultCommandMenuDelegate? InvokeWithMenuItem { get; }

        internal ChosenInlineResultCommand(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, module)
        {
            if(method.ReturnType == typeof(Task<Menu>))
            {
                InvokeWithMenuItem = (ChosenInlineResultCommandMenuDelegate)Delegate.CreateDelegate(typeof(ChosenInlineResultCommandMenuDelegate), null, method);
            }
            else if(method.ReturnType == typeof(Task))
            {
                Invoke = (ChosenInlineResultCommandDelegate)Delegate.CreateDelegate(typeof(ChosenInlineResultCommandDelegate), null, method);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
