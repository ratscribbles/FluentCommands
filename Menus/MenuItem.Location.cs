using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.LocationBuilder;

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuLocationBuilder, IMenuLocationBuilderLatitude, IMenuLocationOptionalBuilder,
        IMenuLocationCancellationToken, IMenuLocationDisableNotification, IMenuLocationLivePeriod
    {
        #region Required
        IMenuLocationBuilderLatitude IMenuLocationBuilder.Latitude(float latitude) { Latitude = latitude; return this; }
        IMenuLocationOptionalBuilder IMenuLocationBuilderLatitude.Longitude(float longitude) { Longitude = longitude; return this; }
        #endregion

        #region Optional
        IMenuLocationCancellationToken IMenuLocationOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuLocationDisableNotification IMenuLocationOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuLocationLivePeriod IMenuLocationOptionalBuilder.LivePeriod(int livePeriod) { LivePeriod = livePeriod; return this; }
        IMenuItem IMenuLocationOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuLocationOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion

        #region Additional Implementation
        IMenuLocationDisableNotification IMenuLocationCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuLocationLivePeriod IMenuLocationCancellationToken.LivePeriod(int livePeriod) { LivePeriod = livePeriod; return this; }
        IMenuItem IMenuLocationCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuLocationCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuLocationLivePeriod IMenuLocationDisableNotification.LivePeriod(int livePeriod) { LivePeriod = livePeriod; return this; }
        IMenuItem IMenuLocationDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuLocationDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuItem IMenuLocationLivePeriod.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuLocationLivePeriod.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
    }
}
