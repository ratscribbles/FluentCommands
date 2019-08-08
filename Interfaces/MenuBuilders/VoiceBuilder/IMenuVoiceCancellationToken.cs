using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.VoiceBuilder
{
    public interface IMenuVoiceCancellationToken : IFluentInterface, IMenuItem
    {
        IMenuVoiceCaption Caption(string caption);
        IMenuVoiceDisableNotification DisableNotification(bool disableNotification);
        IMenuVoiceDuration Duration(int duration);
        IMenuVoiceParseMode ParseMode(ParseMode parseMode);
        IMenuItem ReplyToMessage(Message message);
        IMenuItem ReplyToMessage(int messageId);
    }
}
