using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.VideoBuilder
{
    public interface IMenuVideoCancellationToken : IFluentInterface, IMenuItem
    {
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
