using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Telegram.Bot.Args;

namespace FluentCommands
{
    /// <summary>
    /// A container class for the 5 most commonly-used Telegram EventArgs for obtaining user input:
    /// <para><see cref="Telegram.Bot.Args.CallbackQueryEventArgs"/>, <see cref="Telegram.Bot.Args.ChosenInlineResultEventArgs"/>, <see cref="Telegram.Bot.Args.InlineQueryEventArgs"/>, <see cref="Telegram.Bot.Args.MessageEventArgs"/>, <see cref="Telegram.Bot.Args.UpdateEventArgs"/>.</para>
    /// </summary>
    public class TelegramUpdateEventArgs
    {
        internal CallbackQueryEventArgs? CallbackQueryEventArgs { get; private set; }
        internal ChosenInlineResultEventArgs? ChosenInlineResultEventArgs { get; private set; }
        internal InlineQueryEventArgs? InlineQueryEventArgs { get; private set; }
        internal MessageEventArgs? MessageEventArgs { get; private set; }
        internal UpdateEventArgs? UpdateEventArgs { get; private set; }
        internal bool HasNoArgs => (CallbackQueryEventArgs is null || CallbackQueryEventArgs.CallbackQuery is null)
                                && (ChosenInlineResultEventArgs is null || ChosenInlineResultEventArgs.ChosenInlineResult is null)
                                && (InlineQueryEventArgs is null || InlineQueryEventArgs.InlineQuery is null)
                                && (MessageEventArgs is null || MessageEventArgs.Message is null)
                                && (UpdateEventArgs is null || UpdateEventArgs.Update is null);

        public static implicit operator TelegramUpdateEventArgs(CallbackQueryEventArgs e) => new TelegramUpdateEventArgs { CallbackQueryEventArgs = e };
        public static implicit operator TelegramUpdateEventArgs(ChosenInlineResultEventArgs e) => new TelegramUpdateEventArgs { ChosenInlineResultEventArgs = e };
        public static implicit operator TelegramUpdateEventArgs(InlineQueryEventArgs e) => new TelegramUpdateEventArgs { InlineQueryEventArgs = e };
        public static implicit operator TelegramUpdateEventArgs(MessageEventArgs e) => new TelegramUpdateEventArgs { MessageEventArgs = e };
        public static implicit operator TelegramUpdateEventArgs(UpdateEventArgs e) => new TelegramUpdateEventArgs { UpdateEventArgs = e };

        /// <summary>Attempts to get the CallbackQueryEventArgs contained within.</summary>
        /// <para>Outs a null when false. Can -never- be null when true.</para>
        public bool TryGetCallbackQueryEventArgs([NotNullWhen(true)] out CallbackQueryEventArgs? e)
        {
            if(CallbackQueryEventArgs is null) { e = null; return false; }
            else { e = CallbackQueryEventArgs; return true; }
        }
        /// <summary>Attempts to get the ChosenInlineResultEventArgs contained within.</summary>
        /// <para>Outs a null when false. Can -never- be null when true.</para>
        public bool TryGetChosenInlineResultEventArgs([NotNullWhen(true)] out ChosenInlineResultEventArgs? e)
        {
            if (ChosenInlineResultEventArgs is null) { e = null; return false; }
            else { e = ChosenInlineResultEventArgs; return true; }
        }
        /// <summary>Attempts to get the InlineQueryEventArgs contained within.</summary>
        /// <para>Outs a null when false. Can -never- be null when true.</para>
        public bool TryGetInlineQueryEventArgs([NotNullWhen(true)] out InlineQueryEventArgs? e)
        {
            if (InlineQueryEventArgs is null) { e = null; return false; }
            else { e = InlineQueryEventArgs; return true; }
        }
        /// <summary>Attempts to get the MessageEventArgs contained within.</summary>
        /// <para>Outs a null when false. Can -never- be null when true.</para>
        public bool TryGetMessageEventArgs([NotNullWhen(true)] out MessageEventArgs? e)
        {
            if (MessageEventArgs is null) { e = null; return false; }
            else { e = MessageEventArgs; return true; }
        }
        /// <summary>Attempts to get the UpdateEventArgs contained within.</summary>
        /// <para>Outs a null when false. Can -never- be null when true.</para>
        public bool TryGetUpdateEventArgs([NotNullWhen(true)] out UpdateEventArgs? e)
        {
            if (UpdateEventArgs is null) { e = null; return false; }
            else { e = UpdateEventArgs; return true; }
        }
    }
}
