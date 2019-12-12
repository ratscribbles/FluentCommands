﻿using FluentCommands.Cache;
using FluentCommands.Interfaces;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace FluentCommands.CommandTypes.Steps
{
    public enum StepResult { None, Failure, Success }
    public enum StepAction { None, Next, Move, Redo, Restart, Undo }

    public class Step : IStep, IFluentInterface
    {
        private readonly CancellationToken _token;
        private readonly StepResult _stepResult;
        private readonly Func<Task>? _onResult;
        private StepAction _stepAction;
        private int _stepToMove;
        private int _delay;

        //: public static StepResult PreviousStepResult => _previousStepResult; //: Figure out a way for this to be grabbed from outside of this class.
        //: PROBABLY accomplished by getting the current State of the user, which should contain the step result from the previous one as well
        CancellationToken IStep.Token => _token;
        StepResult IStep.StepResult => _stepResult;
        StepAction IStep.StepAction => _stepAction;
        /// <summary>In milliseconds.</summary>
        int IStep.Delay => _delay;
        int IStep.StepToMove => _stepToMove;
        Func<Task>? IStep.OnResult => _onResult;

        private Step(StepResult result)
        {
            switch (result)
            {
                case StepResult.Failure:
                    _stepResult = StepResult.Failure;
                    _stepAction = StepAction.None;
                    break;
                case StepResult.Success:
                    _stepResult = StepResult.Success;
                    _stepAction = StepAction.Next;
                    break;
            }
        }
        private Step(Func<Task> action, StepResult result)
        {
            switch (result)
            {
                case StepResult.Failure:
                    _stepResult = StepResult.Failure;
                    _stepAction = StepAction.None;
                    break;
                case StepResult.Success:
                    _stepResult = StepResult.Success;
                    _stepAction = StepAction.Next;
                    break;
            }
            _onResult = action;
            _stepResult = result;
        }
        private Step(Func<Task> action, StepResult result, CancellationToken token)
        {
            switch (result)
            {
                case StepResult.Failure:
                    _stepResult = StepResult.Failure;
                    _stepAction = StepAction.None;
                    break;
                case StepResult.Success:
                    _stepResult = StepResult.Success;
                    _stepAction = StepAction.Next;
                    break;
            }
            _onResult = action; 
            _stepResult = result;
            _token = token;
        }

        internal IStep SetTo_Redo() { _stepAction = StepAction.Redo; return this; }
        internal IStep SetTo_Redo(int delay) { _stepAction = StepAction.Redo; _delay = delay; return this; }
        internal IStep SetTo_Restart() { _stepAction = StepAction.Restart; return this; }
        internal IStep SetTo_Restart(int delay) { _stepAction = StepAction.Restart; _delay = delay; return this; }
        internal IStep SetTo_Move(int stepNumber) { _stepAction = StepAction.Move; _stepToMove = stepNumber; return this; }
        internal IStep SetTo_Move(int stepNumber, int delay) { _delay = delay; _stepAction = StepAction.Move; _stepToMove = stepNumber; return this; }
        internal IStep SetTo_GotoPrevious() { _stepAction = StepAction.Undo; return this; }
        internal IStep SetTo_GotoPrevious(int delay) { _stepAction = StepAction.Undo; _delay = delay; return this; }

        public static async Task<StepState> LastStep(MessageEventArgs e) { return (await CommandService.Cache.GetState(e.Message.Chat.Id, e.Message.From.Id)).StepState; } //: Create overloads for this so that it works with eventargs lmao

        /// <summary>
        /// Marks a <see cref="Step"/> as successful and moves to the next <see cref="Step"/> (by default).
        /// <para>If the next <see cref="Step"/> cannot be found (this step is negative, or the next positive step is not exactly 1 after this step), terminates <see cref="Step"/> navigation as "completed".</para>
        /// <para>Use the <see cref="Func{T}"/> (<see cref="{TResult}"/> as <see cref="Task"/>) overload of this method to execute code when this <see cref="Step"/> is successful.</para>
        /// <para>To modify the default behavior, use the <see cref="Step"/> extension methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Success() => new Step(StepResult.Success);
        /// <summary>
        /// Marks a <see cref="Step"/> as successful and moves to the next <see cref="Step"/> (by default).
        /// <para>If the next <see cref="Step"/> cannot be found (this step is negative, or the next positive step is not exactly 1 after this step), terminates <see cref="Step"/> navigation as "completed".</para>
        /// <para>Use the <see cref="Func{T}"/> (<see cref="{TResult}"/> as <see cref="Task"/>) overload of this method to execute code when this <see cref="Step"/> is successful.</para>
        /// <para>To modify the default behavior, use the <see cref="Step"/> extension methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Success(Func<Task> onSuccess) => new Step(onSuccess, StepResult.Success);
        /// <summary>
        /// Marks a <see cref="Step"/> as successful and moves to the next <see cref="Step"/> (by default).
        /// <para>If the next <see cref="Step"/> cannot be found (this step is negative, or the next positive step is not exactly 1 after this step), terminates <see cref="Step"/> navigation as "completed".</para>
        /// <para>Use the <see cref="Func{T}"/> (<see cref="{TResult}"/> as <see cref="Task"/>) overload of this method to execute code when this <see cref="Step"/> is successful.</para>
        /// <para>To modify the default behavior, use the <see cref="Step"/> extension methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Success(Func<Task> onSuccess, CancellationToken token) => new Step(onSuccess, StepResult.Success, token);
        /// <summary>
        /// Marks a <see cref="Step"/> as failed and terminates movement to the next step (by default).
        /// <para>Use the <see cref="Func{T}"/> (<see cref="{TResult}"/> as <see cref="Task"/>) overload of this method to execute code when this <see cref="Step"/> fails.</para>
        /// <para>To modify the default behavior, use the <see cref="Step"/> extension methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Failure() => new Step(StepResult.Failure);
        /// <summary>
        /// Marks a <see cref="Step"/> as failed and terminates movement to the next step (by default).
        /// <para>Use the <see cref="Func{T}"/> (<see cref="{TResult}"/> as <see cref="Task"/>) overload of this method to execute code when this <see cref="Step"/> fails.</para>
        /// <para>To modify the default behavior, use the <see cref="Step"/> extension methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Failure(Func<Task> onFailure) => new Step(onFailure, StepResult.Failure);
        /// <summary>
        /// Marks a <see cref="Step"/> as failed and terminates movement to the next step (by default).
        /// <para>Use the <see cref="Func{T}"/> (<see cref="{TResult}"/> as <see cref="Task"/>) overload of this method to execute code when this <see cref="Step"/> fails.</para>
        /// <para>To modify the default behavior, use the <see cref="Step"/> extension methods provided on this <see cref="Step"/>.</para>
        /// </summary>
        public static Step Failure(Func<Task> onFailure, CancellationToken token) => new Step(onFailure, StepResult.Failure, token);
    }

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
        public static IStep RedoThisStep(this Step s) => s.SetTo_Redo();
        /// <summary>
        /// Forces this <see cref="Step"/> to repeat upon completion.
        /// <para>Care should be taken with this setting; an infinitely repeating <see cref="Step"/> is possible.</para>
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static IStep RedoThisStep(this Step s, int delay) => s.SetTo_Redo(delay);
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the first step of this <see cref="Command"/> (the <see cref="Step"/> marked <c>0</c>) upon completion.
        /// <para>The <see cref="ReturnToStart(int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static IStep ReturnToStart(this Step s) => s.SetTo_Restart();
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the first step of this <see cref="Command"/> (the <see cref="Step"/> marked <c>0</c>) upon completion.
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static IStep ReturnToStart(this Step s, int delay) => s.SetTo_Restart(delay);
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the <see cref="Step"/> number provided upon completion.
        /// <para>If the step does not exist, terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TModule}"/>.</para>
        /// <para>The <see cref="MoveToStep(int, int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static IStep MoveToStep(this Step s, int stepNumber) => s.SetTo_Move(stepNumber);
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the <see cref="Step"/> number provided upon completion.
        /// <para>If the step does not exist, terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TModule}"/>.</para>
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static IStep MoveToStep(this Step s, int stepNumber, int delay) => s.SetTo_Move(stepNumber, delay);
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the previous <see cref="Step"/> upon completion.
        /// <para>If the previous <see cref="Step"/> does not exist (or the previous <see cref="Step"/> is negative), terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TModule}"/>.</para>
        /// <para>The <see cref="GoToPrevious(int)"/> overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static IStep GoToPrevious(this Step s) => s.SetTo_GotoPrevious();
        /// <summary>
        /// Forces this <see cref="Step"/> to go to the previous <see cref="Step"/> upon completion.
        /// <para>If the previous <see cref="Step"/> does not exist (or the previous <see cref="Step"/> is negative), terminates this <see cref="Command"/> with the default error message of this <see cref="Command"/>'s <see cref="CommandModule{TModule}"/>.</para>
        /// <para>This overload allows this action to be delayed (in milliseconds).</para>
        /// </summary>
        public static IStep GoToPrevious(this Step s, int delay) => s.SetTo_GotoPrevious(delay);
    }
}