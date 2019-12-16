﻿using FluentCommands.Interfaces.KeyboardBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.VoiceBuilder
{
    public interface IMenuVoiceOptionalBuilder : IReplyMarkupable<IMenuVoiceReplyMarkup>, IFluentInterface, IMenu
    {
        /// <summary>
        /// Optional. The <see cref="System.Threading.CancellationToken"/> for this <see cref="MenuItem"/>.
        /// <para>Provides the Token responsible for notifying when the <see cref="MenuItem"/> should cancel its send operation.</para>
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuVoiceCancellationToken CancellationToken(CancellationToken token);
        /// <summary>
        /// Optional. Voice message caption, 0-1024 characters.
        /// </summary>
        /// <param name="caption"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuVoiceCaption Caption(string caption);
        /// <summary>
        /// Optional. Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        /// <param name="disableNotification"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuVoiceDisableNotification DisableNotification(bool disableNotification);
        /// <summary>
        /// Optional. Duration of the voice message in seconds.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuVoiceDuration Duration(int duration);
        /// <summary>
        /// Optional. Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in the media caption.
        /// </summary>
        /// <param name="parseMode"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuVoiceParseMode ParseMode(ParseMode parseMode);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenu ReplyToMessage(Message message);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenu ReplyToMessage(int messageId);
    }
}
