using FluentCommands.Interfaces.KeyboardBuilders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.VideoBuilder
{
    public interface IMenuVideoCancellationToken : IReplyMarkupable<IMenuVideoReplyMarkup>, IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Video caption (may also be used when resending videos by file_id), 0-1024 characters.
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        IMenuVideoCaption Caption(string caption);
        /// <summary>
        /// Optional. Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        /// <param name="disableNotification"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoDisableNotification DisableNotification(bool disableNotification);
        /// <summary>
        /// Optional. Duration of sent video in seconds.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoDuration Duration(int duration);
        /// <summary>
        /// Optional. Video height.
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        IMenuVideoHeight Height(int height);
        /// <summary>
        /// Optional. Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in the media caption.
        /// </summary>
        /// <param name="parseMode"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoParseMode ParseMode(ParseMode parseMode);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoReplyToMessage ReplyToMessage(Message message);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoReplyToMessage ReplyToMessage(int messageId);
        /// <summary>
        /// Optional. Pass <c>true</c> if the uploaded video is suitable for streaming.
        /// </summary>
        /// <param name="supportsStreaming"></param>
        /// <returns></returns>
        IMenuVideoSupportsStreaming SupportsStreaming(bool supportsStreaming);
        /// <summary>
        /// Optional. Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
        /// <para>The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail‘s width and height should not exceed 320. Ignored if the file is not uploaded using multipart/form-data.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoThumbnail Thumbnail(string source);
        /// <summary>
        /// Optional. Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
        /// <para>The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail‘s width and height should not exceed 320. Ignored if the file is not uploaded using multipart/form-data.</para>
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoThumbnail Thumbnail(Stream content, string fileName);
        /// <summary>
        /// Optional. Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
        /// <para>The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail‘s width and height should not exceed 320. Ignored if the file is not uploaded using multipart/form-data.</para>
        /// </summary>
        /// <param name="thumbnail"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoThumbnail Thumbnail(InputMedia thumbnail);
        /// <summary>
        /// Optional. Video width.
        /// </summary>
        /// <param name="width"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem Width(int width);
    }
}
