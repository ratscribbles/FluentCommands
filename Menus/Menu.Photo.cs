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
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Commands;

namespace FluentCommands.Menus
{
    public partial class Menu : IMenuPhotoOptionalBuilder,
        IMenuPhotoCancellationToken, IMenuPhotoCaption, IMenuPhotoDisableNotification, IMenuPhotoParseMode,
        IMenuPhotoReplyMarkup, IKeyboardBuilder<IMenuPhotoReplyMarkup>
    {
        #region Optional
        IMenuPhotoCancellationToken IMenuPhotoOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuPhotoCaption IMenuPhotoOptionalBuilder.Caption(string caption) { Caption = caption; return this; }
        IMenuPhotoDisableNotification IMenuPhotoOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuPhotoParseMode IMenuPhotoOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        Menu IMenuPhotoOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this;  }
        Menu IMenuPhotoOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuPhotoCaption IMenuPhotoCancellationToken.Caption(string caption) { Caption = caption; return this; }
        IMenuPhotoDisableNotification IMenuPhotoCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuPhotoParseMode IMenuPhotoCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        Menu IMenuPhotoCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this;  }
        Menu IMenuPhotoCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this;  }
        ////
        IMenuPhotoDisableNotification IMenuPhotoCaption.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuPhotoParseMode IMenuPhotoCaption.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        Menu IMenuPhotoCaption.ReplyToMessage(Message message) { ReplyToMessage = message; return this;  }
        Menu IMenuPhotoCaption.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuPhotoParseMode IMenuPhotoDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        Menu IMenuPhotoDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this;  }
        Menu IMenuPhotoDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        Menu IMenuPhotoParseMode.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuPhotoParseMode.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        Menu IMenuPhotoReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuPhotoReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Keyboard Implementation
        IKeyboardBuilder<IMenuPhotoReplyMarkup> IReplyMarkupable<IMenuPhotoReplyMarkup>.ReplyMarkup() => this;

        IMenuPhotoReplyMarkup IReplyMarkupable<IMenuPhotoReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuPhotoReplyMarkup IReplyMarkupable<IMenuPhotoReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuPhotoReplyMarkup IReplyMarkupable<IMenuPhotoReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuPhotoReplyMarkup IReplyMarkupable<IMenuPhotoReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuPhotoReplyMarkup IKeyboardBuilder<IMenuPhotoReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline();
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuPhotoReplyMarkup IKeyboardBuilder<IMenuPhotoReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuPhotoReplyMarkup IKeyboardBuilder<IMenuPhotoReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuPhotoReplyMarkup IKeyboardBuilder<IMenuPhotoReplyMarkup>.ForceReply(bool selective)
        {
            var keyboard = new ForceReplyMarkup
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }
        #endregion
    }
}
