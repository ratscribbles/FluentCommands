using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentCommands.Builders;
using FluentCommands.CommandTypes;
using FluentCommands.Exceptions;
using FluentCommands.Extensions;
using Telegram.Bot.Args;

namespace FluentCommands.Utility
{
    internal static class AuxiliaryMethods
    {
        /// <summary>
        /// Checks a string to see if it successfully clears the conditions for a <see cref="Command"/> name.
        /// <para>Throws if it doesn't.</para>
        /// </summary>
        /// <exception cref="CommandOnBuildingException"></exception>
        /// <exception cref="InvalidCommandNameException"></exception>
        internal static void CheckCommandNameValidity(string? commandName, bool isAlias = false, string? aliasName = null)
        {
            if (isAlias)
            {
                if (string.IsNullOrWhiteSpace(aliasName))
                {
                    if (string.IsNullOrWhiteSpace(commandName)) throw new InvalidCommandNameException($"Command name AND alias were both null, empty, or whitespace.");
                }
                else CheckName(aliasName, isAlias);
            }
            else CheckName(commandName);

            void CheckName(string? name, bool alias = false)
            {
                string nullOrWhitespace;
                string tooLong;
                string containsWhitespaceCharacters;
                string containsNonAlphanumericCharacters;
                string regexTimeout;
                string tempCommandName;
                string tempName;

                if (string.IsNullOrWhiteSpace(commandName)) tempCommandName = "NULL";
                else tempCommandName = commandName;
                if (string.IsNullOrWhiteSpace(name)) tempName = "NULL";
                else tempName = name;

                if (alias)
                {
                    nullOrWhitespace = $"Command \"{tempCommandName}\": Command had alias that was null, empty, or whitespace.";
                    tooLong = $"Command \"{tempCommandName}\": Alias \"{tempName}\" was too long — Command names and aliases may only be a maximum of 255 characters.";
                    containsWhitespaceCharacters = $"Command \"{tempCommandName}\": Alias \"{tempName}\" — Command names and aliases cannot contain whitespace characters.";
                    containsNonAlphanumericCharacters = $"Command \"{tempCommandName}\": Alias \"{tempName}\" — Command names and aliases cannot contain non-alphanumeric characters.";
                    regexTimeout = $"Command \"{tempCommandName}\": Alias \"{tempName}\" caused a Regex Timeout while checking if the command's name was valid: ";
                }
                else
                {
                    nullOrWhitespace = $"Command name was null, empty, or whitespace.";
                    tooLong = $"Command \"{tempName}\": Command names may only be a maximum of 255 characters.";
                    containsWhitespaceCharacters = $"Command \"{tempName}\": Command names cannot contain whitespace characters.";
                    containsNonAlphanumericCharacters = $"Command \"{tempName}\": — Command names and aliases cannot contain non-alphanumeric characters.";
                    regexTimeout = $"Command \"{tempName}\": caused a Regex Timeout while checking if the command's name was valid: ";
                }

                if (string.IsNullOrWhiteSpace(name)) throw new InvalidCommandNameException(nullOrWhitespace);
                if (name.Length > 255) throw new InvalidCommandNameException(tooLong);
                try
                {
                    if (FluentRegex.CheckForWhiteSpaces.IsMatch(name)) throw new InvalidCommandNameException(containsWhitespaceCharacters);
                    if (FluentRegex.CheckNonAlphanumeric.IsMatch(name)) throw new InvalidCommandNameException(containsNonAlphanumericCharacters);
                }
                catch (RegexMatchTimeoutException e)
                {
                    throw new CommandOnBuildingException(regexTimeout, e);
                }
            }
        }
        /// <summary>
        /// Checks which <see cref="EventArgs"/> is contained within the <see cref="TelegramUpdateEventArgs"/>, and returns the user's raw input for <see cref="Command"/> processing. 
        /// <para>Returns false if the raw input was unable to be found, and outs an empty string.</para>
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        internal static bool TryGetEventArgsRawInput(TelegramUpdateEventArgs e, out ReadOnlyMemory<char> rawInput)
        {
            rawInput = e switch
            {
                var _ when e.CallbackQueryEventArgs is { } => e.CallbackQueryEventArgs.GetRawInput(),
                var _ when e.ChosenInlineResultEventArgs is { } => e.ChosenInlineResultEventArgs.GetRawInput(),
                var _ when e.InlineQueryEventArgs is { } => e.InlineQueryEventArgs.GetRawInput(),
                var _ when e.MessageEventArgs is { } => e.MessageEventArgs.GetRawInput(),
                var _ when e.UpdateEventArgs is { } => e.UpdateEventArgs.GetRawInput(),
                _ => ReadOnlyMemory<char>.Empty,
            };

            if (rawInput.IsEmpty) return false;
            else return true;
        }
        /// <summary>Checks which <see cref="EventArgs"/> is contained within the <see cref="TelegramUpdateEventArgs"/>, and returns the Chat Id for <see cref="Command"/> processing. 
        /// <para>Returns false if not found, or if a bot (this bot) is the sender, and outs "0".</para></summary>
        internal static bool TryGetEventArgsChatId(TelegramUpdateEventArgs e, out long chatId)
        {
            chatId = e switch
            {
                var _ when e.CallbackQueryEventArgs is { } => e.CallbackQueryEventArgs.GetChatId(),
                var _ when e.ChosenInlineResultEventArgs is { } => e.ChosenInlineResultEventArgs.GetChatId(),
                var _ when e.InlineQueryEventArgs is { } => e.InlineQueryEventArgs.GetChatId(),
                var _ when e.MessageEventArgs is { } => e.MessageEventArgs.GetChatId(),
                var _ when e.UpdateEventArgs is { } => e.UpdateEventArgs.GetChatId(),
                _ => 0,
            };

            if (chatId == 0) return false;
            else return true;
        }
        /// <summary>Checks if the given EventArgs is a TelegramEventArgs object, and returns the Chat Id for <see cref="Command"/> processing. 
        /// <para>Returns false if not found, or if a bot (this bot) is the sender, and outs "0".</para></summary>
        internal static bool TryGetEventArgsUserId(TelegramUpdateEventArgs e, out int userId)
        {
            userId = e switch
            {
                var _ when e.CallbackQueryEventArgs is { } => e.CallbackQueryEventArgs.GetUserId(),
                var _ when e.ChosenInlineResultEventArgs is { } => e.ChosenInlineResultEventArgs.GetUserId(),
                var _ when e.InlineQueryEventArgs is { } => e.InlineQueryEventArgs.GetUserId(),
                var _ when e.MessageEventArgs is { } => e.MessageEventArgs.GetUserId(),
                var _ when e.UpdateEventArgs is { } => e.UpdateEventArgs.GetUserId(),
                _ => 0,
            };

            if (userId == 0) return false;
            else return true;
        }

        /// <summary>Converts the <see cref="MethodInfo"/> to the appropriate <see cref="CommandDelegate{TArgs}"/> based on the generic arguments.</summary>
        internal static bool TryConvertDelegate<TArgs>(MethodInfo method, [NotNullWhen(true)] out CommandDelegate<TArgs>? c) where TArgs : EventArgs
        {
            if (method is null) { c = null; return false; }

            c = (CommandDelegate<TArgs>)Delegate.CreateDelegate(typeof(CommandDelegate<TArgs>), null, method);

            if (c is null) return false;
            else return true;
        }
        /// <summary>Converts the <see cref="MethodInfo"/> to the appropriate <see cref="CommandDelegate{TArgs, TReturn}"/> based on the generic arguments.<para>Can return null.</para></summary>
        internal static bool TryConvertDelegate<TArgs, TReturn>(MethodInfo method, [NotNullWhen(true)] out CommandDelegate<TArgs, TReturn>? c) where TArgs : EventArgs
        {
            if (method is null) { c = null; return false; }
            
            c = (CommandDelegate<TArgs, TReturn>)Delegate.CreateDelegate(typeof(CommandDelegate<TArgs, TReturn>), null, method);

            if (c is null) return false;
            else return true;
        }
    }
}
