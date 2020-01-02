using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Commands.Steps
{
    internal interface IStepCommand : ICommand
    {
        StepContainer StepInfo { get; }
    }
}
