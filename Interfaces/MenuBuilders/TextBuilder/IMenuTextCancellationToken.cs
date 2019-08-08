using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.TextBuilder
{
    public interface IMenuTextCancellationToken : IFluentInterface, IMenuItem
    {
        IMenuTextDisableNotification DisableNotification(bool disableNotification);
        IMenuTextDisableWebPagePreview DisableWebPagePreview(bool disableWebPagePreview);
        IMenuTextParseMode ParseMode(ParseMode parseMode);
        IMenuItem ReplyToMessage(Message message);
        IMenuItem ReplyToMessage(int messageId);
    }
}
