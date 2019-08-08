using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.VideoNoteBuilder
{
    public interface IMenuVideoNoteOptionalBuilder : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. The <see cref="System.Threading.CancellationToken"/> for this <see cref="MenuItem"/>.
        /// <para>Provides the Token responsible for notifying when the <see cref="MenuItem"/> should cancel its send operation.</para>
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoNoteCancellationToken CancellationToken(CancellationToken token);
        IMenuVideoNoteDisableNotification DisableNotification(bool disableNotification);
        IMenuVideoNoteDuration Duration(int duration);
        IMenuVideoNoteLength Length(int length);
        IMenuVideoNoteReplyToMessage ReplyToMessage(Message message);
        IMenuVideoNoteReplyToMessage ReplyToMessage(int messageId);
        IMenuItem Thumbnail(string source);
        IMenuItem Thumbnail(Stream content, string fileName);
        IMenuItem Thumbnail(InputMedia thumbnail);
    }
}
