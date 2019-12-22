using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentCommands.Builders;
using FluentCommands.Interfaces;
using FluentCommands.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FluentCommands
{
    public abstract class CommandModule<TModule> where TModule : class
    {
        /// <summary>The class that contains the actual command implementations for this module.</summary>
        internal Type CommandClass { get; } = typeof(TModule);

        protected CommandModule() { }
        internal CommandModule(Action<ModuleBuilder> onBuilding) { } // for testing

        /// <summary>
        /// Builds a <see cref="CommandTypes.Command{TModule}"/> module.
        /// </summary>
        /// <param name="moduleBuilder"></param>
        protected virtual void OnBuilding(ModuleBuilder moduleBuilder) { }
        /// <summary>
        /// Sets the configuration for this <see cref="ModuleBuilder"/>.
        /// </summary>
        /// <param name="moduleBuilderConfig"></param>
        protected virtual void OnConfiguring(ModuleConfigBuilder config) { }

        //: document 
        protected virtual async void OnCallbackQuery(object? sender, CallbackQueryEventArgs e) { }
        protected virtual async void OnChosenInlineResult(object? sender, ChosenInlineResultEventArgs e) { }
        protected virtual async void OnInlineQuery(object? sender, InlineQueryEventArgs e) { }
        protected virtual async void OnMessage(object? sender, MessageEventArgs e) { }
        protected virtual async void OnUpdate(object? sender, UpdateEventArgs e) { }

        internal void RegisterHandlers(TelegramBotClient client)
        {
            if (client is null) return;

            client.OnCallbackQuery += OnCallbackQuery;
            client.OnCallbackQuery += Evaluate_OnCallbackQuery;
            client.OnInlineResultChosen += OnChosenInlineResult;
            client.OnInlineResultChosen += Evaluate_OnChosenInlineResult;
            client.OnInlineQuery += OnInlineQuery;
            client.OnInlineQuery += Evaluate_OnInlineQuery;
            client.OnMessage += OnMessage;
            client.OnMessage += Evaluate_OnMessage;
            client.OnUpdate += OnUpdate;
            client.OnUpdate += Evaluate_OnUpdate;
        }

        private async void Evaluate_OnCallbackQuery(object? sender, CallbackQueryEventArgs e)
            => await CommandService.Evaluate_ToHandler<TModule>(e).ConfigureAwait(false);
        private async void Evaluate_OnChosenInlineResult(object? sender, ChosenInlineResultEventArgs e)
            => await CommandService.Evaluate_ToHandler<TModule>(e).ConfigureAwait(false);
        private async void Evaluate_OnInlineQuery(object? sender, InlineQueryEventArgs e)
            => await CommandService.Evaluate_ToHandler<TModule>(e).ConfigureAwait(false);
        private async void Evaluate_OnMessage(object? sender, MessageEventArgs e)
            => await CommandService.Evaluate_ToHandler<TModule>(e).ConfigureAwait(false);
        private async void Evaluate_OnUpdate(object? sender, UpdateEventArgs e)
            => await CommandService.Evaluate_ToHandler<TModule>(e).ConfigureAwait(false);
    }
}
