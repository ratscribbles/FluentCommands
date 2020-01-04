using FluentCommands.Commands.Steps;
using FluentCommands.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FluentCommands.Commands
{
    internal class StepCommandBase<TContext, TArgs> : CommandBase<TContext, TArgs>, IStepCommand where TContext : ICommandContext<TArgs> where TArgs : EventArgs
    {
        internal StepContainer StepInfo { get; }
        StepContainer IStepCommand.StepInfo => StepInfo;

        internal StepCommandBase(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, method, module)
        {
            if (commandBase.CommandType != CommandType.Step) throw new CommandOnBuildingException("There was an error building Commands (StepCommandBase id not have the correct CommandType). This exception should not happen; please report it as a bug to the FluentCommands github page.");
            StepInfo = commandBase.StepInfo!; // Not Null if building a StepCommandBase.
        }
    }
}
