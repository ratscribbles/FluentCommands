using FluentCommands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommands.Commands.Steps
{
    /// <summary>
    /// Marks a <see cref="Steps.Step"/> as having a unique <see cref="StepAction"/> to be executed.
    /// </summary>
    /// <remarks>Container class that removes the ambiguity of the return type for end-users, but simulates a fluent interface's design of hiding certain methods from showing up with intellisense.
    /// <para>Used to prevent users from providing an additional <see cref="StepExtensions"/> method after providing one already.</para></remarks>
    public class StepWithAction : IFluentInterface
    {
        internal Step Step { get; }

        private StepWithAction(Step step) => Step = step;

        public static implicit operator Step(StepWithAction stepWithAction) => stepWithAction.Step;
        public static implicit operator StepWithAction(Step step) => new StepWithAction(step);
    }
}
