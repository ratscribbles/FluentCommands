using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.TextBuilder
{
    public interface IMenuTextOptionalBuilder : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. The <see cref="System.Threading.CancellationToken"/> for this <see cref="MenuItem"/>.
        /// <para>Provides the Token responsible for notifying when the <see cref="MenuItem"/> should cancel its send operation.</para>
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuTextCancellationToken CancellationToken(CancellationToken token);
        /// <summary>
        /// Optional. Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        /// <param name="disableNotification"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuTextDisableNotification DisableNotification(bool disableNotification);
        /// <summary>
        /// Optional. Disables link previews for links in the sent message.
        /// </summary>
        /// <param name="disableWebPagePreview"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuTextDisableWebPagePreview DisableWebPagePreview(bool disableWebPagePreview);
        /// <summary>
        /// Optional. Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in the media caption.
        /// </summary>
        /// <param name="parseMode"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuTextParseMode ParseMode(ParseMode parseMode);
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
