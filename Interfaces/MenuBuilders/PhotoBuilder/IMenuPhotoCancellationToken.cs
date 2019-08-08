using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.PhotoBuilder
{
    public interface IMenuPhotoCancellationToken : IFluentInterface, IMenuItem
    {
        IMenuPhotoCaption Caption(string caption);
        IMenuPhotoDisableNotification DisableNotification(bool disableNotification);
        IMenuPhotoParseMode ParseMode(ParseMode parseMode);
        IMenuItem ReplyToMessage(Message message);
        IMenuItem ReplyToMessage(int messageId);
    }
}
