using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using FluentCommands.Menu;
using FluentCommands.Interfaces.MenuBuilders.MediaGroupBuilder;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuMediaGroupBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. Provides all files to go inside this album.
        /// <para>Each file must be a photo or video.</para>
        /// </summary>
        /// <param name="media">The album to be sent.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuMediaGroupOptionalBuilder Source(IEnumerable<IAlbumInputMedia> media);
        /// <summary>
        /// Required. Provides all files to go inside this album.
        /// <para>Each file must be a photo or video.</para>
        /// </summary>
        /// <param name="media">The album to be sent.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuMediaGroupOptionalBuilder Source(params IAlbumInputMedia[] media);
    }
}
