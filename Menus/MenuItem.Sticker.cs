using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.StickerBuilder;

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuStickerBuilder, IMenuStickerOptionalBuilder,
        IMenuStickerCancellationToken, IMenuStickerDisableNotification
    {
        #region Required
        IMenuStickerOptionalBuilder IMenuStickerBuilder.Source(string source) { Source = source; return this; }
        IMenuStickerOptionalBuilder IMenuStickerBuilder.Source(Stream content) { Source = content; return this; }
        IMenuStickerOptionalBuilder IMenuStickerBuilder.Source(Stream content, string fileName) { Source = new InputOnlineFile(content, fileName); return this; }
        IMenuStickerOptionalBuilder IMenuStickerBuilder.Source(InputOnlineFile sticker) { Source = sticker; return this; }
        #endregion
        #region Optional
        IMenuStickerCancellationToken IMenuStickerOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuStickerDisableNotification IMenuStickerOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuItem IMenuStickerOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuStickerOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuStickerDisableNotification IMenuStickerCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuItem IMenuStickerCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuStickerCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuItem IMenuStickerDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuStickerDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
    }
}
