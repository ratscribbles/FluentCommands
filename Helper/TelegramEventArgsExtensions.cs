using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Args;

namespace FluentCommands.Helper
{
    internal static class TelegramEventArgsExtensions
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
            if (update is null) return ReadOnlyMemory<char>.Empty;
            else if (update.CallbackQuery != null) return update.CallbackQuery?.Data.AsMemory() ?? ReadOnlyMemory<char>.Empty;
            else if (update.ChosenInlineResult != null) return update.ChosenInlineResult?.Query.AsMemory() ?? ReadOnlyMemory<char>.Empty;
            else if (update.InlineQuery != null) return update.InlineQuery?.Query.AsMemory() ?? ReadOnlyMemory<char>.Empty;
            else if (update.Message != null) return update.Message?.Text.AsMemory() ?? ReadOnlyMemory<char>.Empty;
            else return ReadOnlyMemory<char>.Empty;
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
            if (update is null) return 0;
            else if (update.CallbackQuery != null) return update.CallbackQuery?.Message?.Chat?.Id ?? 0;
            else if (update.ChosenInlineResult != null) return update.ChosenInlineResult?.From?.Id ?? 0;
            else if (update.InlineQuery != null) return update.InlineQuery?.From?.Id ?? 0;
            else if (update.Message != null) return update.Message?.Chat?.Id ?? 0;
            else return 0;
        }

        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found, or if a bot (this bot) is the sender.</summary>
        internal static int GetUserId(this CallbackQueryEventArgs e) { if (!e?.CallbackQuery?.From?.IsBot ?? false) return e?.CallbackQuery?.From?.Id ?? 0; else return 0; }
        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found, or if a bot (this bot) is the sender.</summary>
        internal static int GetUserId(this ChosenInlineResultEventArgs e) { if (!e?.ChosenInlineResult?.From?.IsBot ?? false) return e?.ChosenInlineResult?.From?.Id ?? 0; else return 0; }
        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found, or if a bot (this bot) is the sender.</summary>
        internal static int GetUserId(this InlineQueryEventArgs e) { if (!e?.InlineQuery?.From?.IsBot ?? false) return e?.InlineQuery?.From?.Id ?? 0; else return 0; }
        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found, or if a bot (this bot) is the sender.</summary>
        internal static int GetUserId(this MessageEventArgs e) { if (!e?.Message?.From?.IsBot ?? false) return e?.Message?.From?.Id ?? 0; else return 0; }
        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found, or if a bot (this bot) is the sender.</summary>
        internal static int GetUserId(this UpdateEventArgs e)
        {
            var update = e?.Update;
            if (update is null) return 0;
            else if (!(update.CallbackQuery is null))
            {
                { if (!update.CallbackQuery?.From?.IsBot ?? false) return update.CallbackQuery?.From?.Id ?? 0; else return 0; }
            }
            else if (!(update.ChosenInlineResult is null))
            {
                { if (!update.ChosenInlineResult?.From?.IsBot ?? false) return update.ChosenInlineResult?.From?.Id ?? 0; else return 0; }
            }
            else if (!(update.InlineQuery is null))
            {
                { if (!update.InlineQuery?.From?.IsBot ?? false) return update.InlineQuery?.From?.Id ?? 0; else return 0; }
            }
            else if (!(update.Message is null))
            {
                { if (!update.Message?.From?.IsBot ?? false) return update.Message?.From?.Id ?? 0; else return 0; }
            }
            else return 0;
        }
    }
}
