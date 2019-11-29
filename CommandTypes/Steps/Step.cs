using FluentCommands.Interfaces;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommands.CommandTypes.Steps
{
    public enum StepResult { None, Failure, Success }
    internal enum StepAction { None, Move, Redo, Undo }

    public class Step : IStep, IFluentInterface
    {
        private readonly CancellationToken _token;
        private readonly StepResult _stepResult;
        private readonly Func<Task>? _onSuccess;
        private readonly Func<Task>? _onFailure;
        private StepAction _stepAction;
        private int? _stepToMove;
        private int? _delay;
        private StepResult _previousStepResult; //: Probably unnecessary
         
        public StepResult PreviousStepResult => _previousStepResult; //: Figure out a way for this to be grabbed from outside of this class.
        //: PROBABLY accomplished by getting the current State of the user, which should contain the step result from the previous one as well
        CancellationToken IStep.Token => _token;
        StepResult IStep.StepResult => _stepResult;
        StepAction IStep.StepAction => _stepAction;
        /// <summary>In milliseconds.</summary>
        int? IStep.Delay => _delay;
        int? IStep.StepToMove => _stepToMove;
        Func<Task>? IStep.OnSuccess => _onSuccess;
        Func<Task>? IStep.OnFailure => _onFailure;

        private Step(StepResult result) => _stepResult = result;
        private Step(Func<Task> action, StepResult result)
        {
            switch (result)
            {
                case StepResult.Failure:
                    _onFailure = action; _stepResult = result;
                    break;
                case StepResult.Success:
                    _onSuccess = action; _stepResult = result;
                    break;
            }
        }
        private Step(Func<Task> action, StepResult result, CancellationToken token)
        {
            switch (result)
            {
                case StepResult.Failure:
                    _onFailure = action; _stepResult = result;
                    break;
                case StepResult.Success:
                    _onSuccess = action; _stepResult = result;
                    break;
            }

            _token = token;
        }

        /// <summary>
        /// Marks a <see cref="Step"/> as successful and moves to the next <see cref="Step"/> (by default).
        /// <para>Use the <see cref="Func{T}"/> overload of this method to execute code when this <see cref="Step"/> is successful.</para>
        /// <para>To modify the default behavior, use the <see cref="IStep"/> "extension" methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Success() => new Step(StepResult.Success);
        /// <summary>
        /// Marks a <see cref="Step"/> as successful and moves to the next <see cref="Step"/> (by default).
        /// <para>Use the <see cref="Func{T}"/> overload of this method to execute code when this <see cref="Step"/> is successful.</para>
        /// <para>To modify the default behavior, use the <see cref="IStep"/> "extension" methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Success(Func<Task> onSuccess) => new Step(onSuccess, StepResult.Success);
        /// <summary>
        /// Marks a <see cref="Step"/> as successful and moves to the next <see cref="Step"/> (by default).
        /// <para>Use the <see cref="Func{T}"/> overload of this method to execute code when this <see cref="Step"/> is successful.</para>
        /// <para>To modify the default behavior, use the <see cref="IStep"/> "extension" methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Success(Func<Task> onSuccess, CancellationToken token) => new Step(onSuccess, StepResult.Success, token);
        /// <summary>
        /// Marks a <see cref="Step"/> as failed and terminates movement to the next step (by default).
        /// <para>Use the <see cref="Func{T}"/> overload of this method to execute code when this <see cref="Step"/> fails.</para>
        /// <para>To modify the default behavior, use the <see cref="IStep"/> "extension" methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Failure() => new Step(StepResult.Failure);
        /// <summary>
        /// Marks a <see cref="Step"/> as failed and terminates movement to the next step (by default).
        /// <para>Use the <see cref="Func{T}"/> overload of this method to execute code when this <see cref="Step"/> fails.</para>
        /// <para>To modify the default behavior, use the <see cref="IStep"/> "extension" methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Failure(Func<Task> onFailure) => new Step(onFailure, StepResult.Failure);
        /// <summary>
        /// Marks a <see cref="Step"/> as failed and terminates movement to the next step (by default).
        /// <para>Use the <see cref="Func{T}"/> overload of this method to execute code when this <see cref="Step"/> fails.</para>
        /// <para>To modify the default behavior, use the <see cref="IStep"/> "extension" methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Failure(Func<Task> onFailure, CancellationToken token) => new Step(onFailure, StepResult.Failure, token);

        /// <summary>
        /// Forces this <see cref="Step"/> to repeat upon completion.
        /// <para>Care should be taken with this setting; an infinitely repeating <see cref="Step"/> is possible.</para>
        /// <para>The <see cref="RedoThisStep(int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public IStep RedoThisStep() { _stepAction = StepAction.Redo; return this; }
        /// <summary>
        /// Forces this <see cref="Step"/> to repeat upon completion.
        /// <para>Care should be taken with this setting; an infinitely repeating <see cref="Step"/> is possible.</para>
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public IStep RedoThisStep(int delay) { _delay = delay; _stepAction = StepAction.Redo; return this; }
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the first step of this <see cref="Command"/> (the <see cref="Step"/> marked <c>0</c>) upon completion.
        /// <para>The <see cref="ReturnToStart(int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public IStep ReturnToStart() { _stepAction = StepAction.Move; _stepToMove = 0; return this; }
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the first step of this <see cref="Command"/> (the <see cref="Step"/> marked <c>0</c>) upon completion.
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public IStep ReturnToStart(int delay) { _delay = delay; _stepAction = StepAction.Move; _stepToMove = 0; return this; }
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the <see cref="Step"/> number provided upon completion.
        /// <para>If the step does not exist, terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TModule}"/>.</para>
        /// <para>The <see cref="MoveToStep(int, int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public IStep MoveToStep(int stepNumber) { _stepAction = StepAction.Move; _stepToMove = stepNumber; return this; }
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the <see cref="Step"/> number provided upon completion.
        /// <para>If the step does not exist, terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TModule}"/>.</para>
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public IStep MoveToStep(int stepNumber, int delay) { _delay = delay; _stepAction = StepAction.Move; _stepToMove = stepNumber; return this; }
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the previous <see cref="Step"/> upon completion.
        /// <para>If the previous <see cref="Step"/> does not exist (or the previous <see cref="Step"/> is negative), terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TModule}"/>.</para>
        /// <para>The <see cref="GoToPrevious(int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public IStep GoToPrevious() { _stepAction = StepAction.Undo; return this; }
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the previous <see cref="Step"/> upon completion.
        /// <para>If the previous <see cref="Step"/> does not exist (or the previous <see cref="Step"/> is negative), terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TModule}"/>.</para>
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public IStep GoToPrevious(int delay) { _delay = delay; _stepAction = StepAction.Undo; return this; }
    }
}