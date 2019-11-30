using FluentCommands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommands.CommandTypes.Steps
{
    public interface IStep : IFluentInterface
    {
        internal CancellationToken Token { get; }
        internal StepResult StepResult { get; }
        internal StepAction StepAction { get; }
        internal int? Delay { get; }
        internal int? StepToMove { get; }
        internal Func<Task>? OnSuccess { get; }
        internal Func<Task>? OnFailure { get; }
        internal CommandInvoker<IStep>? Invoker { get; }

        internal void SetInvoker(CommandInvoker<IStep> invoker);
    }
}
