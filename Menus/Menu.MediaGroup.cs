﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.MediaGroupBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Menus
{
    public partial class Menu : IMenuMediaGroupOptionalBuilder, IMenuMediaGroupDisableNotification,
        IMenuMediaGroupReplyMarkup, IKeyboardBuilder<IMenuMediaGroupReplyMarkup>
    {
        #region Optional
        IMenuMediaGroupDisableNotification IMenuMediaGroupOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenu IMenuMediaGroupOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuMediaGroupOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenu IMenuMediaGroupDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuMediaGroupDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenu IMenuMediaGroupReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuMediaGroupReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Keyboard Implementation
        IKeyboardBuilder<IMenuMediaGroupReplyMarkup> IReplyMarkupable<IMenuMediaGroupReplyMarkup>.ReplyMarkup() => this;

        IMenuMediaGroupReplyMarkup IReplyMarkupable<IMenuMediaGroupReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuMediaGroupReplyMarkup IReplyMarkupable<IMenuMediaGroupReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuMediaGroupReplyMarkup IReplyMarkupable<IMenuMediaGroupReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuMediaGroupReplyMarkup IReplyMarkupable<IMenuMediaGroupReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuMediaGroupReplyMarkup IKeyboardBuilder<IMenuMediaGroupReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuMediaGroupReplyMarkup IKeyboardBuilder<IMenuMediaGroupReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateReply(CommandService.UpdateKeyboardRows(keyboard.ReplyRows));
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuMediaGroupReplyMarkup IKeyboardBuilder<IMenuMediaGroupReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuMediaGroupReplyMarkup IKeyboardBuilder<IMenuMediaGroupReplyMarkup>.ForceReply(bool selective)
        {
            var keyboard = new ForceReplyMarkup
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }
        #endregion
    }
}