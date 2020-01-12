using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Commands;
using Telegram.Bot.Types;

namespace FluentCommands.Commands
{
    /// <summary>Represents <see cref="User"/> permissions in a Telegram <see cref="Chat"/>.</summary>
    [Flags]
    public enum Permissions
    {
        /// <summary>User has no special permissions.</summary>
        None = 0,
        /// <summary>User is able to add webpage previews on their messages.</summary>
        CanAddWebPagePreviews = 1,
        /// <summary>User can change the <see cref="Chat"/> title, photo and other settings.</summary>
        CanChangeInfo = 2,
        /// <summary>User can invite users to the <see cref="Chat"/>.</summary>
        CanInviteUsers = 4,
        /// <summary>User can pin messages to the <see cref="Chat"/>.</summary>
        CanPinMessages = 8,
        /// <summary>User can send audio, document, photo, video, video note and voice note messages to the <see cref="Chat"/>.</summary>
        CanSendMediaMessages = 16,
        /// <summary>User can send text messages, contacts, locations and venues to the <see cref="Chat"/>.</summary>
        CanSendMessages = 32,
        /// <summary>User can send animations, games, stickers and use inline bots to the <see cref="Chat"/>.</summary>
        CanSendOtherMessages = 64,
        /// <summary>User can send polls to the <see cref="Chat"/>.</summary>
        CanSendPolls = 128,
        /// <summary>User has no special permissions.</summary>
        Administrator = 256
    }
    /// <summary>
    /// Flags a command method to check for specific <see cref="User"/> permissions before executing command inputs.
    /// <para>To set <see cref="Permissions"/> for an entire <see cref="CommandModule{TCommand}"/>, use the <see cref="CommandModule{TCommand}.OnConfiguring(ModuleConfigBuilder)"/> method.</para>
    /// <para><strong>Commands marked with this attribute will override the current module permissions settings. Please use bitwise OR (the | operator) to check multiple permissions.</strong></para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PermissionsAttribute : Attribute
    {
        internal Permissions In_Permissions { get; } = Permissions.None;

        /// <summary>
        /// Flags a command method to check for specific <see cref="User"/> permissions before executing command inputs.
        /// <para>To set <see cref="Permissions"/> for an entire <see cref="CommandModule{TCommand}"/>, use the <see cref="CommandModule{TCommand}.OnConfiguring(ModuleConfigBuilder)"/> method.</para>
        /// <para><strong>Commands marked with this attribute will override the current module permissions settings. Please use bitwise OR (the | operator) to check multiple permissions.</strong></para>
        /// </summary>
        /// <param name="permissions"></param>
        public PermissionsAttribute(Permissions permissions) => In_Permissions = permissions;
    }
}
