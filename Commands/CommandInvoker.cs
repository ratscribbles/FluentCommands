using FluentCommands.Attributes;
using FluentCommands.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Telegram.Bot.Args;

namespace FluentCommands.Commands
{
    /// <summary>
    /// Responsible for containing the possible delegates that can be invoked from a <see cref="CommandDelegate{TArgs, TReturn}"/> of this {TReturn}.
    /// <para>Used primarily for added features for the <see cref="Command"/> class that are not limited to the parent <see cref="Command"/> object's type.</para>
    /// </summary>
    internal class CommandInvoker<TReturn>
    {
        private readonly CommandDelegate<CallbackQueryContext, CallbackQueryEventArgs, TReturn>? _callbackQuery;
        private readonly CommandDelegate<ChosenInlineResultContext, ChosenInlineResultEventArgs, TReturn>? _chosenInlineResult;
        private readonly CommandDelegate<InlineQueryContext, InlineQueryEventArgs, TReturn>? _inlineQuery;
        private readonly CommandDelegate<MessageContext, MessageEventArgs, TReturn>? _message;
        private readonly CommandDelegate<UpdateContext, UpdateEventArgs, TReturn>? _update;

        /// <summary>
        /// Can be one of 5 delegate types:
        /// <para>* CommandDelegate{CallbackQueryContext, CallbackQueryEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{ChosenInlineResultContext, ChosenInlineResultEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{InlineQueryContext, InlineQueryEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{MessageContext, MessageEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{UpdateContext, UpdateEventArgs, TReturn}</para>
        /// </summary>
        internal Type DelegateType { get; }
        /// <summary>
        /// Can be one of 5 delegates:
        /// <para>* CommandDelegate{CallbackQueryContext, CallbackQueryEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{ChosenInlineResultContext, ChosenInlineResultEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{InlineQueryContext, InlineQueryEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{MessageContext, MessageEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{UpdateContext, UpdateEventArgs, TReturn}</para>
        /// </summary>
        internal Delegate Delegate
        {
            get
            {
                Type t = DelegateType;
#nullable disable
                return t switch
                {
                    var _ when t == typeof(CommandDelegate<CallbackQueryContext, CallbackQueryEventArgs, TReturn>) => _callbackQuery,
                    var _ when t == typeof(CommandDelegate<ChosenInlineResultContext, ChosenInlineResultEventArgs, TReturn>) => _chosenInlineResult,
                    var _ when t == typeof(CommandDelegate<InlineQueryContext, InlineQueryEventArgs, TReturn>) => _inlineQuery,
                    var _ when t == typeof(CommandDelegate<MessageContext, MessageEventArgs, TReturn>) => _message,
                    var _ when t == typeof(CommandDelegate<UpdateContext, UpdateEventArgs, TReturn>) => _update,
                    _ => throw new ArgumentNullException("No delegate was found. (This exception should NEVER occur. If it does, please contact the creator of the library.)"),
                };
#nullable enable
            }
        }

        /// <summary>
        /// Responsible for containing the possible delegates that can be invoked from a <see cref="CommandDelegate{TArgs, TReturn}"/> of this {TReturn}.
        /// <para>Used primarily for added features for the <see cref="Command"/> class that are not limited to the parent <see cref="Command"/> object's type.</para>
        /// </summary>
        internal CommandInvoker(MethodInfo method)
        {
            Type t = method.GetParameters()[1].ParameterType;
            switch (t)
            {
                case var _ when t == typeof(CallbackQueryContext):
                    AuxiliaryMethods.TryConvertDelegate<CallbackQueryContext, CallbackQueryEventArgs, TReturn>(method, out var c1);
                    _callbackQuery = c1; DelegateType = typeof(CommandDelegate<CallbackQueryContext, CallbackQueryEventArgs, TReturn>);
                    break;
                case var _ when t == typeof(ChosenInlineResultContext):
                    AuxiliaryMethods.TryConvertDelegate<ChosenInlineResultContext, ChosenInlineResultEventArgs, TReturn>(method, out var c2);
                    _chosenInlineResult = c2; DelegateType = typeof(CommandDelegate<ChosenInlineResultContext, ChosenInlineResultEventArgs, TReturn>);
                    break;
                case var _ when t == typeof(InlineQueryContext):
                    AuxiliaryMethods.TryConvertDelegate<InlineQueryContext, InlineQueryEventArgs, TReturn>(method, out var c3);
                    _inlineQuery = c3; DelegateType = typeof(CommandDelegate<InlineQueryContext, InlineQueryEventArgs, TReturn>);
                    break;
                case var _ when t == typeof(MessageContext):
                    AuxiliaryMethods.TryConvertDelegate<MessageContext, MessageEventArgs, TReturn>(method, out var c4);
                    _message = c4; DelegateType = typeof(CommandDelegate<MessageContext, MessageEventArgs, TReturn>);
                    break;
                case var _ when t == typeof(UpdateContext):
                    AuxiliaryMethods.TryConvertDelegate<UpdateContext, UpdateEventArgs, TReturn>(method, out var c5);
                    _update = c5; DelegateType = typeof(CommandDelegate<UpdateContext, UpdateEventArgs, TReturn>);
                    break;
                default: throw new ArgumentException($"Step {method.GetCustomAttribute<StepAttribute>()!.StepNum} had invalid method signature.");
            }
        }
    }
}
