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

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuDocumentBuilder, IMenuDocumentOptionalBuilder,
        IMenuDocumentCancellationToken, IMenuDocumentCaption, IMenuDocumentDisableNotification, IMenuDocumentParseMode, IMenuDocumentReplyToMessage
    {
        #region Required
        IMenuDocumentOptionalBuilder IMenuDocumentBuilder.Source(string source) { Source = source; return this; }
        IMenuDocumentOptionalBuilder IMenuDocumentBuilder.Source(Stream content) { Source = content; return this; }
        IMenuDocumentOptionalBuilder IMenuDocumentBuilder.Source(Stream content, string fileName) { Source = new InputOnlineFile(content, fileName); return this; }
        IMenuDocumentOptionalBuilder IMenuDocumentBuilder.Source(InputOnlineFile animation) { Source = animation; return this; }
        #endregion

        #region Optional
        IMenuDocumentCancellationToken IMenuDocumentOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuDocumentCaption IMenuDocumentOptionalBuilder.Caption(string caption) { Caption = caption; return this; }
        IMenuDocumentDisableNotification IMenuDocumentOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuDocumentParseMode IMenuDocumentOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuItem IMenuDocumentOptionalBuilder.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuDocumentOptionalBuilder.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuDocumentOptionalBuilder.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        #endregion

        #region Additional Implementation
        IMenuDocumentCaption IMenuDocumentCancellationToken.Caption(string caption) { Caption = caption; return this; }
        IMenuDocumentDisableNotification IMenuDocumentCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuDocumentParseMode IMenuDocumentCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuItem IMenuDocumentCancellationToken.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuDocumentCancellationToken.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuDocumentCancellationToken.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentDisableNotification IMenuDocumentCaption.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuDocumentParseMode IMenuDocumentCaption.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCaption.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentCaption.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuItem IMenuDocumentCaption.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuDocumentCaption.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuDocumentCaption.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentParseMode IMenuDocumentDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuItem IMenuDocumentDisableNotification.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuDocumentDisableNotification.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuDocumentDisableNotification.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuDocumentReplyToMessage IMenuDocumentParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuDocumentReplyToMessage IMenuDocumentParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuItem IMenuDocumentParseMode.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuDocumentParseMode.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuDocumentParseMode.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuItem IMenuDocumentReplyToMessage.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuDocumentReplyToMessage.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuDocumentReplyToMessage.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        #endregion
    }
}
