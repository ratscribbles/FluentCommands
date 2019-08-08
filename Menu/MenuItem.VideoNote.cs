using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.VideoNoteBuilder;

namespace FluentCommands.Menu
{
    public partial class MenuItem : IMenuVideoNoteBuilder, IMenuVideoNoteOptionalBuilder,
        IMenuVideoNoteCancellationToken, IMenuVideoNoteDisableNotification, IMenuVideoNoteDuration, IMenuVideoNoteLength, IMenuVideoNoteReplyToMessage
    {
        #region Required
        IMenuVideoNoteOptionalBuilder IMenuVideoNoteBuilder.Source(string fileId) { SourceVideoNote = fileId; return this; }
        IMenuVideoNoteOptionalBuilder IMenuVideoNoteBuilder.Source(Stream content) { SourceVideoNote = content; return this; }
        IMenuVideoNoteOptionalBuilder IMenuVideoNoteBuilder.Source(Stream content, string fileName) { SourceVideoNote = new InputTelegramFile(content, fileName); return this; }
        IMenuVideoNoteOptionalBuilder IMenuVideoNoteBuilder.Source(InputTelegramFile videoNote) { SourceVideoNote = videoNote; return this; }
        #endregion
        #region Optional
        IMenuVideoNoteCancellationToken IMenuVideoNoteOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuVideoNoteDisableNotification IMenuVideoNoteOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVideoNoteDuration IMenuVideoNoteOptionalBuilder.Duration(int duration) { Duration = duration; return this; }
        IMenuVideoNoteLength IMenuVideoNoteOptionalBuilder.Length(int length) { Length = length; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuItem IMenuVideoNoteOptionalBuilder.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuVideoNoteOptionalBuilder.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuVideoNoteOptionalBuilder.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        #endregion
        #region Additional Implementation
        IMenuVideoNoteDisableNotification IMenuVideoNoteCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVideoNoteDuration IMenuVideoNoteCancellationToken.Duration(int duration) { Duration = duration; return this; }
        IMenuVideoNoteLength IMenuVideoNoteCancellationToken.Length(int length) { Length = length; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuItem IMenuVideoNoteCancellationToken.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuVideoNoteCancellationToken.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuVideoNoteCancellationToken.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuVideoNoteDuration IMenuVideoNoteDisableNotification.Duration(int duration) { Duration = duration; return this; }
        IMenuVideoNoteLength IMenuVideoNoteDisableNotification.Length(int length) { Length = length; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuItem IMenuVideoNoteDisableNotification.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuVideoNoteDisableNotification.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuVideoNoteDisableNotification.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuVideoNoteLength IMenuVideoNoteDuration.Length(int length) { Length = length; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteDuration.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteDuration.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuItem IMenuVideoNoteDuration.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuVideoNoteDuration.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuVideoNoteDuration.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuVideoNoteReplyToMessage IMenuVideoNoteLength.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteLength.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        IMenuItem IMenuVideoNoteLength.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuVideoNoteLength.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuVideoNoteLength.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuItem IMenuVideoNoteReplyToMessage.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuItem IMenuVideoNoteReplyToMessage.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuItem IMenuVideoNoteReplyToMessage.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        #endregion
    }
}
