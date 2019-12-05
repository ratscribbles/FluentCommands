using FluentCommands.Attributes;
using FluentCommands.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Telegram.Bot.Args;

namespace FluentCommands.CommandTypes
{
    /// <summary>
    /// Responsible for containing the possible delegates that can be invoked from a <see cref="CommandDelegate{TArgs, TReturn}"/> of this {TReturn}.
    /// <para>Used primarily for added features for the <see cref="Command"/> class that are not limited to the parent <see cref="Command"/> object's type.</para>
    /// </summary>
    internal class CommandInvoker<TReturn>
    {
        private readonly CommandDelegate<CallbackQueryEventArgs, TReturn>? _callbackQuery;
        private readonly CommandDelegate<ChosenInlineResultEventArgs, TReturn>? _chosenInlineResult;
        private readonly CommandDelegate<InlineQueryEventArgs, TReturn>? _inlineQuery;
        private readonly CommandDelegate<MessageEventArgs, TReturn>? _message;
        private readonly CommandDelegate<UpdateEventArgs, TReturn>? _update;

        /// <summary>
        /// Can be one of 5 delegate types:
        /// <para>* CommandDelegate{CallbackQueryEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{ChosenInlineResultEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{InlineQueryEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{MessageEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{UpdateEventArgs, TReturn}</para>
        /// </summary>
        internal Type DelegateType { get; }
        /// <summary>
        /// Can be one of 5 delegates:
        /// <para>* CommandDelegate{CallbackQueryEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{ChosenInlineResultEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{InlineQueryEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{MessageEventArgs, TReturn}</para>
        /// <para>* CommandDelegate{UpdateEventArgs, TReturn}</para>
        /// </summary>
        internal Delegate Delegate
        {
            get
            {
                Type t = DelegateType;
#nullable disable
                return t switch
                {
                    var _ when t == typeof(CommandDelegate<CallbackQueryEventArgs, TReturn>) => _callbackQuery,
                    var _ when t == typeof(CommandDelegate<ChosenInlineResultEventArgs, TReturn>) => _chosenInlineResult,
                    var _ when t == typeof(CommandDelegate<InlineQueryEventArgs, TReturn>) => _inlineQuery,
                    var _ when t == typeof(CommandDelegate<MessageEventArgs, TReturn>) => _message,
                    var _ when t == typeof(CommandDelegate<UpdateEventArgs, TReturn>) => _update,
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
                case var _ when t == typeof(CallbackQueryEventArgs):
                    AuxiliaryMethods.TryConvertDelegate<CallbackQueryEventArgs, TReturn>(method, out var c1);
                    _callbackQuery = c1; DelegateType = typeof(CommandDelegate<CallbackQueryEventArgs, TReturn>);
                    break;
                case var _ when t == typeof(ChosenInlineResultEventArgs):
                    AuxiliaryMethods.TryConvertDelegate<ChosenInlineResultEventArgs, TReturn>(method, out var c2);
                    _chosenInlineResult = c2; DelegateType = typeof(CommandDelegate<ChosenInlineResultEventArgs, TReturn>);
                    break;
                case var _ when t == typeof(InlineQueryEventArgs):
                    AuxiliaryMethods.TryConvertDelegate<InlineQueryEventArgs, TReturn>(method, out var c3);
                    _inlineQuery = c3; DelegateType = typeof(CommandDelegate<InlineQueryEventArgs, TReturn>);
                    break;
                case var _ when t == typeof(MessageEventArgs):
                    AuxiliaryMethods.TryConvertDelegate<MessageEventArgs, TReturn>(method, out var c4);
                    _message = c4; DelegateType = typeof(CommandDelegate<MessageEventArgs, TReturn>);
                    break;
                case var _ when t == typeof(UpdateEventArgs):
                    AuxiliaryMethods.TryConvertDelegate<UpdateEventArgs, TReturn>(method, out var c5);
                    _update = c5; DelegateType = typeof(CommandDelegate<UpdateEventArgs, TReturn>);
                    break;
                default: throw new ArgumentException($"Step {method.GetCustomAttribute<StepAttribute>()!.StepNum} had invalid method signature.");
            }
        }
    }
}
