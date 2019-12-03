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
    internal class ChosenInlineResultCommand : Command
    {
        internal CommandDelegate<ChosenInlineResultEventArgs>? Invoke {  get; }

        internal ChosenInlineResultCommand(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, module)
        {
            if (AuxiliaryMethods.TryConvertDelegate<ChosenInlineResultEventArgs>(method, out var c)) Invoke = c;
            else throw new ArgumentException();
        }
    }
}
