using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors
{
    public interface IBuildInlineKeyboardButtonReference<TNext> : IFluentInterface where TNext : ICommandBaseBuilder
    {
        internal InlineKeyboardButton? In_Button { get; set; }

        TNext InlineKeyboardButtonReference(InlineKeyboardButton button) { In_Button = button; return (TNext)this; }
    }

    public interface IBuildInlineKeyboardButtonReference : IFluentInterface
    {
        internal InlineKeyboardButton? In_Button { get; set; }

        void InlineKeyboardButtonReference(InlineKeyboardButton button) { In_Button = button; }
    }
}
