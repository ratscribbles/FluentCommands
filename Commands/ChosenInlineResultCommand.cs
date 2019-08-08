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
    internal delegate Task ChosenInlineResultCommandDelegate(TelegramBotClient c, ChosenInlineResultEventArgs e);
    internal delegate Task<MenuItem> ChosenInlineResultCommandMenuDelegate(TelegramBotClient c, ChosenInlineResultEventArgs e);
    internal class ChosenInlineResultCommand : Command
    {
        internal ChosenInlineResultCommandDelegate Invoke { get; private set; }
        internal ChosenInlineResultCommandMenuDelegate InvokeWithMenuItem { get; private set; }

        internal ChosenInlineResultCommand(CommandBase commandBase, MethodInfo method) : base(commandBase)
        {
            if(method.ReturnType == typeof(Task<MenuItem>))
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
