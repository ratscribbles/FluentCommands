using FluentCommands.Interfaces.KeyboardBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.PollBuilder
{
    public interface IMenuPollOptionalBuilder : IReplyMarkupable<IMenuPollReplyMarkup>, IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. The <see cref="System.Threading.CancellationToken"/> for this <see cref="MenuItem"/>.
        /// <para>Provides the Token responsible for notifying when the <see cref="MenuItem"/> should cancel its send operation.</para>
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPollCancellationToken CancellationToken(CancellationToken token);
        /// <summary>
        /// Optional. Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        /// <param name="disableNotification"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPollDisableNotification DisableNotification(bool disableNotification);
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
