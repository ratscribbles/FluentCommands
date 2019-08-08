using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.MenuBuilders.VoiceBuilder
{
    public interface IMenuVoiceOptionalBuilder : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. The <see cref="System.Threading.CancellationToken"/> for this <see cref="MenuItem"/>.
        /// <para>Provides the Token responsible for notifying when the <see cref="MenuItem"/> should cancel its send operation.</para>
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVoiceCancellationToken CancellationToken(CancellationToken token);
        IMenuVoiceCaption Caption(string caption);
        IMenuVoiceDisableNotification DisableNotification(bool disableNotification);
        IMenuVoiceDuration Duration(int duration);
        IMenuVoiceParseMode ParseMode(ParseMode parseMode);
        IMenuItem ReplyToMessage(Message message);
        IMenuItem ReplyToMessage(int messageId);
    }
}
