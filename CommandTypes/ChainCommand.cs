using FluentCommands.Builders;
using FluentCommands.Exceptions;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace FluentCommands.CommandTypes
{
    internal delegate Task ChainCommandDelegate(TelegramBotClient c, TelegramUpdateEventArgs e);
    internal delegate Task ChainCommandDelegateWithResult<T>(TelegramBotClient c, TelegramUpdateEventArgs e, ChainResult<T> result);
    internal delegate Task<Menu> ChainCommandMenuDelegate(TelegramBotClient c, TelegramUpdateEventArgs e);
    internal delegate Task<Menu> ChainCommandMenuDelegateWithResult<T>(TelegramBotClient c, TelegramUpdateEventArgs e, ChainResult<T> result);
    internal class ChainCommand<T> : Command
    {
        internal int Step { get; }
        internal ChainCommandDelegate? Invoke { get; }
        internal ChainCommandDelegateWithResult<T>? InvokeWithResult { get; }
        internal ChainCommandMenuDelegate? InvokeWithMenuItem { get; }
        internal ChainCommandMenuDelegateWithResult<T>? InvokeWithMenuItemWithResult { get; }

        internal ChainCommand(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, module)
        {
            var length = method?.GetParameters()?.Length ?? throw new CommandOnBuildingException("Method or its parameters were null.");

            if (method.ReturnType == typeof(Task<Menu>))
            {
                if (length == 3) InvokeWithMenuItemWithResult = (ChainCommandMenuDelegateWithResult<T>)Delegate.CreateDelegate(typeof(ChainCommandMenuDelegateWithResult<T>), null, method);
                else if (length == 2) InvokeWithMenuItem = (ChainCommandMenuDelegate)Delegate.CreateDelegate(typeof(ChainCommandMenuDelegate), null, method);
                else throw new ArgumentException();
            }
            else if (method.ReturnType == typeof(Task))
            {
                if (length == 3) InvokeWithResult = (ChainCommandDelegateWithResult<T>)Delegate.CreateDelegate(typeof(ChainCommandDelegateWithResult<T>), null, method);
                else if (length == 2) Invoke = (ChainCommandDelegate)Delegate.CreateDelegate(typeof(ChainCommandDelegate), null, method);
                else throw new ArgumentException();
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
