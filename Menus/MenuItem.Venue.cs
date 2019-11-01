using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.VenueBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuVenueBuilder, IMenuVenueBuilderLatitude, IMenuVenueBuilderLongitude, IMenuVenueBuilderTitle, IMenuVenueOptionalBuilder,
        IMenuVenueCancellationToken, IMenuVenueDisableNotification, IMenuVenueFourSquareId, IMenuVenueFourSquareType,
        IMenuVenueReplyMarkup, IKeyboardBuilder<IMenuVenueReplyMarkup>
    {
        #region Required
        IMenuVenueBuilderLatitude IMenuVenueBuilder.Latitude(float latitude) { Latitude = latitude; return this; }
        IMenuVenueBuilderLongitude IMenuVenueBuilderLatitude.Longitude(float longitude) { Longitude = longitude; return this; }
        IMenuVenueBuilderTitle IMenuVenueBuilderLongitude.Title(string title) { Title = title; return this; }
        IMenuVenueOptionalBuilder IMenuVenueBuilderTitle.Address(string address) { Address = address; return this; }
        #endregion
        #region Required
        IMenuVenueCancellationToken IMenuVenueOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuVenueDisableNotification IMenuVenueOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVenueFourSquareId IMenuVenueOptionalBuilder.FourSquareId(string fourSquareId) { FourSquareId = fourSquareId; return this; }
        IMenuVenueFourSquareType IMenuVenueOptionalBuilder.FourSquareType(string fourSquareType) { FourSquareType = fourSquareType; return this; }
        IMenuItem IMenuVenueOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVenueOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuVenueDisableNotification IMenuVenueCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuVenueFourSquareId IMenuVenueCancellationToken.FourSquareId(string fourSquareId) { FourSquareId = fourSquareId; return this; }
        IMenuVenueFourSquareType IMenuVenueCancellationToken.FourSquareType(string fourSquareType) { FourSquareType = fourSquareType; return this; }
        IMenuItem IMenuVenueCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVenueCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuVenueFourSquareId IMenuVenueDisableNotification.FourSquareId(string fourSquareId) { FourSquareId = fourSquareId; return this; }
        IMenuVenueFourSquareType IMenuVenueDisableNotification.FourSquareType(string fourSquareType) { FourSquareType = fourSquareType; return this; }
        IMenuItem IMenuVenueDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVenueDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuVenueFourSquareType IMenuVenueFourSquareId.FourSquareType(string fourSquareType) { FourSquareType = fourSquareType; return this; }
        IMenuItem IMenuVenueFourSquareId.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVenueFourSquareId.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuItem IMenuVenueFourSquareType.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVenueFourSquareType.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuItem IMenuVenueReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuVenueReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Keyboard Implementation
        IKeyboardBuilder<IMenuVenueReplyMarkup> IReplyMarkupable<IMenuVenueReplyMarkup>.ReplyMarkup() => this;

        IMenuVenueReplyMarkup IReplyMarkupable<IMenuVenueReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuVenueReplyMarkup IReplyMarkupable<IMenuVenueReplyMarkup>.ReplyMarkup(ReplyKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuVenueReplyMarkup IReplyMarkupable<IMenuVenueReplyMarkup>.ReplyMarkup(ForceReplyMarkup markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuVenueReplyMarkup IReplyMarkupable<IMenuVenueReplyMarkup>.ReplyMarkup(ReplyKeyboardRemove markup, bool selective) { ReplyMarkup = markup; return this; }

        IMenuVenueReplyMarkup IKeyboardBuilder<IMenuVenueReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }

        IMenuVenueReplyMarkup IKeyboardBuilder<IMenuVenueReplyMarkup>.Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateReply(CommandService.UpdateKeyboardRows(keyboard.ReplyRows));
            ReplyMarkup = new ReplyKeyboardMarkup(keyboard.ReplyRows);
            return this;
        }

        IMenuVenueReplyMarkup IKeyboardBuilder<IMenuVenueReplyMarkup>.Remove(bool selective)
        {
            var keyboard = new ReplyKeyboardRemove
            {
                Selective = selective
            };
            ReplyMarkup = keyboard;
            return this;
        }

        IMenuVenueReplyMarkup IKeyboardBuilder<IMenuVenueReplyMarkup>.ForceReply(bool selective)
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
