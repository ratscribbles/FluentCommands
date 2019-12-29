using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.GameBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Commands;

namespace FluentCommands.Menus
{
    public partial class Menu : IMenuGameOptionalBuilder,
        IMenuGameCancellationToken, IMenuGameDisableNotification,
        IMenuGameReplyMarkup, IKeyboardBuilderForceInline<IMenuGameReplyMarkup>
    {
        #region Optional
        IMenuGameCancellationToken IMenuGameOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuGameDisableNotification IMenuGameOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        Menu IMenuGameOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuGameOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion

        #region Additional Implementation
        IMenuGameDisableNotification IMenuGameCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        Menu IMenuGameCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuGameCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        Menu IMenuGameDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuGameDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        Menu IMenuGameReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        Menu IMenuGameReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion

        #region Keyboard Implementation
        IKeyboardBuilderForceInline<IMenuGameReplyMarkup> IReplyMarkupableForceInline<IMenuGameReplyMarkup>.ReplyMarkup() => this;

        IMenuGameReplyMarkup IReplyMarkupableForceInline<IMenuGameReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuGameReplyMarkup IKeyboardBuilderForceInline<IMenuGameReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }
        #endregion
    }
}
