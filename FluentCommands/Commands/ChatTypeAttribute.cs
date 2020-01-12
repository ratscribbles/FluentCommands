using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Commands
{
    /// <summary></summary>
    [Flags]
    public enum TelegramChatType
    {
        Private = 0,
        Group = 1,
        Channel = 2,
        Supergroup = 4
    }
    /// <summary>
    /// Flags <see cref="Command"/> methods or an entire <see cref="CommandModule{TCommand}"/> to check for specific <see cref="Telegram.Bot.Types.User"/> permissions before executing command inputs.
    /// <para>Please use bitwise OR (the | operator) to check multiple permissions.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ChatTypeAttribute : Attribute
    {
        internal ChatType ChatType { get; } = ChatType.

        public ChatTypeAttribute(ChatType chatType) => ChatType = chatType;
    }
}
