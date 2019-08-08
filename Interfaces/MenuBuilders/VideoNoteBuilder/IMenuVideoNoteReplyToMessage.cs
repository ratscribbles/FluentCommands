using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.VideoNoteBuilder
{
    public interface IMenuVideoNoteReplyToMessage : IFluentInterface, IMenuItem
    {
        IMenuItem Thumbnail(string source);
        IMenuItem Thumbnail(Stream content, string fileName);
        IMenuItem Thumbnail(InputMedia thumbnail);
    }
}
