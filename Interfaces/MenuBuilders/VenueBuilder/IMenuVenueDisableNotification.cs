using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.VenueBuilder
{
    public interface IMenuVenueDisableNotification : IFluentInterface, IMenuItem
    {
        IMenuVenueFourSquareId FourSquareId(string fourSquareId);
        IMenuVenueFourSquareType FourSquareType(string fourSquareType);
        IMenuItem ReplyToMessage(Message message);
        IMenuItem ReplyToMessage(int messageId);
    }
}
