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
    internal delegate Task UpdateCommandDelegate(TelegramBotClient c, UpdateEventArgs e);
    internal delegate Task<Menu> UpdateCommandMenuDelegate(TelegramBotClient c, UpdateEventArgs e);
    internal class UpdateCommand : Command
    {
        internal UpdateCommandDelegate? Invoke { get; }
        internal UpdateCommandMenuDelegate? InvokeWithMenuItem { get; }

        internal UpdateCommand(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, module)
        {
            if(method.ReturnType == typeof(Task<Menu>))
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
