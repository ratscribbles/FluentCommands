using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Commands;

namespace FluentCommands.Commands
{
    [Flags]
    public enum Permissions
    {
        None = 0,
        CanAddWebPagePreviews = 1,
        CanChangeInfo = 2,
        CanInviteUsers = 4,
        CanPinMessages = 8,
        CanSendMediaMessages = 16,
        CanSendMessages = 32,
        CanSendOtherMessages = 64,
        CanSendPolls = 128,
        Administrator = 256
    }
    /// <summary>
    /// Flags <see cref="Command"/> methods or an entire <see cref="CommandModule{TCommand}"/> to check for specific <see cref="Telegram.Bot.Types.User"/> permissions before executing command inputs.
    /// <para>Please use bitwise OR (the | operator) to check multiple permissions.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PermissionsAttribute : Attribute
    {
        internal Permissions Permissions { get; private set; } = Permissions.None;

        /// <summary>
        /// Sets the permissions required to execute this <see cref="Command"/>.
        /// <para>Please use bitwise OR (the | operator) to check multiple permissions.</para>
        /// </summary>
        /// <param name="permissions"></param>
        public PermissionsAttribute(Permissions permissions) => Permissions = permissions;
    }
}
