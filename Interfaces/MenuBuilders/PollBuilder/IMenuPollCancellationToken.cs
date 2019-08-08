using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.PollBuilder
{
    public interface IMenuPollCancellationToken : IFluentInterface, IMenuItem
    {
        IMenuPollDisableNotification DisableNotification(bool disableNotification);
        IMenuItem ReplyToMessage(Message message);
        IMenuItem ReplyToMessage(int messageId);
    }
}
