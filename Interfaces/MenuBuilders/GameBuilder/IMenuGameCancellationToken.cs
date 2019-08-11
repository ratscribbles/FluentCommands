using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.GameBuilder
{
    public interface IMenuGameCancellationToken : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        /// <param name="disableNotification"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuGameDisableNotification DisableNotification(bool disableNotification);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem ReplyToMessage(Message message);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem ReplyToMessage(int messageId);
    }
}
