﻿using FluentCommands.Interfaces.KeyboardBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.VoiceBuilder
{
    public interface IMenuVoiceCaption : IReplyMarkupable<IMenuVoiceReplyMarkup>, IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        /// <param name="disableNotification"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVoiceDisableNotification DisableNotification(bool disableNotification);
        /// <summary>
        /// Optional. Duration of the voice message in seconds.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVoiceDuration Duration(int duration);
        /// <summary>
        /// Optional. Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in the media caption.
        /// </summary>
        /// <param name="parseMode"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVoiceParseMode ParseMode(ParseMode parseMode);
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
