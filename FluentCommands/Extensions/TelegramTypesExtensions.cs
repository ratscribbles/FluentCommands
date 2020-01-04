using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Extensions
{
    internal static class TelegramTypesExtensions
    {
        internal static string ToFluentLogger(this User u)
            => u is { }
                ? $"User: {(!string.IsNullOrWhiteSpace(u.Username) ? $"@{u.Username}" : "(Username hidden)")}, ID: {u.Id} (\"{u.FirstName}{(!string.IsNullOrWhiteSpace(u.LastName) ? $" {u.LastName}" : "")}\")"
                : "User: Null (not found)";

        internal static string ToFluentLogger(this Chat c)
        {
            return c is { }
                ? $"Chat: {(!string.IsNullOrWhiteSpace(c.Username) ? $"@{c.Username}" : (!string.IsNullOrWhiteSpace(c.Title) ? c.Title : "(Chatname not found)"))}, ID: {c.Id}"
                : "Chat: Null (not found)";
        }
    }
}
