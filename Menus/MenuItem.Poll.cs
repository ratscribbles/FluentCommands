using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.PollBuilder;

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuPollBuilder, IMenuPollBuilderQuestion, IMenuPollOptionalBuilder,
        IMenuPollCancellationToken, IMenuPollDisableNotification
    {
        #region Required
        IMenuPollBuilderQuestion IMenuPollBuilder.Question(string question) { Question = question; return this; }
        IMenuPollOptionalBuilder IMenuPollBuilderQuestion.Options(params string[] options) { Options = options; return this; }
        IMenuPollOptionalBuilder IMenuPollBuilderQuestion.Options(IEnumerable<string> options) { Options = options; return this; }
        #endregion
        #region Optional
        IMenuPollCancellationToken IMenuPollOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuPollDisableNotification IMenuPollOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuItem IMenuPollOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuPollOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
        #region Additional Implementation
        IMenuPollDisableNotification IMenuPollCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuItem IMenuPollCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuPollCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuItem IMenuPollDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuPollDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion
    }
}
