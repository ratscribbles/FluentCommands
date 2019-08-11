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

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuVoiceBuilder, IMenuVoiceOptionalBuilder,
        IMenuVoiceCancellationToken, IMenuVoiceCaption, IMenuVoiceDisableNotification, IMenuVoiceDuration, IMenuVoiceParseMode
    {
        #region Required
        IMenuVoiceOptionalBuilder IMenuVoiceBuilder.Source(string source) { Source = source; return this; }
        IMenuVoiceOptionalBuilder IMenuVoiceBuilder.Source(Stream content) { Source = content; return this; }
        IMenuVoiceOptionalBuilder IMenuVoiceBuilder.Source(Stream content, string fileName) { Source = new InputOnlineFile(content, fileName); return this; }
        IMenuVoiceOptionalBuilder IMenuVoiceBuilder.Source(InputOnlineFile voice) { Source = voice; return this; }
        #endregion
        #region Optional
        IMenuVoiceCancellationToken IMenuVoiceOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuVoiceCaption IMenuVoiceOptionalBuilder.Caption(string caption) { Caption = caption; return this; }
        IMenuVoiceDisableNotification IMenuVoiceOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVoiceDuration IMenuVoiceOptionalBuilder.Duration(int duration) { Duration = duration; return this; }
        IMenuVoiceParseMode IMenuVoiceOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuVoiceOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVoiceOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuVoiceCaption IMenuVoiceCancellationToken.Caption(string caption) { Caption = caption; return this; }
        IMenuVoiceDisableNotification IMenuVoiceCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVoiceDuration IMenuVoiceCancellationToken.Duration(int duration) { Duration = duration; return this; }
        IMenuVoiceParseMode IMenuVoiceCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuVoiceCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVoiceCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuVoiceDisableNotification IMenuVoiceCaption.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVoiceDuration IMenuVoiceCaption.Duration(int duration) { Duration = duration; return this; }
        IMenuVoiceParseMode IMenuVoiceCaption.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuVoiceCaption.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVoiceCaption.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuVoiceDuration IMenuVoiceDisableNotification.Duration(int duration) { Duration = duration; return this; }
        IMenuVoiceParseMode IMenuVoiceDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuVoiceDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVoiceDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuVoiceParseMode IMenuVoiceDuration.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuVoiceDuration.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVoiceDuration.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuItem IMenuVoiceParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVoiceParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
    }
}
