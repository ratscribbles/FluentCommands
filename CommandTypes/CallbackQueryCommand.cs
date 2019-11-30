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
using FluentCommands.Helper;

namespace FluentCommands.CommandTypes
{
    internal class CallbackQueryCommand : Command
    {
        internal CommandDelegate<CallbackQueryEventArgs>? Invoke { get; }
        internal CommandDelegate<CallbackQueryEventArgs, IStep>? Invoke_ReturnStep { get; }

        internal CallbackQueryCommand(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, module)
        {
            if (AuxiliaryMethods.TryConvertDelegate<CallbackQueryEventArgs, IStep>(method, out var c_step)) Invoke_ReturnStep = c_step;
            else if (AuxiliaryMethods.TryConvertDelegate<CallbackQueryEventArgs>(method, out var c)) Invoke = c;
            else throw new ArgumentException();
        }
    }
}
