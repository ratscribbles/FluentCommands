﻿using FluentCommands.Interfaces.KeyboardBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.VenueBuilder
{
    public interface IMenuVenueFourSquareId : IReplyMarkupable<IMenuVenueReplyMarkup>, IFluentInterface, ISendableMenu
    {
        /// <summary>
        /// Optional. Foursquare type of the venue, if known.
        /// <para>(For example, “arts_entertainment/default”, “arts_entertainment/aquarium” or “food/icecream”.)</para>
        /// </summary>
        /// <param name="fourSquareType"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuVenueFourSquareType FourSquareType(string fourSquareType);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        Menus.Menu ReplyToMessage(Message message);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        Menus.Menu ReplyToMessage(int messageId);
    }
}
