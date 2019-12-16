using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menus;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using System.Threading.Tasks;

namespace FluentCommands.Interfaces.MenuBuilders
{
    /// <summary>
    /// Represents a valid <see cref="Menu"/> object that can be sent to a <see cref="Chat"/> or <see cref="User"/> on Telegram.
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// Allows you specify where to send this <see cref="Menu"/>.
        /// <para>If the bot hasn't interacted with this <see cref="Chat"/> or <see cref="User"/>, it will throw a <see cref="ChatNotInitiatedException"/>.</para>
        /// <para>Exceptions thrown by <see cref="Menu"/> objects can be suppressed with the <see cref="CommandServiceConfig.SwallowCriticalExceptions"/> property (set to true).</para>
        /// </summary>
        /// <returns>Returns this completed <see cref="Menu"/> as a <see cref="MenuItem"/> object.</returns>
        /// <param name="idToSendTo">The userId to send this <see cref="Menu"/> to.</param>
        /// <param name="chatAction">The <see cref="ChatAction"/> to send along with the <see cref="Menu"/>.</param>
        /// <param name="duration">The duration (in milliseconds) to wait before sending the <see cref="Menu"/>, with the <see cref="ChatAction"/>.</param>
        /// <exception cref="ChatNotFoundException"></exception>
        /// <exception cref="ChatNotInitiatedException"></exception>
        /// <exception cref="ContactRequestException"></exception>
        /// <exception cref="InvalidUserIdException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        Task Send(int idToSendTo, ChatAction? chatAction = null, int duration = 0);

        /// <summary>
        /// Allows you specify where to send this <see cref="Menu"/>.
        /// <para>If the bot hasn't interacted with this <see cref="Chat"/> or <see cref="User"/>, it will throw a <see cref="ChatNotInitiatedException"/>.</para>
        /// <para>Exceptions thrown by <see cref="Menu"/> objects can be suppressed with the <see cref="CommandServiceConfig.SwallowCriticalExceptions"/> property (set to true).</para>
        /// </summary>
        /// <returns>Returns this completed <see cref="Menu"/> as a <see cref="MenuItem"/> object.</returns>
        /// <param name="idToSendTo">The userId to send this <see cref="Menu"/> to.</param>
        /// <param name="chatAction">The <see cref="ChatAction"/> to send along with the <see cref="Menu"/>.</param>
        /// <param name="duration">The duration (in milliseconds) to wait before sending the <see cref="Menu"/>, with the <see cref="ChatAction"/>.</param>
        /// <exception cref="ChatNotFoundException"></exception>
        /// <exception cref="ChatNotInitiatedException"></exception>
        /// <exception cref="ContactRequestException"></exception>
        /// <exception cref="InvalidUserIdException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        Task Send(long idToSendTo, ChatAction? chatAction = null, int duration = 0);
    }
}
