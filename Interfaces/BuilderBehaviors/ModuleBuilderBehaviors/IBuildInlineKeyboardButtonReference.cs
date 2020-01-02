using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors
{
    public interface IBuildInlineKeyboardButtonReference<TNext> : IFluentInterface where TNext : IModuleBuilder
    {
        internal InlineKeyboardButton In_Button { get; set; }

        TNext InlineKeyboardButtonReference(InlineKeyboardButton button) { In_Button = button; return (TNext)this; }
    }
}
