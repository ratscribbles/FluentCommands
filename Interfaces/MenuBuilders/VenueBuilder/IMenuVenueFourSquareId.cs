﻿using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.VenueBuilder
{
    public interface IMenuVenueFourSquareId : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Foursquare type of the venue, if known.
        /// <para>(For example, “arts_entertainment/default”, “arts_entertainment/aquarium” or “food/icecream”.)</para>
        /// </summary>
        /// <param name="fourSquareType"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVenueFourSquareType FourSquareType(string fourSquareType);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem ReplyToMessage(Message message);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem ReplyToMessage(int messageId);
    }
}
