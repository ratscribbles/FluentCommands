using FluentCommands.Helper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Telegram.Bot.Args;

namespace FluentCommands.CommandTypes.Steps
{
    /// <summary>
    /// Responsible for containing the possible delegates that can be invoked within a <see cref="Step"/>.
    /// </summary>
    internal class StepInvoker
    {
        private readonly CommandDelegate<CallbackQueryEventArgs, IStep>? _callbackQuery;
        private readonly CommandDelegate<ChosenInlineResultEventArgs, IStep>? _chosenInlineResult;
        private readonly CommandDelegate<InlineQueryEventArgs, IStep>? _inlineQuery;
        private readonly CommandDelegate<MessageEventArgs, IStep>? _message;
        private readonly CommandDelegate<UpdateEventArgs, IStep>? _update;

        /// <summary>
        /// Can be one of 5 delegate types:
        /// <para>* CommandDelegate{CallbackQueryEventArgs, IStep}</para>
        /// <para>* CommandDelegate{ChosenInlineResultEventArgs, IStep}</para>
        /// <para>* CommandDelegate{InlineQueryEventArgs, IStep}</para>
        /// <para>* CommandDelegate{MessageEventArgs, IStep}</para>
        /// <para>* CommandDelegate{UpdateEventArgs, IStep}</para>
        /// </summary>
        internal Type DelegateType { get; }
        /// <summary>
        /// Can be one of 5 delegates:
        /// <para>* CommandDelegate{CallbackQueryEventArgs, IStep}</para>
        /// <para>* CommandDelegate{ChosenInlineResultEventArgs, IStep}</para>
        /// <para>* CommandDelegate{InlineQueryEventArgs, IStep}</para>
        /// <para>* CommandDelegate{MessageEventArgs, IStep}</para>
        /// <para>* CommandDelegate{UpdateEventArgs, IStep}</para>
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

        internal StepInvoker(MethodInfo method)
        {
            if (AuxiliaryMethods.TryConvertDelegate<CallbackQueryEventArgs, IStep>(method, out var c1)) { _callbackQuery = c1; DelegateType = typeof(CommandDelegate<CallbackQueryEventArgs, IStep>); }
            else if (AuxiliaryMethods.TryConvertDelegate<ChosenInlineResultEventArgs, IStep>(method, out var c2)) { _chosenInlineResult = c2; DelegateType = typeof(CommandDelegate<ChosenInlineResultEventArgs, IStep>); }
            else if (AuxiliaryMethods.TryConvertDelegate<InlineQueryEventArgs, IStep>(method, out var c3)) { _inlineQuery = c3; DelegateType = typeof(CommandDelegate<InlineQueryEventArgs, IStep>); }
            else if (AuxiliaryMethods.TryConvertDelegate<MessageEventArgs, IStep>(method, out var c4)) { _message = c4; DelegateType = typeof(CommandDelegate<MessageEventArgs, IStep>); }
            else if (AuxiliaryMethods.TryConvertDelegate<UpdateEventArgs, IStep>(method, out var c5)) { _update = c5; DelegateType = typeof(CommandDelegate<UpdateEventArgs, IStep>); }
            else throw new ArgumentException("Failure to convert Step delegate.");
        }
    }
}
