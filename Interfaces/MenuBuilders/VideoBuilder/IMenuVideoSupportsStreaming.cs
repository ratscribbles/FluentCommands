using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.VideoBuilder
{
    public interface IMenuVideoSupportsStreaming : IFluentInterface, IMenuItem
    {
        IMenuVideoThumbnail Thumbnail(string source);
        IMenuVideoThumbnail Thumbnail(Stream content, string fileName);
        IMenuVideoThumbnail Thumbnail(InputMedia thumbnail);
        IMenuItem Width(int width);
    }
}
