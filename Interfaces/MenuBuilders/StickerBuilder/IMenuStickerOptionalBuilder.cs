using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.StickerBuilder
{
    public interface IMenuStickerOptionalBuilder : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. The <see cref="System.Threading.CancellationToken"/> for this <see cref="MenuItem"/>.
        /// <para>Provides the Token responsible for notifying when the <see cref="MenuItem"/> should cancel its send operation.</para>
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuStickerCancellationToken CancellationToken(CancellationToken token);
        IMenuStickerDisableNotification DisableNotification(bool disableNotification);
        IMenuItem ReplyToMessage(Message message);
        IMenuItem ReplyToMessage(int messageId);
    }
}
