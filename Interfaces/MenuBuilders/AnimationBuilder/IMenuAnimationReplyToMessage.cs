using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using System.IO;

namespace FluentCommands.Interfaces.MenuBuilders.AnimationBuilder
{
    public interface IMenuAnimationReplyToMessage : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
        /// <para>The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail‘s width and height should not exceed 320. Ignored if the file is not uploaded using multipart/form-data.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAnimationThumbnail Thumbnail(string source);
        /// <summary>
        /// Optional. Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
        /// <para>The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail‘s width and height should not exceed 320. Ignored if the file is not uploaded using multipart/form-data.</para>
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAnimationThumbnail Thumbnail(Stream content, string fileName);
        /// <summary>
        /// Optional. Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
        /// <para>The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail‘s width and height should not exceed 320. Ignored if the file is not uploaded using multipart/form-data.</para>
        /// </summary>
        /// <param name="thumbnail"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAnimationThumbnail Thumbnail(InputMedia thumbnail);
        /// <summary>
        /// Optional. Animation width.
        /// </summary>
        /// <param name="width"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem Width(int width);
    }
}
