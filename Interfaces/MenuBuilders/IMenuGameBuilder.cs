using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menu;
using FluentCommands.Interfaces.MenuBuilders.GameBuilder;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuGameBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the "Short Name" for this game.
        /// </summary>
        /// <param name="shortName">Serves as the unique identifier for the game. Setup your games with the Botfather.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuGameOptionalBuilder ShortName(string shortName);
    }
}
