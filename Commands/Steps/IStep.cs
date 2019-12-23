using FluentCommands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommands.Commands.Steps
{
    public interface IStep : IFluentInterface
    {
        internal CancellationToken Token { get; }
        internal StepResult StepResult { get; }
        internal StepAction StepAction { get; }
        internal int Delay { get; }
        internal int StepToMove { get; }
        internal Func<Task>? OnResult { get; }
    }
}
