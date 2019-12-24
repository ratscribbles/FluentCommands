using FluentCommands.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace FluentCommands.Extensions
{
    public static class TelegramEventArgsExtensions
    {
        //: descriptions.
        public static bool TryGetArgs(this CallbackQueryEventArgs e, [NotNull] out IReadOnlyList<string> args)
        {
            var match = FluentRegex.CheckCommand.Match(e?.CallbackQuery?.Data ?? "");
            if (match.Success) { args = match.Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries); return true; }
            else { args = Array.Empty<string>(); return false; }
        }

        //: NEEDS TO BE TESTED
        public static bool TryGetArgs(this ChosenInlineResultEventArgs e, [NotNull] out IReadOnlyList<string> args)
        {
            var match = FluentRegex.CheckCommand.Match(e?.ChosenInlineResult?.Query ?? "");
            if (match.Success) { args = match.Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries); return true; }
            else { args = Array.Empty<string>(); return false; }
        }
        //: ALSO NEEDS TO BE TESTED. CREATE A NEW REGEX TO DETECT QUERIES
        public static bool TryGetArgs(this InlineQueryEventArgs e, [NotNull] out IReadOnlyList<string> args)
        {
            var match = FluentRegex.CheckCommand.Match(e?.InlineQuery?.Query ?? "");
            if (match.Success) { args = match.Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries); return true; }
            else { args = Array.Empty<string>(); return false; }
        }
        public static bool TryGetArgs(this MessageEventArgs e, [NotNull] out IReadOnlyList<string> args)
        {
            var match = FluentRegex.CheckCommand.Match(e?.Message?.Text ?? "");
            if (match.Success) { args = match.Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries); return true; }
            else { args = Array.Empty<string>(); return false; }
        }
        public static bool TryGetArgs(this UpdateEventArgs e, [NotNull] out IReadOnlyList<string> args)
        {
            bool result;
            switch (e.Update)
            {
                case var u when e is { Update: { CallbackQuery: { Data: { } } } }:
                    result = u.CallbackQuery.Data.TryGetArgs_Internal(out args);
                    return result;
                case var u when e is { Update: { ChosenInlineResult: { Query: { } } } }:
                    result = u.ChosenInlineResult.Query.TryGetArgs_Internal(out args);
                    return result;
                case var u when e is { Update: { InlineQuery: { Query: { } } } }:
                    result = u.InlineQuery.Query.TryGetArgs_Internal(out args);
                    return result;
                case var u when e is { Update: { Message: { Text: { } } } }:
                    result = u.Message.Text.TryGetArgs_Internal(out args);
                    return result;
                default:
                    args = Array.Empty<string>();
                    return false;
            }
        }
        private static bool TryGetArgs_Internal(this string data, out IReadOnlyList<string> args)
        {
            var match = FluentRegex.CheckCommand.Match(data);
            if (match.Success) { args = match.Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries); return true; }
            else { args = Array.Empty<string>(); return false; }
        }
        /// <summary>Returns the raw string input for <see cref="Command"/> processing. Returns empty string if not found.</summary>
        internal static ReadOnlyMemory<char> GetRawInput(this CallbackQueryEventArgs e) => e?.CallbackQuery?.Data.AsMemory() ?? ReadOnlyMemory<char>.Empty;
        /// <summary>Returns the raw string input for <see cref="Command"/> processing. Returns empty string if not found.</summary>
        internal static ReadOnlyMemory<char> GetRawInput(this ChosenInlineResultEventArgs e) => e?.ChosenInlineResult?.Query.AsMemory() ?? ReadOnlyMemory<char>.Empty;
        /// <summary>Returns the raw string input for <see cref="Command"/> processing. Returns empty string if not found.</summary>
        internal static ReadOnlyMemory<char> GetRawInput(this InlineQueryEventArgs e) => e?.InlineQuery?.Query.AsMemory() ?? ReadOnlyMemory<char>.Empty;
        /// <summary>Returns the raw string input for <see cref="Command"/> processing. Returns empty string if not found.</summary>
        internal static ReadOnlyMemory<char> GetRawInput(this MessageEventArgs e) => e?.Message?.Text.AsMemory() ?? ReadOnlyMemory<char>.Empty;
        /// <summary>Returns the raw string input for <see cref="Command"/> processing. Returns empty string if not found.</summary>
        internal static ReadOnlyMemory<char> GetRawInput(this UpdateEventArgs e)
        {
            var update = e?.Update;

            return update switch
            {
                { CallbackQuery: { } } => update.CallbackQuery.Data.AsMemory(),
                { ChosenInlineResult: { } } => update.ChosenInlineResult.Query.AsMemory(),
                { InlineQuery: { } } => update.InlineQuery.Query.AsMemory(),
                { Message: { } } => update.Message.Text.AsMemory(),
                null => ReadOnlyMemory<char>.Empty,
                _ => ReadOnlyMemory<char>.Empty
            };
        }

        internal static User? GetUser(this CallbackQueryEventArgs? e) => e?.CallbackQuery?.From;
        internal static User? GetUser(this ChosenInlineResultEventArgs? e) => e?.ChosenInlineResult?.From;
        internal static User? GetUser(this InlineQueryEventArgs? e) => e?.InlineQuery?.From;
        internal static User? GetUser(this MessageEventArgs? e) => e?.Message?.From;
        internal static User? GetUser(this UpdateEventArgs? e)
        {
            var update = e?.Update;

            return update switch
            {
                { CallbackQuery: { } } => update.CallbackQuery.From,
                { ChosenInlineResult: { } } => update.ChosenInlineResult.From,
                { InlineQuery: { } } => update.InlineQuery.From,
                { Message: { } } => update.Message.From,
                null => null,
                _ => null,
            };
        }

        internal static Chat? GetChat(this CallbackQueryEventArgs? e) => e?.CallbackQuery?.Message?.Chat;
        internal static Chat? GetChat(this MessageEventArgs? e) => e?.Message?.Chat;
        internal static Chat? GetChat(this UpdateEventArgs? e)
        {
            var update = e?.Update;

            return update switch
            {
                { CallbackQuery: { } } => update.CallbackQuery?.Message?.Chat,
                { Message: { } } => update.Message.Chat,
                null => null,
                _ => null,
            };
        }

        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found. Returns the sender (User id) for ChosenInlineResult and InlineQuery EventArgs.</summary>
        internal static long GetChatId(this CallbackQueryEventArgs e) => e?.CallbackQuery?.Message?.Chat?.Id ?? 0;
        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found. Returns the sender (User id) for ChosenInlineResult and InlineQuery EventArgs.</summary>
        internal static long GetChatId(this ChosenInlineResultEventArgs e) { if (!e?.ChosenInlineResult?.From?.IsBot ?? false) return e?.ChosenInlineResult?.From?.Id ?? 0; else return 0; }
        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found. Returns the sender (User id) for ChosenInlineResult and InlineQuery EventArgs.</summary>
        internal static long GetChatId(this InlineQueryEventArgs e) { if (!e?.InlineQuery?.From?.IsBot ?? false) return e?.InlineQuery?.From?.Id ?? 0; else return 0; }
        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found. Returns the sender (User id) for ChosenInlineResult and InlineQuery EventArgs.</summary>
        internal static long GetChatId(this MessageEventArgs e) => e?.Message?.Chat?.Id ?? 0;
        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found. Returns the sender (User id) for ChosenInlineResult and InlineQuery EventArgs.</summary>
        internal static long GetChatId(this UpdateEventArgs e)
        {
            var update = e?.Update;

            return update switch
            {
                { CallbackQuery: { } } => update.CallbackQuery.Message?.Chat?.Id ?? 0,
                { ChosenInlineResult: { } } => update.ChosenInlineResult.From?.Id ?? 0,
                { InlineQuery: { } } => update.InlineQuery.From?.Id ?? 0,
                { Message: { } } => update.Message.Chat?.Id ?? 0,
                null => 0,
                _ => 0,
            };
        }

        /// <summary>Returns the Chat Id. Returns 0 if not found, or if a bot (this bot) is the sender.</summary>
        internal static int GetUserId(this CallbackQueryEventArgs e) { if (!e?.CallbackQuery?.From?.IsBot ?? false) return e?.CallbackQuery?.From?.Id ?? 0; else return 0; }
        /// <summary>Returns the Chat Id. Returns 0 if not found, or if a bot (this bot) is the sender.</summary>
        internal static int GetUserId(this ChosenInlineResultEventArgs e) { if (!e?.ChosenInlineResult?.From?.IsBot ?? false) return e?.ChosenInlineResult?.From?.Id ?? 0; else return 0; }
        /// <summary>Returns the Chat Id. Returns 0 if not found, or if a bot (this bot) is the sender.</summary>
        internal static int GetUserId(this InlineQueryEventArgs e) { if (!e?.InlineQuery?.From?.IsBot ?? false) return e?.InlineQuery?.From?.Id ?? 0; else return 0; }
        /// <summary>Returns the Chat Id. Returns 0 if not found, or if a bot (this bot) is the sender.</summary>
        internal static int GetUserId(this MessageEventArgs e) { if (!e?.Message?.From?.IsBot ?? false) return e?.Message?.From?.Id ?? 0; else return 0; }
        /// <summary>Returns the Chat Id. Returns 0 if not found, or if a bot (this bot) is the sender.</summary>
        internal static int GetUserId(this UpdateEventArgs e)
        {
            var update = e?.Update;

            return update switch
            {
                { CallbackQuery: { From: { IsBot: false } } } => update.CallbackQuery.From.Id,
                { ChosenInlineResult: { From: { IsBot: false } } } => update.ChosenInlineResult.From.Id,
                { InlineQuery: { From: { IsBot: false } } } => update.InlineQuery.From.Id,
                { Message: { From: { IsBot: false } } } => update.Message.From.Id,
                null => 0,
                _ => 0
            };
        }

        internal static Message? GetMessage(this CallbackQueryEventArgs e) => e?.CallbackQuery?.Message;
        internal static Message? GetMessage(this MessageEventArgs e) => e?.Message;
        internal static Message? GetMessage(this UpdateEventArgs e)
        {
            var update = e?.Update;

            return update switch
            {
                { CallbackQuery: { Message: { } } } => update.CallbackQuery.Message,
                { Message: { } } => update.Message,
                _ => null
            };
        }
    }
}
