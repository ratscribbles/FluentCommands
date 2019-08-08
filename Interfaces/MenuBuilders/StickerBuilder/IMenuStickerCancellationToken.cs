using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.StickerBuilder
{
    public interface IMenuStickerCancellationToken : IFluentInterface, IMenuItem
    {
        IMenuStickerDisableNotification DisableNotification(bool disableNotification);
        IMenuItem ReplyToMessage(Message message);
        IMenuItem ReplyToMessage(int messageId);
    }
}
