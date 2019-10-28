using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Telegram.Bot.Types;
using FluentCommands.Interfaces.KeyboardBuilders;

namespace FluentCommands.Interfaces.MenuBuilders.AudioBuilder
{
    public interface IMenuAudioPerformer : IReplyMarkupable<IMenuAudioReplyMarkup>, IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
        /// <para>The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail‘s width and height should not exceed 320. Ignored if the file is not uploaded using multipart/form-data.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAudioThumbnail Thumbnail(string source);
        /// <summary>
        /// Optional. Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
        /// <para>The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail‘s width and height should not exceed 320. Ignored if the file is not uploaded using multipart/form-data.</para>
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAudioThumbnail Thumbnail(Stream content, string fileName);
        /// <summary>
        /// Optional. Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
        /// <para>The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail‘s width and height should not exceed 320. Ignored if the file is not uploaded using multipart/form-data.</para>
        /// </summary>
        /// <param name="thumbnail"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAudioThumbnail Thumbnail(InputMedia thumbnail);
        /// <summary>
        /// Optional. Track name.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem Title(string title);
    }
}
