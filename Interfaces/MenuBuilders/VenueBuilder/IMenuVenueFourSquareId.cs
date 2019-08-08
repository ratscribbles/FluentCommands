using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.VenueBuilder
{
    public interface IMenuVenueFourSquareId : IFluentInterface, IMenuItem
    {
        IMenuVenueFourSquareType FourSquareType(string fourSquareType);
        IMenuItem ReplyToMessage(Message message);
        IMenuItem ReplyToMessage(int messageId);
    }
}
