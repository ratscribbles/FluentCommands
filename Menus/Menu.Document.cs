using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.DocumentBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Menus
{
    public partial class Menu : IMenuDocumentOptionalBuilder,
        IMenuDocumentCancellationToken, IMenuDocumentCaption, IMenuDocumentDisableNotification, IMenuDocumentParseMode, IMenuDocumentReplyToMessage,
        IMenuDocumentReplyMarkup, IKeyboardBuilder<IMenuDocumentReplyMarkup>
    {
        #region Optional
        IMenuDocumentCancellationToken IMenuDocumentOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuDocumentCaption IMenuDocumentOptionalBuilder.Caption(string caption) { Caption = caption; return this; }
        IMenuDocumentDisableNotification IMenuDocumentOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuDocumentParseMode IMenuDocumentOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenu IMenuDocumentOptionalBuilder.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenu IMenuDocumentOptionalBuilder.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenu IMenuDocumentOptionalBuilder.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        #endregion

        #region Additional Implementation
        IMenuDocumentCaption IMenuDocumentCancellationToken.Caption(string caption) { Caption = caption; return this; }
        IMenuDocumentDisableNotification IMenuDocumentCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuDocumentParseMode IMenuDocumentCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenu IMenuDocumentCancellationToken.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenu IMenuDocumentCancellationToken.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenu IMenuDocumentCancellationToken.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentDisableNotification IMenuDocumentCaption.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuDocumentParseMode IMenuDocumentCaption.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCaption.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCaption.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenu IMenuDocumentCaption.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenu IMenuDocumentCaption.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenu IMenuDocumentCaption.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentParseMode IMenuDocumentDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenu IMenuDocumentDisableNotification.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenu IMenuDocumentDisableNotification.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenu IMenuDocumentDisableNotification.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentReplyToMessage IMenuDocumentParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenu IMenuDocumentParseMode.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenu IMenuDocumentParseMode.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenu IMenuDocumentParseMode.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentReplyToMessage IMenuDocumentReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenu IMenuDocumentReplyMarkup.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenu IMenuDocumentReplyMarkup.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenu IMenuDocumentReplyMarkup.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenu IMenuDocumentReplyToMessage.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenu IMenuDocumentReplyToMessage.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenu IMenuDocumentReplyToMessage.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        #endregion

        #region Keyboard Implementation
        IKeyboardBuilder<IMenuDocumentReplyMarkup> IReplyMarkupable<IMenuDocumentReplyMarkup>.ReplyMarkup() => this;

        IMenuDocumentReplyMarkup IReplyMarkupable<IMenuDocumentReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuDocumentReplyMarkup IReplyMarkupable<IMenuDocumentReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuDocumentReplyMarkup IReplyMarkupable<IMenuDocumentReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuDocumentReplyMarkup IReplyMarkupable<IMenuDocumentReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuDocumentReplyMarkup IKeyboardBuilder<IMenuDocumentReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuDocumentReplyMarkup IKeyboardBuilder<IMenuDocumentReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateReply(CommandService.UpdateKeyboardRows(keyboard.ReplyRows));
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuDocumentReplyMarkup IKeyboardBuilder<IMenuDocumentReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuDocumentReplyMarkup IKeyboardBuilder<IMenuDocumentReplyMarkup>.ForceReply(bool selective)
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
