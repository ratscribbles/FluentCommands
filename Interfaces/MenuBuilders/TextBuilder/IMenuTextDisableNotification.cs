﻿using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.TextBuilder
{
    public interface IMenuTextDisableNotification : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Disables link previews for links in the sent message.
        /// </summary>
        /// <param name="disableWebPagePreview"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuTextDisableWebPagePreview DisableWebPagePreview(bool disableWebPagePreview);
        /// <summary>
        /// Optional. Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in the media caption.
        /// </summary>
        /// <param name="parseMode"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuTextParseMode ParseMode(ParseMode parseMode);
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
