using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.MediaGroupBuilder;

namespace FluentCommands.Menu
{
    public partial class MenuItem : IMenuMediaGroupBuilder, IMenuMediaGroupOptionalBuilder, IMenuMediaGroupDisableNotification
    {
        #region Required
        IMenuMediaGroupOptionalBuilder IMenuMediaGroupBuilder.Source(IEnumerable<IAlbumInputMedia> media) { Media = media; return this; }
        IMenuMediaGroupOptionalBuilder IMenuMediaGroupBuilder.Source(params IAlbumInputMedia[] media) { Media = media; return this; }
        #endregion
        #region Optional
        IMenuMediaGroupDisableNotification IMenuMediaGroupOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuItem IMenuMediaGroupOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuMediaGroupOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuItem IMenuMediaGroupDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuMediaGroupDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
    }
}
