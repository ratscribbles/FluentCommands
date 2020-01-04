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
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Commands;

namespace FluentCommands.Menus
{
    public partial class Menu : IMenuVideoNoteOptionalBuilder,
        IMenuVideoNoteCancellationToken, IMenuVideoNoteDisableNotification, IMenuVideoNoteDuration, IMenuVideoNoteLength, IMenuVideoNoteReplyToMessage,
        IMenuVideoNoteReplyMarkup, IKeyboardBuilder<IMenuVideoNoteReplyMarkup>
    {
        #region Optional
        IMenuVideoNoteCancellationToken IMenuVideoNoteOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuVideoNoteDisableNotification IMenuVideoNoteOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVideoNoteDuration IMenuVideoNoteOptionalBuilder.Duration(int duration) { Duration = duration; return this; }
        IMenuVideoNoteLength IMenuVideoNoteOptionalBuilder.Length(int length) { Length = length; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menu IMenuVideoNoteOptionalBuilder.Thumbnail(string source) { Thumbnail = source; return this; }
        Menu IMenuVideoNoteOptionalBuilder.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menu IMenuVideoNoteOptionalBuilder.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        #endregion
        #region Additional Implementation
        IMenuVideoNoteDisableNotification IMenuVideoNoteCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVideoNoteDuration IMenuVideoNoteCancellationToken.Duration(int duration) { Duration = duration; return this; }
        IMenuVideoNoteLength IMenuVideoNoteCancellationToken.Length(int length) { Length = length; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menu IMenuVideoNoteCancellationToken.Thumbnail(string source) { Thumbnail = source; return this; }
        Menu IMenuVideoNoteCancellationToken.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menu IMenuVideoNoteCancellationToken.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuVideoNoteDuration IMenuVideoNoteDisableNotification.Duration(int duration) { Duration = duration; return this; }
        IMenuVideoNoteLength IMenuVideoNoteDisableNotification.Length(int length) { Length = length; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menu IMenuVideoNoteDisableNotification.Thumbnail(string source) { Thumbnail = source; return this; }
        Menu IMenuVideoNoteDisableNotification.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menu IMenuVideoNoteDisableNotification.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuVideoNoteLength IMenuVideoNoteDuration.Length(int length) { Length = length; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteDuration.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteDuration.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menu IMenuVideoNoteDuration.Thumbnail(string source) { Thumbnail = source; return this; }
        Menu IMenuVideoNoteDuration.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menu IMenuVideoNoteDuration.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuVideoNoteReplyToMessage IMenuVideoNoteLength.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteLength.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menu IMenuVideoNoteLength.Thumbnail(string source) { Thumbnail = source; return this; }
        Menu IMenuVideoNoteLength.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menu IMenuVideoNoteLength.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        IMenuVideoNoteReplyToMessage IMenuVideoNoteReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuVideoNoteReplyToMessage IMenuVideoNoteReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        Menu IMenuVideoNoteReplyMarkup.Thumbnail(string source) { Thumbnail = source; return this; }
        Menu IMenuVideoNoteReplyMarkup.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menu IMenuVideoNoteReplyMarkup.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        ////
        Menu IMenuVideoNoteReplyToMessage.Thumbnail(string source) { Thumbnail = source; return this; }
        Menu IMenuVideoNoteReplyToMessage.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        Menu IMenuVideoNoteReplyToMessage.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        #endregion
        #region Keyboard Implementation
        IKeyboardBuilder<IMenuVideoNoteReplyMarkup> IReplyMarkupable<IMenuVideoNoteReplyMarkup>.ReplyMarkup() => this;

        IMenuVideoNoteReplyMarkup IReplyMarkupable<IMenuVideoNoteReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuVideoNoteReplyMarkup IReplyMarkupable<IMenuVideoNoteReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuVideoNoteReplyMarkup IReplyMarkupable<IMenuVideoNoteReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuVideoNoteReplyMarkup IReplyMarkupable<IMenuVideoNoteReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuVideoNoteReplyMarkup IKeyboardBuilder<IMenuVideoNoteReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline();
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuVideoNoteReplyMarkup IKeyboardBuilder<IMenuVideoNoteReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuVideoNoteReplyMarkup IKeyboardBuilder<IMenuVideoNoteReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuVideoNoteReplyMarkup IKeyboardBuilder<IMenuVideoNoteReplyMarkup>.ForceReply(bool selective)
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
