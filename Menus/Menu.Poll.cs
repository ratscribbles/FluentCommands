using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.PollBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Menus
{
    public partial class Menu : IMenuPollBuilderQuestion, IMenuPollOptionalBuilder,
        IMenuPollCancellationToken, IMenuPollDisableNotification,
        IMenuPollReplyMarkup, IKeyboardBuilder<IMenuPollReplyMarkup>
    {
        #region Required
        IMenuPollOptionalBuilder IMenuPollBuilderQuestion.Options(params string[] options) { Options = options; return this; }
        IMenuPollOptionalBuilder IMenuPollBuilderQuestion.Options(IEnumerable<string> options) { Options = options; return this; }
        #endregion
        #region Optional
        IMenuPollCancellationToken IMenuPollOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuPollDisableNotification IMenuPollOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenu IMenuPollOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuPollOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuPollDisableNotification IMenuPollCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenu IMenuPollCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuPollCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenu IMenuPollDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuPollDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenu IMenuPollReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenu IMenuPollReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Keyboard Implementation
        IKeyboardBuilder<IMenuPollReplyMarkup> IReplyMarkupable<IMenuPollReplyMarkup>.ReplyMarkup() => this;

        IMenuPollReplyMarkup IReplyMarkupable<IMenuPollReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuPollReplyMarkup IReplyMarkupable<IMenuPollReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuPollReplyMarkup IReplyMarkupable<IMenuPollReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuPollReplyMarkup IReplyMarkupable<IMenuPollReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuPollReplyMarkup IKeyboardBuilder<IMenuPollReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuPollReplyMarkup IKeyboardBuilder<IMenuPollReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateReply(CommandService.UpdateKeyboardRows(keyboard.ReplyRows));
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuPollReplyMarkup IKeyboardBuilder<IMenuPollReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuPollReplyMarkup IKeyboardBuilder<IMenuPollReplyMarkup>.ForceReply(bool selective)
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
