using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Telegram.Bot.Types.InputFiles;
using FluentCommands.Menus;
using FluentCommands.Interfaces.MenuBuilders.PhotoBuilder;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuPhotoBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="source">The source URL for the file of this <see cref="MenuItem"/>.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPhotoOptionalBuilder Source(string source);
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="MenuItem"/>.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPhotoOptionalBuilder Source(Stream content);
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="MenuItem"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPhotoOptionalBuilder Source(Stream content, string fileName);
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="photo">The source file of this <see cref="MenuItem"/>.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPhotoOptionalBuilder Source(InputOnlineFile photo);
    }
}
