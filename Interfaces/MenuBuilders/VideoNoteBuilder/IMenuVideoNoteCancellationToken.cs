using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.VideoNoteBuilder
{
    public interface IMenuVideoNoteCancellationToken : IFluentInterface, IMenuItem
    {
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
