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
using FluentCommands.Commands;

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
        Menus.Menu IMenuDocumentOptionalBuilder.Thumbnail(string source) { Thumbnail = source; return this; }
        Menus.Menu IMenuDocumentOptionalBuilder.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menus.Menu IMenuDocumentOptionalBuilder.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        #endregion

        #region Additional Implementation
        IMenuDocumentCaption IMenuDocumentCancellationToken.Caption(string caption) { Caption = caption; return this; }
        IMenuDocumentDisableNotification IMenuDocumentCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuDocumentParseMode IMenuDocumentCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menus.Menu IMenuDocumentCancellationToken.Thumbnail(string source) { Thumbnail = source; return this; }
        Menus.Menu IMenuDocumentCancellationToken.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menus.Menu IMenuDocumentCancellationToken.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentDisableNotification IMenuDocumentCaption.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuDocumentParseMode IMenuDocumentCaption.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCaption.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCaption.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menus.Menu IMenuDocumentCaption.Thumbnail(string source) { Thumbnail = source; return this; }
        Menus.Menu IMenuDocumentCaption.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menus.Menu IMenuDocumentCaption.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentParseMode IMenuDocumentDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menus.Menu IMenuDocumentDisableNotification.Thumbnail(string source) { Thumbnail = source; return this; }
        Menus.Menu IMenuDocumentDisableNotification.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menus.Menu IMenuDocumentDisableNotification.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentReplyToMessage IMenuDocumentParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menus.Menu IMenuDocumentParseMode.Thumbnail(string source) { Thumbnail = source; return this; }
        Menus.Menu IMenuDocumentParseMode.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menus.Menu IMenuDocumentParseMode.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentReplyToMessage IMenuDocumentReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menus.Menu IMenuDocumentReplyMarkup.Thumbnail(string source) { Thumbnail = source; return this; }
        Menus.Menu IMenuDocumentReplyMarkup.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menus.Menu IMenuDocumentReplyMarkup.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        Menus.Menu IMenuDocumentReplyToMessage.Thumbnail(string source) { Thumbnail = source; return this; }
        Menus.Menu IMenuDocumentReplyToMessage.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menus.Menu IMenuDocumentReplyToMessage.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
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
            keyboard.UpdateInline();
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuDocumentReplyMarkup IKeyboardBuilder<IMenuDocumentReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
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
