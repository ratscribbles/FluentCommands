using FluentCommands.Cache;
using FluentCommands.Interfaces;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FluentCommands.Commands.Steps
{
    public class Step : IFluentInterface
    {
        internal CancellationToken Token { get; }
        internal StepResult StepResult { get; }
        internal StepAction StepAction { get; private set; }
        /// <summary>In milliseconds.</summary>
        internal int Delay { get; private set; }
        internal int StepToMove { get; private set; }
        internal Func<Task>? OnResult { get; }

        private Step(StepResult result)
        {
            switch (result)
            {
                case StepResult.Failure:
                    StepResult = StepResult.Failure;
                    StepAction = StepAction.Stop;
                    break;
                case StepResult.Success:
                    StepResult = StepResult.Success;
                    StepAction = StepAction.Next;
                    break;
            }
        }
        private Step(Func<Task> action, StepResult result)
        {
            switch (result)
            {
                case StepResult.Failure:
                    StepResult = StepResult.Failure;
                    StepAction = StepAction.Stop;
                    break;
                case StepResult.Success:
                    StepResult = StepResult.Success;
                    StepAction = StepAction.Next;
                    break;
            }
            OnResult = action;
            StepResult = result;
        }
        private Step(Func<Task> action, StepResult result, CancellationToken token)
        {
            switch (result)
            {
                case StepResult.Failure:
                    StepResult = StepResult.Failure;
                    StepAction = StepAction.Stop;
                    break;
                case StepResult.Success:
                    StepResult = StepResult.Success;
                    StepAction = StepAction.Next;
                    break;
            }
            OnResult = action; 
            StepResult = result;
            Token = token;
        }

        internal StepWithAction SetTo_Redo() { StepAction = StepAction.Redo; return this; }
        internal StepWithAction SetTo_Redo(int delay) { StepAction = StepAction.Redo; Delay = delay; return this; }
        internal StepWithAction SetTo_Restart() { StepAction = StepAction.Restart; return this; }
        internal StepWithAction SetTo_Restart(int delay) { StepAction = StepAction.Restart; Delay = delay; return this; }
        internal StepWithAction SetTo_Move(int stepNumber) { StepAction = StepAction.Move; StepToMove = stepNumber; return this; }
        internal StepWithAction SetTo_Move(int stepNumber, int delay) { Delay = delay; StepAction = StepAction.Move; StepToMove = stepNumber; return this; }
        internal StepWithAction SetTo_GotoPrevious() { StepAction = StepAction.Undo; return this; }
        internal StepWithAction SetTo_GotoPrevious(int delay) { StepAction = StepAction.Undo; Delay = delay; return this; }

        public static async Task<StepState> LastStep(TelegramBotClient client, CallbackQueryEventArgs e) { return (await CommandService.Cache.GetState(client.BotId, e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.From.Id)).StepState; }  //: Create overloads for this so that it works with eventargs lmao
        public static async Task<StepState> LastStep(TelegramBotClient client, MessageEventArgs e) { return (await CommandService.Cache.GetState(client.BotId, e.Message.Chat.Id, e.Message.From.Id)).StepState; } //: Create overloads for this so that it works with eventargs lmao

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
}