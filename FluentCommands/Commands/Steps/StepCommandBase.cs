using FluentCommands.Commands.Steps;
using FluentCommands.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FluentCommands.Commands
{
    internal class StepCommandBase : CommandBase, IStepCommand
    {
        internal StepContainer StepInfo { get; }
        StepContainer IStepCommand.StepInfo => StepInfo;

        internal StepCommandBase(IEnumerable<MethodInfo> methods, CommandBaseBuilder commandBase, Type module) 
            : base(commandBase, module)
        {
            if (CommandType != CommandType.Step) throw new CommandOnBuildingException("There was an error building Commands (StepCommandBase id not have the correct CommandType). This exception should not happen; please report it as a bug to the FluentCommands github page.");
            StepInfo = new StepContainer(methods);
        }
    }
}
