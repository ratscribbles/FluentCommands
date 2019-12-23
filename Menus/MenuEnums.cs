using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace FluentCommands.Menus
{
    public enum MenuMode
    {
        /// <summary>The default <see cref="MenuMode"/>. Treats <see cref="Menu"/> objects like normal message requests.</summary>
        NoAction,
        /// <summary>Attempts to modify the last message sent by this <see cref="TelegramBotClient"/>. 
        /// <para>On failure, sends this menu as normal with the previous message still present.</para>
        /// <para><strong>Does not work with MediaGroup messages.</strong></para></summary>
        EditLastMessage,
        /// <summary>Attempts to modiy the last message sent by this <see cref="TelegramBotClient"/> with the contents of this <see cref="Menu"/>. 
        /// <para>On failure, attempts to delete the last message sent. If that fails, sends the <see cref="Menu"/> as normal with the previous message still present.</para></summary>
        EditOrDeleteLastMessage,
    }
    internal enum MenuType
    {
        /// <summary>Default.</summary>
        None,
        Animation,
        Audio,
        Contact,
        Document,
        Game,
        Invoice,
        Location,
        MediaGroup,
        Photo,
        Poll,
        Sticker,
        Text,
        Venue,
        Video,
        VideoNote,
        Voice
    }
}
