using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Commands.Steps
{
    /// <summary>Represents the result of a <see cref="Step"/> based on the defintion of a <see cref="Command{TArgs}"/> step.</summary>
    public enum StepResult
    {
        /// <summary>The default result. Occurs when the user is not currently in a <see cref="Command{TArgs}"/> with steps defined.</summary>
        None,
        /// <summary>The result when a <see cref="Step"/> is marked as entering a failed state.<para>The default <see cref="StepAction"/> for <see cref="StepResult.Failure"/> is <see cref="StepAction.Stop"/>.</para></summary>
        Failure,
        /// <summary>The result when a <see cref="Step"/> is marked as entering a success state.<para>The default <see cref="StepAction"/> for <see cref="StepResult.Success"/> is <see cref="StepAction.Next"/>.</para></summary>
        Success
    }
    /// <summary>Represents the navigation type for the next <see cref="Step"/> provided to the user.</summary>
    public enum StepAction
    {
        /// <summary>The default result. Occurs when the user is not currently in a <see cref="Command{TArgs}"/> with steps defined.</summary>
        None,
        /// <summary>The result when a user is moving to the next positive <see cref="Step"/>.<para>This is the default <see cref="StepAction"/> for <see cref="StepResult.Success"/>.</para></summary>
        Next,
        /// <summary>The result when a user is moving to a specified <see cref="Step"/> number.</summary>
        Move,
        /// <summary>The result when a user is repeating the last <see cref="Step"/> executed.</summary>
        Redo,
        /// <summary>The result when a user is returning to the parent <see cref="Step"/> (labeled step 0).</summary>
        Restart,
        /// <summary>The result when a user is terminating <see cref="Step"/> navigation.<para>This is the default <see cref="StepAction"/> for <see cref="StepResult.Failure"/>.</para></summary>
        Stop,
        /// <summary>The result when a user is moving to the <see cref="Step"/> before the last <see cref="Step"/> executed by this user.</summary>
        Undo
    }
}
