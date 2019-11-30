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
    internal class MessageCommand : Command
    {
        internal CommandDelegate<MessageEventArgs>? Invoke { get; }
        internal CommandDelegate<MessageEventArgs, IStep>? Invoke_ReturnStep { get; }

        internal MessageCommand(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, module)
        {
            if (AuxiliaryMethods.TryConvertDelegate<MessageEventArgs, IStep>(method, out var c_step)) Invoke_ReturnStep = c_step;
            else if (AuxiliaryMethods.TryConvertDelegate<MessageEventArgs>(method, out var c)) Invoke = c;
            else throw new ArgumentException();
        }
    }
}
