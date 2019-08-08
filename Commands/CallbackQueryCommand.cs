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
    internal delegate Task CallbackQueryCommandDelegate(TelegramBotClient c, CallbackQueryEventArgs e);
    internal delegate Task<MenuItem> CallbackQueryCommandMenuDelegate(TelegramBotClient c, CallbackQueryEventArgs e);
    internal class CallbackQueryCommand : Command
    {
        internal CallbackQueryCommandDelegate Invoke { get; private set; }
        internal CallbackQueryCommandMenuDelegate InvokeWithMenuItem { get; private set; }

        internal CallbackQueryCommand(CommandBase commandBase, MethodInfo method) : base(commandBase)
        {
            if(method.ReturnType == typeof(Task<MenuItem>))
            {
                InvokeWithMenuItem = (CallbackQueryCommandMenuDelegate)Delegate.CreateDelegate(typeof(CallbackQueryCommandMenuDelegate), null, method);
            }
            else if(method.ReturnType == typeof(Task))
            {
                Invoke = (CallbackQueryCommandDelegate)Delegate.CreateDelegate(typeof(CallbackQueryCommandDelegate), null, method);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
