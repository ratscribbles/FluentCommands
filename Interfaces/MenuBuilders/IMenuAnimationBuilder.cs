using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menus;
using FluentCommands.Interfaces.MenuBuilders.AnimationBuilder;
using Telegram.Bot.Types.InputFiles;
using System.IO;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuAnimationBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="source">The source URL for the file of this <see cref="MenuItem"/>.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAnimationOptionalBuilder Source(string source);
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="MenuItem"/>.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAnimationOptionalBuilder Source(Stream content);
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="MenuItem"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAnimationOptionalBuilder Source(Stream content, string fileName);
        /// <summary>
        /// Required. Provides the source file of this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="animation">The source file of this <see cref="MenuItem"/>.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAnimationOptionalBuilder Source(InputOnlineFile animation);
    }
}
