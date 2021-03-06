﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.LocationBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Commands;

namespace FluentCommands.Menus
{
    public partial class Menu : IMenuLocationOptionalBuilder,
        IMenuLocationCancellationToken, IMenuLocationDisableNotification, IMenuLocationLivePeriod,
        IMenuLocationReplyMarkup, IKeyboardBuilder<IMenuLocationReplyMarkup>
    {
        #region Optional
        IMenuLocationCancellationToken IMenuLocationOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuLocationDisableNotification IMenuLocationOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuLocationLivePeriod IMenuLocationOptionalBuilder.LivePeriod(int livePeriod) { LivePeriod = livePeriod; return this; }
        Menu IMenuLocationOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuLocationOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion

        #region Additional Implementation
        IMenuLocationDisableNotification IMenuLocationCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuLocationLivePeriod IMenuLocationCancellationToken.LivePeriod(int livePeriod) { LivePeriod = livePeriod; return this; }
        Menu IMenuLocationCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuLocationCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuLocationLivePeriod IMenuLocationDisableNotification.LivePeriod(int livePeriod) { LivePeriod = livePeriod; return this; }
        Menu IMenuLocationDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuLocationDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        Menu IMenuLocationLivePeriod.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuLocationLivePeriod.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        Menu IMenuLocationReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuLocationReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion

        #region Keyboard Implementation
        IKeyboardBuilder<IMenuLocationReplyMarkup> IReplyMarkupable<IMenuLocationReplyMarkup>.ReplyMarkup() => this;

        IMenuLocationReplyMarkup IReplyMarkupable<IMenuLocationReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuLocationReplyMarkup IReplyMarkupable<IMenuLocationReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuLocationReplyMarkup IReplyMarkupable<IMenuLocationReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuLocationReplyMarkup IReplyMarkupable<IMenuLocationReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuLocationReplyMarkup IKeyboardBuilder<IMenuLocationReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline();
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuLocationReplyMarkup IKeyboardBuilder<IMenuLocationReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuLocationReplyMarkup IKeyboardBuilder<IMenuLocationReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuLocationReplyMarkup IKeyboardBuilder<IMenuLocationReplyMarkup>.ForceReply(bool selective)
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
