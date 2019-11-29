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
using FluentCommands.CommandTypes.Steps;

namespace FluentCommands.CommandTypes
{
    internal delegate Task CallbackQueryCommandDelegate(TelegramBotClient c, CallbackQueryEventArgs e);
    internal delegate Task<IStep> CallbackQueryCommandStepDelegate(TelegramBotClient c, CallbackQueryEventArgs e);
    internal class CallbackQueryCommand : Command
    {
        internal CallbackQueryCommandDelegate? Invoke { get; }
        internal CallbackQueryCommandStepDelegate? InvokeWithMenuItem { get; }

        internal CallbackQueryCommand(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, module)
        {
            if(method.ReturnType == typeof(Task<IStep>))
            {
                InvokeWithMenuItem = (CallbackQueryCommandStepDelegate)Delegate.CreateDelegate(typeof(CallbackQueryCommandStepDelegate), null, method);
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
