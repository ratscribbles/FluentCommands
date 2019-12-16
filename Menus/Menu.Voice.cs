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
using FluentCommands.Interfaces.MenuBuilders.VoiceBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Menus
{
    public partial class Menu : IMenuVoiceOptionalBuilder,
        IMenuVoiceCancellationToken, IMenuVoiceCaption, IMenuVoiceDisableNotification, IMenuVoiceDuration, IMenuVoiceParseMode,
        IMenuVoiceReplyMarkup, IKeyboardBuilder<IMenuVoiceReplyMarkup>  
    {
        #region Optional
        IMenuVoiceCancellationToken IMenuVoiceOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuVoiceCaption IMenuVoiceOptionalBuilder.Caption(string caption) { Caption = caption; return this; }
        IMenuVoiceDisableNotification IMenuVoiceOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVoiceDuration IMenuVoiceOptionalBuilder.Duration(int duration) { Duration = duration; return this; }
        IMenuVoiceParseMode IMenuVoiceOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenu IMenuVoiceOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuVoiceOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuVoiceCaption IMenuVoiceCancellationToken.Caption(string caption) { Caption = caption; return this; }
        IMenuVoiceDisableNotification IMenuVoiceCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVoiceDuration IMenuVoiceCancellationToken.Duration(int duration) { Duration = duration; return this; }
        IMenuVoiceParseMode IMenuVoiceCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenu IMenuVoiceCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuVoiceCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuVoiceDisableNotification IMenuVoiceCaption.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVoiceDuration IMenuVoiceCaption.Duration(int duration) { Duration = duration; return this; }
        IMenuVoiceParseMode IMenuVoiceCaption.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenu IMenuVoiceCaption.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuVoiceCaption.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuVoiceDuration IMenuVoiceDisableNotification.Duration(int duration) { Duration = duration; return this; }
        IMenuVoiceParseMode IMenuVoiceDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenu IMenuVoiceDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuVoiceDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuVoiceParseMode IMenuVoiceDuration.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenu IMenuVoiceDuration.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuVoiceDuration.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenu IMenuVoiceParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuVoiceParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenu IMenuVoiceReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuVoiceReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Keyboard Implementation
        IKeyboardBuilder<IMenuVoiceReplyMarkup> IReplyMarkupable<IMenuVoiceReplyMarkup>.ReplyMarkup() => this;

        IMenuVoiceReplyMarkup IReplyMarkupable<IMenuVoiceReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuVoiceReplyMarkup IReplyMarkupable<IMenuVoiceReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuVoiceReplyMarkup IReplyMarkupable<IMenuVoiceReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuVoiceReplyMarkup IReplyMarkupable<IMenuVoiceReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuVoiceReplyMarkup IKeyboardBuilder<IMenuVoiceReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuVoiceReplyMarkup IKeyboardBuilder<IMenuVoiceReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateReply(CommandService.UpdateKeyboardRows(keyboard.ReplyRows));
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuVoiceReplyMarkup IKeyboardBuilder<IMenuVoiceReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuVoiceReplyMarkup IKeyboardBuilder<IMenuVoiceReplyMarkup>.ForceReply(bool selective)
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
