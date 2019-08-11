using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.PhotoBuilder;

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuPhotoBuilder, IMenuPhotoOptionalBuilder,
        IMenuPhotoCancellationToken, IMenuPhotoCaption, IMenuPhotoDisableNotification, IMenuPhotoParseMode
    {
        #region Required
        IMenuPhotoOptionalBuilder IMenuPhotoBuilder.Source(string source) { Source = source; return this; }
        IMenuPhotoOptionalBuilder IMenuPhotoBuilder.Source(Stream content) { Source = content; return this; }
        IMenuPhotoOptionalBuilder IMenuPhotoBuilder.Source(Stream content, string fileName) { Source = new InputOnlineFile(content, fileName); return this; }
        IMenuPhotoOptionalBuilder IMenuPhotoBuilder.Source(InputOnlineFile audio) { Source = audio; return this; }
        #endregion
        #region Optional
        IMenuPhotoCancellationToken IMenuPhotoOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuPhotoCaption IMenuPhotoOptionalBuilder.Caption(string caption) { Caption = caption; return this; }
        IMenuPhotoDisableNotification IMenuPhotoOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuPhotoParseMode IMenuPhotoOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuPhotoOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this;  }
        IMenuItem IMenuPhotoOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuPhotoCaption IMenuPhotoCancellationToken.Caption(string caption) { Caption = caption; return this; }
        IMenuPhotoDisableNotification IMenuPhotoCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuPhotoParseMode IMenuPhotoCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuPhotoCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this;  }
        IMenuItem IMenuPhotoCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this;  }
        ////
        IMenuPhotoDisableNotification IMenuPhotoCaption.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuPhotoParseMode IMenuPhotoCaption.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuPhotoCaption.ReplyToMessage(Message message) { ReplyToMessage = message; return this;  }
        IMenuItem IMenuPhotoCaption.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuPhotoParseMode IMenuPhotoDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuItem IMenuPhotoDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this;  }
        IMenuItem IMenuPhotoDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuItem IMenuPhotoParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuPhotoParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
    }
}
