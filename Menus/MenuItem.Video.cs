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
using FluentCommands.Interfaces.MenuBuilders.VideoBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuVideoBuilder, IMenuVideoOptionalBuilder,
        IMenuVideoCancellationToken, IMenuVideoCaption, IMenuVideoDisableNotification, IMenuVideoDuration, IMenuVideoHeight, IMenuVideoParseMode, IMenuVideoReplyToMessage, IMenuVideoSupportsStreaming, IMenuVideoThumbnail,
        IMenuVideoReplyMarkup, IKeyboardBuilder<IMenuVideoReplyMarkup>
    {
        #region Required
        IMenuVideoOptionalBuilder IMenuVideoBuilder.Source(string source) { Source = source; return this; }
        IMenuVideoOptionalBuilder IMenuVideoBuilder.Source(Stream content) { Source = content; return this; }
        IMenuVideoOptionalBuilder IMenuVideoBuilder.Source(Stream content, string fileName) { Source = new InputOnlineFile(content, fileName); return this; }
        IMenuVideoOptionalBuilder IMenuVideoBuilder.Source(InputOnlineFile video) { Source = video; return this; }
        #endregion
        #region
        IMenuVideoCancellationToken IMenuVideoOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuVideoCaption IMenuVideoOptionalBuilder.Caption(string caption) { Caption = caption; return this; }
        IMenuVideoDisableNotification IMenuVideoOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVideoDuration IMenuVideoOptionalBuilder.Duration(int duration) { Duration = duration; return this; }
        IMenuVideoHeight IMenuVideoOptionalBuilder.Height(int height) { Height = height; return this; }
        IMenuVideoParseMode IMenuVideoOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuVideoReplyToMessage IMenuVideoOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoReplyToMessage IMenuVideoOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuVideoSupportsStreaming IMenuVideoOptionalBuilder.SupportsStreaming(bool supportsStreaming) { SupportsStreaming = supportsStreaming; return this; }
        IMenuVideoThumbnail IMenuVideoOptionalBuilder.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuVideoThumbnail IMenuVideoOptionalBuilder.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuVideoThumbnail IMenuVideoOptionalBuilder.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuVideoOptionalBuilder.Width(int width) { Width = width; return this; }
        #endregion
        #region Additional Implenentation
        IMenuVideoCaption IMenuVideoCancellationToken.Caption(string caption) { Caption = caption; return this; }
        IMenuVideoDisableNotification IMenuVideoCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVideoDuration IMenuVideoCancellationToken.Duration(int duration) { Duration = duration; return this; }
        IMenuVideoHeight IMenuVideoCancellationToken.Height(int height) { Height = height; return this; }
        IMenuVideoParseMode IMenuVideoCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuVideoReplyToMessage IMenuVideoCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoReplyToMessage IMenuVideoCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuVideoSupportsStreaming IMenuVideoCancellationToken.SupportsStreaming(bool supportsStreaming) { SupportsStreaming = supportsStreaming; return this; }
        IMenuVideoThumbnail IMenuVideoCancellationToken.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuVideoThumbnail IMenuVideoCancellationToken.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuVideoThumbnail IMenuVideoCancellationToken.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuVideoCancellationToken.Width(int width) { Width = width; return this; }
        ////
        IMenuVideoDisableNotification IMenuVideoCaption.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVideoDuration IMenuVideoCaption.Duration(int duration) { Duration = duration; return this; }
        IMenuVideoHeight IMenuVideoCaption.Height(int height) { Height = height; return this; }
        IMenuVideoParseMode IMenuVideoCaption.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuVideoReplyToMessage IMenuVideoCaption.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoReplyToMessage IMenuVideoCaption.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuVideoSupportsStreaming IMenuVideoCaption.SupportsStreaming(bool supportsStreaming) { SupportsStreaming = supportsStreaming; return this; }
        IMenuVideoThumbnail IMenuVideoCaption.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuVideoThumbnail IMenuVideoCaption.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuVideoThumbnail IMenuVideoCaption.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuVideoCaption.Width(int width) { Width = width; return this; }
        ////
        IMenuVideoDuration IMenuVideoDisableNotification.Duration(int duration) { Duration = duration; return this; }
        IMenuVideoHeight IMenuVideoDisableNotification.Height(int height) { Height = height; return this; }
        IMenuVideoParseMode IMenuVideoDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuVideoReplyToMessage IMenuVideoDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoReplyToMessage IMenuVideoDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuVideoSupportsStreaming IMenuVideoDisableNotification.SupportsStreaming(bool supportsStreaming) { SupportsStreaming = supportsStreaming; return this; }
        IMenuVideoThumbnail IMenuVideoDisableNotification.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuVideoThumbnail IMenuVideoDisableNotification.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuVideoThumbnail IMenuVideoDisableNotification.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuVideoDisableNotification.Width(int width) { Width = width; return this; }
        ////
        IMenuVideoHeight IMenuVideoDuration.Height(int height) { Height = height; return this; }
        IMenuVideoParseMode IMenuVideoDuration.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuVideoReplyToMessage IMenuVideoDuration.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoReplyToMessage IMenuVideoDuration.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuVideoSupportsStreaming IMenuVideoDuration.SupportsStreaming(bool supportsStreaming) { SupportsStreaming = supportsStreaming; return this; }
        IMenuVideoThumbnail IMenuVideoDuration.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuVideoThumbnail IMenuVideoDuration.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuVideoThumbnail IMenuVideoDuration.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuVideoDuration.Width(int width) { Width = width; return this; }
        ////
        IMenuVideoParseMode IMenuVideoHeight.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuVideoReplyToMessage IMenuVideoHeight.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoReplyToMessage IMenuVideoHeight.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuVideoSupportsStreaming IMenuVideoHeight.SupportsStreaming(bool supportsStreaming) { SupportsStreaming = supportsStreaming; return this; }
        IMenuVideoThumbnail IMenuVideoHeight.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuVideoThumbnail IMenuVideoHeight.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuVideoThumbnail IMenuVideoHeight.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuVideoHeight.Width(int width) { Width = width; return this; }
        ////
        IMenuVideoReplyToMessage IMenuVideoParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoReplyToMessage IMenuVideoParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuVideoSupportsStreaming IMenuVideoParseMode.SupportsStreaming(bool supportsStreaming) { SupportsStreaming = supportsStreaming; return this; }
        IMenuVideoThumbnail IMenuVideoParseMode.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuVideoThumbnail IMenuVideoParseMode.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuVideoThumbnail IMenuVideoParseMode.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuVideoParseMode.Width(int width) { Width = width; return this; }
        ////
        IMenuVideoReplyToMessage IMenuVideoReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoReplyToMessage IMenuVideoReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuVideoSupportsStreaming IMenuVideoReplyMarkup.SupportsStreaming(bool supportsStreaming) { SupportsStreaming = supportsStreaming; return this; }
        IMenuVideoThumbnail IMenuVideoReplyMarkup.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuVideoThumbnail IMenuVideoReplyMarkup.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuVideoThumbnail IMenuVideoReplyMarkup.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuVideoReplyMarkup.Width(int width) { Width = width; return this; }
        ////
        IMenuVideoSupportsStreaming IMenuVideoReplyToMessage.SupportsStreaming(bool supportsStreaming) { SupportsStreaming = supportsStreaming; return this; }
        IMenuVideoThumbnail IMenuVideoReplyToMessage.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuVideoThumbnail IMenuVideoReplyToMessage.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuVideoThumbnail IMenuVideoReplyToMessage.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuVideoReplyToMessage.Width(int width) { Width = width; return this; }
        ////
        IMenuVideoThumbnail IMenuVideoSupportsStreaming.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuVideoThumbnail IMenuVideoSupportsStreaming.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuVideoThumbnail IMenuVideoSupportsStreaming.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuVideoSupportsStreaming.Width(int width) { Width = width; return this; }
        ////
        IMenuItem IMenuVideoThumbnail.Width(int width) { Width = width; return this; }
        #endregion
        #region Keyboard Implementation
        IKeyboardBuilder<IMenuVideoReplyMarkup> IReplyMarkupable<IMenuVideoReplyMarkup>.ReplyMarkup() => this;

        IMenuVideoReplyMarkup IReplyMarkupable<IMenuVideoReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuVideoReplyMarkup IReplyMarkupable<IMenuVideoReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuVideoReplyMarkup IReplyMarkupable<IMenuVideoReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuVideoReplyMarkup IReplyMarkupable<IMenuVideoReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuVideoReplyMarkup IKeyboardBuilder<IMenuVideoReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuVideoReplyMarkup IKeyboardBuilder<IMenuVideoReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateReply(CommandService.UpdateKeyboardRows(keyboard.ReplyRows));
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuVideoReplyMarkup IKeyboardBuilder<IMenuVideoReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuVideoReplyMarkup IKeyboardBuilder<IMenuVideoReplyMarkup>.ForceReply(bool selective)
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
