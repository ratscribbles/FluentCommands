using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.TextBuilder;

namespace FluentCommands.Menu
{
    public partial class MenuItem : IMenuTextBuilder, IMenuTextOptionalBuilder,
        IMenuTextCancellationToken, IMenuTextDisableNotification, IMenuTextDisableWebPagePreview, IMenuTextParseMode
    {
        #region Required
        IMenuTextOptionalBuilder IMenuTextBuilder.TextSource(string text) { TextString = text; return this; }
        #endregion
        #region Optional
        IMenuTextCancellationToken IMenuTextOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuTextDisableNotification IMenuTextOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuTextDisableWebPagePreview IMenuTextOptionalBuilder.DisableWebPagePreview(bool disableWebPagePreview) { DisableWebPagePreview = disableWebPagePreview; return this; }
        IMenuTextParseMode IMenuTextOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuTextOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuTextOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuTextDisableNotification IMenuTextCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuTextDisableWebPagePreview IMenuTextCancellationToken.DisableWebPagePreview(bool disableWebPagePreview) { DisableWebPagePreview = disableWebPagePreview; return this; }
        IMenuTextParseMode IMenuTextCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuTextCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuTextCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuTextDisableWebPagePreview IMenuTextDisableNotification.DisableWebPagePreview(bool disableWebPagePreview) { DisableWebPagePreview = disableWebPagePreview; return this; }
        IMenuTextParseMode IMenuTextDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuTextDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuTextDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuTextParseMode IMenuTextDisableWebPagePreview.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuTextDisableWebPagePreview.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuTextDisableWebPagePreview.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuItem IMenuTextParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuTextParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
    }
}
