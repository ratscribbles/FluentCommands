using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.ContactBuilder;

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuContactBuilder, IMenuContactBuilderPhoneNumber, IMenuContactOptionalBuilder,
        IMenuContactCancellationToken, IMenuContactDisableNotification, IMenuContactLastName, IMenuContactReplyToMessage, IMenuContactThumbnail
    {
        #region Required
        IMenuContactBuilderPhoneNumber IMenuContactBuilder.PhoneNumber(string phoneNumber) { PhoneNumber = phoneNumber; return this; }
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
        IMenuItem IMenuContactOptionalBuilder.VCard(string vCard) { VCard = vCard; return this; }
        #endregion

        #region Additional Implementation
        IMenuContactDisableNotification IMenuContactCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuContactLastName IMenuContactCancellationToken.LastName(string lastName) { LastName = lastName; return this; }
        IMenuContactReplyToMessage IMenuContactCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuContactReplyToMessage IMenuContactCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuContactThumbnail IMenuContactCancellationToken.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuContactThumbnail IMenuContactCancellationToken.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuContactThumbnail IMenuContactCancellationToken.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuContactCancellationToken.VCard(string vCard) { VCard = vCard; return this; }
        ////
        IMenuContactLastName IMenuContactDisableNotification.LastName(string lastName) { LastName = lastName; return this; }
        IMenuContactReplyToMessage IMenuContactDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuContactReplyToMessage IMenuContactDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuContactThumbnail IMenuContactDisableNotification.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuContactThumbnail IMenuContactDisableNotification.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuContactThumbnail IMenuContactDisableNotification.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuContactDisableNotification.VCard(string vCard) { VCard = vCard; return this; }
        ////
        IMenuContactReplyToMessage IMenuContactLastName.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuContactReplyToMessage IMenuContactLastName.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuContactThumbnail IMenuContactLastName.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuContactThumbnail IMenuContactLastName.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuContactThumbnail IMenuContactLastName.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuContactLastName.VCard(string vCard) { VCard = vCard; return this; }
        ////
        IMenuContactThumbnail IMenuContactReplyToMessage.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuContactThumbnail IMenuContactReplyToMessage.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuContactThumbnail IMenuContactReplyToMessage.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuContactReplyToMessage.VCard(string vCard) { VCard = vCard; return this; }
        ////
        IMenuItem IMenuContactThumbnail.VCard(string vCard) { VCard = vCard; return this; }
        #endregion
    }
}
