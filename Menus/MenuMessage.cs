using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Menus
{
    /// <summary>
    /// Responsible for coupling <see cref="MenuItem"/> objects with the respective <see cref="Telegram.Bot.Types.Message"/> it generates.
    /// </summary>
    internal class MenuMessage
    {
        private Menu _menu;
        private Message _message;

        internal Menu Menu
        {
            get => _menu;
            private set
            {
                if (value is null) _menu = MenuItem.Empty();
            }
        }
        internal Message Message
        {
            get => _message;
            private set
            {
                if (value is null) _message = new Message();
            }
        }
    }
}
