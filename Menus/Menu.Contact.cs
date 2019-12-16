using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.ContactBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Menus
{
    public partial class Menu : IMenuContactBuilderPhoneNumber, IMenuContactOptionalBuilder,
        IMenuContactCancellationToken, IMenuContactDisableNotification, IMenuContactLastName, IMenuContactReplyToMessage, IMenuContactThumbnail,
        IMenuContactReplyMarkup, IKeyboardBuilder<IMenuContactReplyMarkup>
    {
        #region Required
        IMenuContactOptionalBuilder IMenuContactBuilderPhoneNumber.FirstName(string firstName) { FirstName = firstName; return this; }
        #endregion

        #region Optional
        IMenuContactCancellationToken IMenuContactOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuContactDisableNotification IMenuContactOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuContactLastName IMenuContactOptionalBuilder.LastName(string lastName) { LastName = lastName; return this; }
        IMenuContactReplyToMessage IMenuContactOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuContactReplyToMessage IMenuContactOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuContactThumbnail IMenuContactOptionalBuilder.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuContactThumbnail IMenuContactOptionalBuilder.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuContactThumbnail IMenuContactOptionalBuilder.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenu IMenuContactOptionalBuilder.VCard(string vCard) { VCard = vCard; return this; }
        #endregion

        #region Additional Implementation
        IMenuContactDisableNotification IMenuContactCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuContactLastName IMenuContactCancellationToken.LastName(string lastName) { LastName = lastName; return this; }
        IMenuContactReplyToMessage IMenuContactCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuContactReplyToMessage IMenuContactCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuContactThumbnail IMenuContactCancellationToken.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuContactThumbnail IMenuContactCancellationToken.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuContactThumbnail IMenuContactCancellationToken.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenu IMenuContactCancellationToken.VCard(string vCard) { VCard = vCard; return this; }
        ////
        IMenuContactLastName IMenuContactDisableNotification.LastName(string lastName) { LastName = lastName; return this; }
        IMenuContactReplyToMessage IMenuContactDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuContactReplyToMessage IMenuContactDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuContactThumbnail IMenuContactDisableNotification.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuContactThumbnail IMenuContactDisableNotification.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuContactThumbnail IMenuContactDisableNotification.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenu IMenuContactDisableNotification.VCard(string vCard) { VCard = vCard; return this; }
        ////
        IMenuContactReplyToMessage IMenuContactLastName.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuContactReplyToMessage IMenuContactLastName.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuContactThumbnail IMenuContactLastName.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuContactThumbnail IMenuContactLastName.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuContactThumbnail IMenuContactLastName.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenu IMenuContactLastName.VCard(string vCard) { VCard = vCard; return this; }
        ////
        IMenuContactReplyToMessage IMenuContactReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuContactReplyToMessage IMenuContactReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuContactThumbnail IMenuContactReplyMarkup.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuContactThumbnail IMenuContactReplyMarkup.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuContactThumbnail IMenuContactReplyMarkup.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenu IMenuContactReplyMarkup.VCard(string vCard) { VCard = vCard; return this; }
        ////
        IMenuContactThumbnail IMenuContactReplyToMessage.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuContactThumbnail IMenuContactReplyToMessage.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuContactThumbnail IMenuContactReplyToMessage.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenu IMenuContactReplyToMessage.VCard(string vCard) { VCard = vCard; return this; }
        ////
        IMenu IMenuContactThumbnail.VCard(string vCard) { VCard = vCard; return this; }
        #endregion

        #region Keyboard Implementation
        IKeyboardBuilder<IMenuContactReplyMarkup> IReplyMarkupable<IMenuContactReplyMarkup>.ReplyMarkup() => this;

        IMenuContactReplyMarkup IReplyMarkupable<IMenuContactReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuContactReplyMarkup IReplyMarkupable<IMenuContactReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuContactReplyMarkup IReplyMarkupable<IMenuContactReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuContactReplyMarkup IReplyMarkupable<IMenuContactReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuContactReplyMarkup IKeyboardBuilder<IMenuContactReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);    
            return this;
        }

        IMenuContactReplyMarkup IKeyboardBuilder<IMenuContactReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateReply(CommandService.UpdateKeyboardRows(keyboard.ReplyRows));
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuContactReplyMarkup IKeyboardBuilder<IMenuContactReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuContactReplyMarkup IKeyboardBuilder<IMenuContactReplyMarkup>.ForceReply(bool selective)
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
