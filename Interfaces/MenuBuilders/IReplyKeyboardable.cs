using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IReplyKeyboardable
    {
        public IDoesSomething ReplyMarkup(IReplyMarkup replyMarkup);
        public IDoesSomething ReplyMarkup(Action<KeyboardBuilders.IKeyboardBuilder>)
    }
}
