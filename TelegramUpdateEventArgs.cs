using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Args;

namespace FluentCommands
{
    public class TelegramUpdateEventArgs
    {
        public string Input { get; private set; } = "";
        public CallbackQueryEventArgs CallbackQueryEventArgs { get; private set; } = null;
        public ChosenInlineResultEventArgs ChosenInlineResultEventArgs { get; private set; } = null;
        public InlineQueryEventArgs InlineQueryEventArgs { get; private set; } = null;
        public MessageEventArgs MessageEventArgs { get; private set; } = null;
        public UpdateEventArgs UpdateEventArgs { get; private set; } = null;

        private TelegramUpdateEventArgs(CallbackQueryEventArgs e) { CallbackQueryEventArgs = e; Input = CallbackQueryEventArgs?.CallbackQuery?.Data; }
        private TelegramUpdateEventArgs(ChosenInlineResultEventArgs e) { ChosenInlineResultEventArgs = e; Input = ChosenInlineResultEventArgs?.ChosenInlineResult?.Query; }
        private TelegramUpdateEventArgs(InlineQueryEventArgs e) { InlineQueryEventArgs = e; Input = InlineQueryEventArgs?.InlineQuery?.Query; }
        private TelegramUpdateEventArgs(MessageEventArgs e) { MessageEventArgs = e; Input = MessageEventArgs?.Message?.Text; }
        private TelegramUpdateEventArgs(UpdateEventArgs e)
        {
            UpdateEventArgs = e;
            Input = e.Update?.CallbackQuery?.Data ??
                    e.Update?.ChosenInlineResult?.Query ??
                    e.Update?.InlineQuery?.Query ??
                    e.Update?.Message?.Text;
        }

        public static implicit operator TelegramUpdateEventArgs(CallbackQueryEventArgs e) => new TelegramUpdateEventArgs(e);
        public static implicit operator TelegramUpdateEventArgs(ChosenInlineResultEventArgs e) => new TelegramUpdateEventArgs(e);
        public static implicit operator TelegramUpdateEventArgs(InlineQueryEventArgs e) => new TelegramUpdateEventArgs(e);
        public static implicit operator TelegramUpdateEventArgs(MessageEventArgs e) => new TelegramUpdateEventArgs(e);
        public static implicit operator TelegramUpdateEventArgs(UpdateEventArgs e) => new TelegramUpdateEventArgs(e);
    }
}
