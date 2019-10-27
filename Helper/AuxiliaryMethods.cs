using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentCommands.Exceptions;
using Telegram.Bot.Args;

namespace FluentCommands.Helper
{
    internal static class AuxiliaryMethods
    {
        /// <summary>
        /// Checks a string to see if it successfully clears the conditions for a <see cref="Command"/> name.
        /// <para>Throws if it doesn't.</para>
        /// </summary>
        /// <exception cref="CommandOnBuildingException"></exception>
        /// <exception cref="InvalidCommandNameException"></exception>
        internal static void CheckCommandNameValidity(string commandName, bool isAlias = false, string aliasName = null)
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

            void CheckName(string name, bool alias = false)
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
        /// Checks an EventArgs to see if it's a recognized TelegramEventArgs, and then attempts to get the raw input from that EventArgs.
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        internal static string GetEventArgsRawInput(EventArgs eventArgs)
        {
            switch (eventArgs)
            {
                case var _ when eventArgs is CallbackQueryEventArgs:
                    return ((CallbackQueryEventArgs)eventArgs).GetRawInput();
                case var _ when eventArgs is ChosenInlineResultEventArgs:
                    return ((ChosenInlineResultEventArgs)eventArgs).GetRawInput();
                case var _ when eventArgs is InlineQueryEventArgs:
                    return ((InlineQueryEventArgs)eventArgs).GetRawInput();
                case var _ when eventArgs is MessageEventArgs:
                    return ((MessageEventArgs)eventArgs).GetRawInput();
                case var _ when eventArgs is UpdateEventArgs:
                    return ((UpdateEventArgs)eventArgs).GetRawInput();
                default:
                    return "";
            }
        }
        /// <summary>Checks if the given EventArgs is a TelegramEventArgs object, and returns the Chat Id for <see cref="Command"/> processing. 
        /// <para>Returns 0 if not found, or if a bot (this bot) is the sender.</para></summary>
        internal static long GetEventArgsChatId(EventArgs eventArgs)
        {
            switch (eventArgs)
            {
                case var _ when eventArgs is CallbackQueryEventArgs:
                    return ((CallbackQueryEventArgs)eventArgs).GetChatId();
                case var _ when eventArgs is ChosenInlineResultEventArgs:
                    return ((ChosenInlineResultEventArgs)eventArgs).GetChatId();
                case var _ when eventArgs is InlineQueryEventArgs:
                    return ((InlineQueryEventArgs)eventArgs).GetChatId();
                case var _ when eventArgs is MessageEventArgs:
                    return ((MessageEventArgs)eventArgs).GetChatId();
                case var _ when eventArgs is UpdateEventArgs:
                    return ((UpdateEventArgs)eventArgs).GetChatId();
                default:
                    return 0;
            }
        }
        /// <summary>Checks if the given EventArgs is a TelegramEventArgs object, and returns the Chat Id for <see cref="Command"/> processing. 
        /// <para>Returns 0 if not found, or if a bot (this bot) is the sender.</para></summary>
        internal static long GetEventArgsUserId(EventArgs eventArgs)
        {
            switch (eventArgs)
            {
                case var _ when eventArgs is CallbackQueryEventArgs:
                    return ((CallbackQueryEventArgs)eventArgs).GetUserId();
                case var _ when eventArgs is ChosenInlineResultEventArgs:
                    return ((ChosenInlineResultEventArgs)eventArgs).GetUserId();
                case var _ when eventArgs is InlineQueryEventArgs:
                    return ((InlineQueryEventArgs)eventArgs).GetUserId();
                case var _ when eventArgs is MessageEventArgs:
                    return ((MessageEventArgs)eventArgs).GetUserId();
                case var _ when eventArgs is UpdateEventArgs:
                    return ((UpdateEventArgs)eventArgs).GetUserId();
                default:
                    return 0;
            }
        }
    }
}
