using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces;

namespace FluentCommands.Menus
{
    /// <summary>
    /// Finalized <see cref="Menus.MenuItem"/> ready to send to Telegram.
    /// </summary>
    public class Menu : IFluentInterface
    {
        internal MenuItem MenuItem { get; private set; }

        /// <summary>
        /// This class cannot be instantiated directly. Please use <see cref="MenuItem.As()"/> or <see cref="MenuItem.WithChatAction(Telegram.Bot.Types.Enums.ChatAction)"/> static builder methods.
        /// </summary>
        /// <param name="m"></param>
        internal Menu(MenuItem m) => MenuItem = m;
    }
}
