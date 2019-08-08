using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.VideoBuilder
{
    public interface IMenuVideoOptionalBuilder : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. The <see cref="System.Threading.CancellationToken"/> for this <see cref="MenuItem"/>.
        /// <para>Provides the Token responsible for notifying when the <see cref="MenuItem"/> should cancel its send operation.</para>
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoCancellationToken CancellationToken(CancellationToken token);
        IMenuVideoCaption Caption(string caption);
        IMenuVideoDisableNotification DisableNotification(bool disableNotification);
        IMenuVideoDuration Duration(int duration);
        IMenuVideoHeight Height(int height);
        IMenuVideoParseMode ParseMode(ParseMode parseMode);
        IMenuVideoReplyToMessage ReplyToMessage(Message message);
        IMenuVideoReplyToMessage ReplyToMessage(int messageId);
        IMenuVideoSupportsStreaming SupportsStreaming(bool supportsStreaming);
        IMenuVideoThumbnail Thumbnail(string source);
        IMenuVideoThumbnail Thumbnail(Stream content, string fileName);
        IMenuVideoThumbnail Thumbnail(InputMedia thumbnail);
        IMenuItem Width(int width);
    }
}
