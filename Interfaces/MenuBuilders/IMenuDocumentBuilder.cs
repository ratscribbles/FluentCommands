using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Telegram.Bot.Types.InputFiles;
using FluentCommands.Menus;
using FluentCommands.Interfaces.MenuBuilders.DocumentBuilder;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuDocumentBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="source">The source URL for this file of this <see cref="MenuItem"/>.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuDocumentOptionalBuilder Source(string source);
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="MenuItem"/>.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuDocumentOptionalBuilder Source(Stream content);
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="MenuItem"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuDocumentOptionalBuilder Source(Stream content, string fileName);
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="document">The source file of this <see cref="MenuItem"/>.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuDocumentOptionalBuilder Source(InputOnlineFile document);
    }
}
