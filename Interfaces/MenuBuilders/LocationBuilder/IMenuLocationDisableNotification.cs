using FluentCommands.Interfaces.KeyboardBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.LocationBuilder
{
    public interface IMenuLocationDisableNotification : IReplyMarkupable<IMenuLocationReplyMarkup>, IFluentInterface, ISendableMenu
    {
        /// <summary>
        /// Optional. Period in seconds for which the location will be updated (see Live Locations, should be between 60 and 86400.)
        /// </summary>
        /// <param name="livePeriod"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuLocationLivePeriod LivePeriod(int livePeriod);
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
