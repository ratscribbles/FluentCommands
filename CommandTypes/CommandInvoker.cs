using FluentCommands.Helper;
using System;
using System.Collections.Generic;
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
                if (_callbackQuery is { }) return _callbackQuery;
                else if (_chosenInlineResult is { }) return _chosenInlineResult;
                else if(_inlineQuery is { }) return _inlineQuery;
                else if(_message is { }) return _message;
                else if(_update is { }) return _update;
                else throw new ArgumentNullException("No delegate was found. (This exception should NEVER occur. If it does, please contact the creator of the library.)");
            }
        }

        /// <summary>
        /// Responsible for containing the possible delegates that can be invoked from a <see cref="CommandDelegate{TArgs, TReturn}"/> of this {TReturn}.
        /// <para>Used primarily for added features for the <see cref="Command"/> class that are not limited to the parent <see cref="Command"/> object's type.</para>
        /// </summary>
        internal CommandInvoker(MethodInfo method)
        {
            if (AuxiliaryMethods.TryConvertDelegate<CallbackQueryEventArgs, TReturn>(method, out var c1)) { _callbackQuery = c1; DelegateType = typeof(CommandDelegate<CallbackQueryEventArgs, TReturn>); }
            else if (AuxiliaryMethods.TryConvertDelegate<ChosenInlineResultEventArgs, TReturn>(method, out var c2)) { _chosenInlineResult = c2; DelegateType = typeof(CommandDelegate<ChosenInlineResultEventArgs, TReturn>); }
            else if (AuxiliaryMethods.TryConvertDelegate<InlineQueryEventArgs, TReturn>(method, out var c3)) { _inlineQuery = c3; DelegateType = typeof(CommandDelegate<InlineQueryEventArgs, TReturn>); }
            else if (AuxiliaryMethods.TryConvertDelegate<MessageEventArgs, TReturn>(method, out var c4)) { _message = c4; DelegateType = typeof(CommandDelegate<MessageEventArgs, TReturn>); }
            else if (AuxiliaryMethods.TryConvertDelegate<UpdateEventArgs, TReturn>(method, out var c5)) { _update = c5; DelegateType = typeof(CommandDelegate<UpdateEventArgs, TReturn>); }
            else throw new ArgumentException("Failure to convert delegates.");
        }
    }
}
