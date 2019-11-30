﻿using System;
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
    internal class UpdateCommand : Command
    {
        internal CommandDelegate<UpdateEventArgs>? Invoke { get; }
        internal CommandDelegate<UpdateEventArgs, IStep>? Invoke_ReturnStep { get; }

        internal UpdateCommand(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, module)
        {
            if (AuxiliaryMethods.TryConvertDelegate<UpdateEventArgs, IStep>(method, out var c_step)) Invoke_ReturnStep = c_step;
            else if (AuxiliaryMethods.TryConvertDelegate<UpdateEventArgs>(method, out var c)) Invoke = c;
            else throw new ArgumentException();
        }
    }
}
