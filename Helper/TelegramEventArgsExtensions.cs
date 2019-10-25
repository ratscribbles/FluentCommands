using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Args;

namespace FluentCommands.Helper
{
    internal static class TelegramEventArgsExtensions
    {
        /// <summary>Returns the raw string input for <see cref="Command"/> processing. Returns empty string if not found.</summary>
        internal static string GetRawInput(this CallbackQueryEventArgs e) => e?.CallbackQuery?.Data ?? "";
        /// <summary>Returns the raw string input for <see cref="Command"/> processing. Returns empty string if not found.</summary>
        internal static string GetRawInput(this ChosenInlineResultEventArgs e) => e?.ChosenInlineResult?.Query ?? "";
        /// <summary>Returns the raw string input for <see cref="Command"/> processing. Returns empty string if not found.</summary>
        internal static string GetRawInput(this InlineQueryEventArgs e) => e?.InlineQuery?.Query ?? "";
        /// <summary>Returns the raw string input for <see cref="Command"/> processing. Returns empty string if not found.</summary>
        internal static string GetRawInput(this MessageEventArgs e) => e?.Message?.Text ?? "";
        /// <summary>Returns the raw string input for <see cref="Command"/> processing. Returns empty string if not found.</summary>
        internal static string GetRawInput(this UpdateEventArgs e)
        {
            var update = e?.Update;
            if (update == null) return "";
            else if (update.CallbackQuery != null) return update.CallbackQuery?.Data ?? "";
            else if (update.ChosenInlineResult != null) return update.ChosenInlineResult?.Query ?? "";
            else if (update.InlineQuery != null) return update.InlineQuery?.Query ?? "";
            else if (update.Message != null) return update.Message?.Text ?? "";
            else return "";
        }

        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found.</summary>
        internal static long GetChatId(this CallbackQueryEventArgs e) => e?.CallbackQuery?.Message?.Chat?.Id ?? 0;
        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found.</summary>
        internal static long GetChatId(this MessageEventArgs e) => e?.Message?.Chat?.Id ?? 0;
        /// <summary>Returns the Chat Id for <see cref="Command"/> processing. Returns 0 if not found.</summary>
        internal static long GetChatId(this UpdateEventArgs e)
        {
            var update = e?.Update;
            if (update == null) return 0;
            else if (update.CallbackQuery != null) return update.CallbackQuery?.Message?.Chat?.Id ?? 0;
            else if (update.Message != null) return update.Message?.Chat?.Id ?? 0;
            else return 0;
        }
    }
}
