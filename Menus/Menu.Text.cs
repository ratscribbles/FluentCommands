using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.TextBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Commands;

namespace FluentCommands.Menus
{
    public partial class Menu : IMenuTextOptionalBuilder,
        IMenuTextCancellationToken, IMenuTextDisableNotification, IMenuTextDisableWebPagePreview, IMenuTextParseMode,
        IMenuTextReplyMarkup, IKeyboardBuilder<IMenuTextReplyMarkup>
    {
        #region Optional
        IMenuTextCancellationToken IMenuTextOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuTextDisableNotification IMenuTextOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuTextDisableWebPagePreview IMenuTextOptionalBuilder.DisableWebPagePreview(bool disableWebPagePreview) { DisableWebPagePreview = disableWebPagePreview; return this; }
        IMenuTextParseMode IMenuTextOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        Menu IMenuTextOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuTextOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuTextDisableNotification IMenuTextCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuTextDisableWebPagePreview IMenuTextCancellationToken.DisableWebPagePreview(bool disableWebPagePreview) { DisableWebPagePreview = disableWebPagePreview; return this; }
        IMenuTextParseMode IMenuTextCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        Menu IMenuTextCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuTextCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuTextDisableWebPagePreview IMenuTextDisableNotification.DisableWebPagePreview(bool disableWebPagePreview) { DisableWebPagePreview = disableWebPagePreview; return this; }
        IMenuTextParseMode IMenuTextDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        Menu IMenuTextDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuTextDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuTextParseMode IMenuTextDisableWebPagePreview.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        Menu IMenuTextDisableWebPagePreview.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuTextDisableWebPagePreview.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        Menu IMenuTextParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuTextParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        Menu IMenuTextReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuTextReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Keyboard Implementation
        IKeyboardBuilder<IMenuTextReplyMarkup> IReplyMarkupable<IMenuTextReplyMarkup>.ReplyMarkup() => this;

        IMenuTextReplyMarkup IReplyMarkupable<IMenuTextReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuTextReplyMarkup IReplyMarkupable<IMenuTextReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuTextReplyMarkup IReplyMarkupable<IMenuTextReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuTextReplyMarkup IReplyMarkupable<IMenuTextReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuTextReplyMarkup IKeyboardBuilder<IMenuTextReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuTextReplyMarkup IKeyboardBuilder<IMenuTextReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateReply(CommandService.UpdateKeyboardRows(keyboard.ReplyRows));
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuTextReplyMarkup IKeyboardBuilder<IMenuTextReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuTextReplyMarkup IKeyboardBuilder<IMenuTextReplyMarkup>.ForceReply(bool selective)
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
