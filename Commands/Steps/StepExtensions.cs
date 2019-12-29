using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Commands.Steps
{
    /// <summary>
    /// This class provides extension methods for <see cref="Step"/> objects.
    /// </summary>
    public static class StepExtensions
    {
        /// <summary>
        /// Forces this <see cref="Step"/> to repeat upon completion.
        /// <para>Care should be taken with this setting; an infinitely repeating <see cref="Step"/> is possible.</para>
        /// <para>The <see cref="RedoThisStep(int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static StepWithAction RedoThisStep(this Step s) => s.SetTo_Redo();
        /// <summary>
        /// Forces this <see cref="Step"/> to repeat upon completion.
        /// <para>Care should be taken with this setting; an infinitely repeating <see cref="Step"/> is possible.</para>
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static StepWithAction RedoThisStep(this Step s, int delay) => s.SetTo_Redo(delay);
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the first step of this <see cref="Command"/> (the <see cref="Step"/> marked <c>0</c>) upon completion.
        /// <para>The <see cref="ReturnToStart(int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static StepWithAction ReturnToStart(this Step s) => s.SetTo_Restart();
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the first step of this <see cref="Command"/> (the <see cref="Step"/> marked <c>0</c>) upon completion.
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static StepWithAction ReturnToStart(this Step s, int delay) => s.SetTo_Restart(delay);
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the <see cref="Step"/> number provided upon completion.
        /// <para>If the step does not exist, terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TCommand}"/>.</para>
        /// <para>The <see cref="MoveToStep(int, int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static StepWithAction MoveToStep(this Step s, int stepNumber) => s.SetTo_Move(stepNumber);
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the <see cref="Step"/> number provided upon completion.
        /// <para>If the step does not exist, terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TCommand}"/>.</para>
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static StepWithAction MoveToStep(this Step s, int stepNumber, int delay) => s.SetTo_Move(stepNumber, delay);
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the previous <see cref="Step"/> upon completion.
        /// <para>If the previous <see cref="Step"/> does not exist (or the previous <see cref="Step"/> is negative), terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TCommand}"/>.</para>
        /// <para>The <see cref="GoToPrevious(int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static StepWithAction GoToPrevious(this Step s) => s.SetTo_GotoPrevious();
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the previous <see cref="Step"/> upon completion.
        /// <para>If the previous <see cref="Step"/> does not exist (or the previous <see cref="Step"/> is negative), terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TCommand}"/>.</para>
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static StepWithAction GoToPrevious(this Step s, int delay) => s.SetTo_GotoPrevious(delay);
    }
}
