using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.VideoBuilder
{
    public interface IMenuVideoParseMode : IFluentInterface, IMenuItem
    {
        IMenuVideoReplyToMessage ReplyToMessage(Message message);
        IMenuVideoReplyToMessage ReplyToMessage(int messageId);
        IMenuVideoSupportsStreaming SupportsStreaming(bool supportsStreaming);
        IMenuVideoThumbnail Thumbnail(string source);
        IMenuVideoThumbnail Thumbnail(Stream content, string fileName);
        IMenuVideoThumbnail Thumbnail(InputMedia thumbnail);
        IMenuItem Width(int width);
    }
}
