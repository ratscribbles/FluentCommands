using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentCommands.Interfaces;
using FluentCommands.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FluentCommands.Commands
{
    public abstract class CommandModule<TCommand> where TCommand : class
    {
        /// <summary>The class that contains the actual command implementations for this module.</summary>
        internal Type CommandClass { get; } = typeof(TCommand);

        protected CommandModule() { }
        internal CommandModule(Action<ModuleBuilder> onBuilding) { } // for testing

        /// <summary>
        /// Builds a <see cref="Commands.Command{TCommand}"/> module.
        /// </summary>
        /// <param name="moduleBuilder"></param>
        protected virtual void OnBuilding(ModuleBuilder moduleBuilder) { }
        /// <summary>
        /// Sets the configuration for this <see cref="ModuleBuilder"/>.
        /// </summary>
        /// <param name="moduleBuilderConfig"></param>
        protected virtual void OnConfiguring(ModuleConfigBuilder config) { }

        //: document 
        protected virtual async void On_ApiResponseReceived(object? sender, ApiResponseEventArgs e) { }
        protected virtual async void On_MakingApiRequest(object? sender, ApiRequestEventArgs e) { }
        protected virtual async void On_CallbackQuery(object? sender, CallbackQueryEventArgs e) { }
        protected virtual async void On_ChosenInlineResult(object? sender, ChosenInlineResultEventArgs e) { }
        protected virtual async void On_InlineQuery(object? sender, InlineQueryEventArgs e) { }
        protected virtual async void On_Message(object? sender, MessageEventArgs e) { }
        protected virtual async void On_MessageEdited (object? sender, MessageEventArgs e) { }
        protected virtual async void On_ReceiveError (object? sender, ReceiveErrorEventArgs e) { }
        protected virtual async void On_ReceiveGeneralError(object? sender, ReceiveGeneralErrorEventArgs e) { }
        protected virtual async void On_Update(object? sender, UpdateEventArgs e) { }

        internal void RegisterHandlers(TelegramBotClient client, bool disableEvaluate = false)
        {
            if (client is null) return;

            if (!disableEvaluate)
            {
                client.OnCallbackQuery += Evaluate_OnCallbackQuery;
                client.OnInlineResultChosen += Evaluate_OnChosenInlineResult;
                client.OnInlineQuery += Evaluate_OnInlineQuery;
                client.OnMessage += Evaluate_OnMessage;
            }

            client.ApiResponseReceived += On_ApiResponseReceived;
            client.MakingApiRequest += On_MakingApiRequest;
            client.OnCallbackQuery += On_CallbackQuery;
            client.OnInlineResultChosen += On_ChosenInlineResult;
            client.OnInlineQuery += On_InlineQuery;
            client.OnMessage += On_Message;
            client.OnMessageEdited += On_MessageEdited;
            client.OnReceiveError += On_ReceiveError;
            client.OnReceiveGeneralError += On_ReceiveGeneralError;
            client.OnUpdate += On_Update;
        }

        private async void Evaluate_OnCallbackQuery(object? sender, CallbackQueryEventArgs e)
            => await CommandService.Evaluate_ToHandler<TCommand>(e).ConfigureAwait(false);
        private async void Evaluate_OnChosenInlineResult(object? sender, ChosenInlineResultEventArgs e)
            => await CommandService.Evaluate_ToHandler<TCommand>(e).ConfigureAwait(false);
        private async void Evaluate_OnInlineQuery(object? sender, InlineQueryEventArgs e)
            => await CommandService.Evaluate_ToHandler<TCommand>(e).ConfigureAwait(false);
        private async void Evaluate_OnMessage(object? sender, MessageEventArgs e)
            => await CommandService.Evaluate_ToHandler<TCommand>(e).ConfigureAwait(false);
    }
}
