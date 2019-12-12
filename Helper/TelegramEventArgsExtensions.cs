using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace FluentCommands.Helper
{
    public static class TelegramEventArgsExtensions
    {
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
    }
}
