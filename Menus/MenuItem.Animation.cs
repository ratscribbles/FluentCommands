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
using FluentCommands.Interfaces.MenuBuilders.AnimationBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuAnimationBuilder, IMenuAnimationOptionalBuilder,
        IMenuAnimationCancellationToken, IMenuAnimationCaption, IMenuAnimationDisableNotification, IMenuAnimationDuration, IMenuAnimationHeight, IMenuAnimationParseMode, IMenuAnimationReplyToMessage, IMenuAnimationThumbnail,
        IMenuAnimationReplyMarkup, IKeyboardBuilder<IMenuAnimationReplyMarkup>
    {
        #region Required
        IMenuAnimationOptionalBuilder IMenuAnimationBuilder.Source(string source) { Source = new InputOnlineFile(source); return this; }
        IMenuAnimationOptionalBuilder IMenuAnimationBuilder.Source(Stream content) { Source = new InputOnlineFile(content); return this; }
        IMenuAnimationOptionalBuilder IMenuAnimationBuilder.Source(Stream content, string fileName) { Source = new InputOnlineFile(content, fileName); return this; }
        IMenuAnimationOptionalBuilder IMenuAnimationBuilder.Source(InputOnlineFile animation) { Source = animation; return this; }
        #endregion

        #region Optional
        IMenuAnimationCancellationToken IMenuAnimationOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuAnimationCaption IMenuAnimationOptionalBuilder.Caption(string caption) { Caption = caption; return this; }
        IMenuAnimationDisableNotification IMenuAnimationOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuAnimationDuration IMenuAnimationOptionalBuilder.Duration(int duration) { Duration = duration; return this; }
        IMenuAnimationHeight IMenuAnimationOptionalBuilder.Height(int height) { Height = height; return this; }
        IMenuAnimationParseMode IMenuAnimationOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuAnimationThumbnail IMenuAnimationOptionalBuilder.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAnimationThumbnail IMenuAnimationOptionalBuilder.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAnimationThumbnail IMenuAnimationOptionalBuilder.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAnimationOptionalBuilder.Width(int width) { Width = width; return this; }
        #endregion

        #region Additional Implementations
        IMenuAnimationCaption IMenuAnimationCancellationToken.Caption(string caption) { Caption = caption; return this; }
        IMenuAnimationDisableNotification IMenuAnimationCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuAnimationDuration IMenuAnimationCancellationToken.Duration(int duration) { Duration = duration; return this; }
        IMenuAnimationHeight IMenuAnimationCancellationToken.Height(int height) { Height = height; return this; }
        IMenuAnimationParseMode IMenuAnimationCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuAnimationThumbnail IMenuAnimationCancellationToken.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAnimationThumbnail IMenuAnimationCancellationToken.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAnimationThumbnail IMenuAnimationCancellationToken.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAnimationCancellationToken.Width(int width) { Width = width; return this; }
        ////
        IMenuAnimationDisableNotification IMenuAnimationCaption.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuAnimationDuration IMenuAnimationCaption.Duration(int duration) { Duration = duration; return this; }
        IMenuAnimationHeight IMenuAnimationCaption.Height(int height) { Height = height; return this; }
        IMenuAnimationParseMode IMenuAnimationCaption.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationCaption.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationCaption.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuAnimationThumbnail IMenuAnimationCaption.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAnimationThumbnail IMenuAnimationCaption.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAnimationThumbnail IMenuAnimationCaption.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAnimationCaption.Width(int width) { Width = width; return this; }
        ////
        IMenuAnimationDuration IMenuAnimationDisableNotification.Duration(int duration) { Duration = duration; return this; }
        IMenuAnimationHeight IMenuAnimationDisableNotification.Height(int height) { Height = height; return this; }
        IMenuAnimationParseMode IMenuAnimationDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuAnimationThumbnail IMenuAnimationDisableNotification.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAnimationThumbnail IMenuAnimationDisableNotification.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAnimationThumbnail IMenuAnimationDisableNotification.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAnimationDisableNotification.Width(int width) { Width = width; return this; }
        ////
        IMenuAnimationHeight IMenuAnimationDuration.Height(int height) { Height = height; return this; }
        IMenuAnimationParseMode IMenuAnimationDuration.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationDuration.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationDuration.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuAnimationThumbnail IMenuAnimationDuration.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAnimationThumbnail IMenuAnimationDuration.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAnimationThumbnail IMenuAnimationDuration.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAnimationDuration.Width(int width) { Width = width; return this; }
        ////
        IMenuAnimationParseMode IMenuAnimationHeight.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationHeight.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationHeight.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuAnimationThumbnail IMenuAnimationHeight.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAnimationThumbnail IMenuAnimationHeight.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAnimationThumbnail IMenuAnimationHeight.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAnimationHeight.Width(int width) { Width = width; return this; }
        ////
        IMenuAnimationReplyToMessage IMenuAnimationParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuAnimationThumbnail IMenuAnimationParseMode.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAnimationThumbnail IMenuAnimationParseMode.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAnimationThumbnail IMenuAnimationParseMode.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAnimationParseMode.Width(int width) { Width = width; return this; }
        ////
        IMenuAnimationReplyToMessage IMenuAnimationReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuAnimationReplyToMessage IMenuAnimationReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuAnimationThumbnail IMenuAnimationReplyMarkup.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAnimationThumbnail IMenuAnimationReplyMarkup.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAnimationThumbnail IMenuAnimationReplyMarkup.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAnimationReplyMarkup.Width(int width) { Width = width; return this; }
        ///
        IMenuAnimationThumbnail IMenuAnimationReplyToMessage.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAnimationThumbnail IMenuAnimationReplyToMessage.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAnimationThumbnail IMenuAnimationReplyToMessage.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAnimationReplyToMessage.Width(int width) { Width = width; return this; }
        ////
        IMenuItem IMenuAnimationThumbnail.Width(int width) { Width = width; return this; }
        #endregion

        #region Keyboard Implementation
        IKeyboardBuilder<IMenuAnimationReplyMarkup> IReplyMarkupable<IMenuAnimationReplyMarkup>.ReplyMarkup() => this;

        IMenuAnimationReplyMarkup IReplyMarkupable<IMenuAnimationReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuAnimationReplyMarkup IReplyMarkupable<IMenuAnimationReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuAnimationReplyMarkup IReplyMarkupable<IMenuAnimationReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuAnimationReplyMarkup IReplyMarkupable<IMenuAnimationReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuAnimationReplyMarkup IKeyboardBuilder<IMenuAnimationReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuAnimationReplyMarkup IKeyboardBuilder<IMenuAnimationReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateReply(CommandService.UpdateKeyboardRows(keyboard.ReplyRows));
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuAnimationReplyMarkup IKeyboardBuilder<IMenuAnimationReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuAnimationReplyMarkup IKeyboardBuilder<IMenuAnimationReplyMarkup>.ForceReply(bool selective)
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
