using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Exceptions;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.BaseBuilders;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Commands.KeyboardTypes;
using FluentCommands.Utility;
using System.Text.RegularExpressions;

namespace FluentCommands.Commands
{
    /// <summary>
    /// Parent builder of all <see cref="IReplyMarkup"/> types. Stores keyboard information and provides it on demand.
    /// </summary>
    public class KeyboardBuilder : IInlineKeyboardBuilder, IReplyKeyboardBuilder, IFluentInterface
    {
        private bool _rowsUpdated = false;

        /// <summary>
        /// Gets the <see cref="InlineKeyboardButton"/> rows to be used to create an <see cref="InlineKeyboardMarkup"/>.
        /// </summary>
        internal List<InlineKeyboardButton[]> InlineRows { get; private set; } = new List<InlineKeyboardButton[]>();
        /// <summary>
        /// Gets the <see cref="KeyboardButton"/> rows to be used to create a <see cref="ReplyKeyboardMarkup"/>.
        /// </summary>
        internal List<KeyboardButton[]> ReplyRows { get; private set; } = new List<KeyboardButton[]>();
        /// <summary>
        /// Gets the <see cref="ReplyKeyboardRemove"/> to be used for this <see cref="KeyboardBuilder"/>.
        /// </summary>
        internal ReplyKeyboardRemove? ReplyRemove { get; private set; } = null;
        /// <summary>
        /// Gets the <see cref="ForceReplyMarkup"/> to be used for this <see cref="KeyboardBuilder"/>.
        /// </summary>
        internal ForceReplyMarkup? ForceReply { get; private set; } = null;
        /// <summary>
        /// Gets the boolean that will be assigned to <see cref="ReplyKeyboardMarkup.OneTimeKeyboard"/>.
        /// </summary>
        internal bool OneTimeKeyboard { get; private set; } = false;
        /// <summary>
        /// Gets the boolean that will be assigned to <see cref="ReplyKeyboardMarkup.ResizeKeyboard"/>.
        /// </summary>
        internal bool ResizeKeyboard { get; private set; } = false;
        /// <summary>
        /// Gets the boolean that will be assigned to <see cref="ReplyMarkupBase.Selective"/>.
        /// </summary>
        internal bool Selective { get; private set; } = false;

        /// <summary>
        /// Indexer that creates a reference to an <see cref="ICommand"/> object's <see cref="IKeyboardButton"/>.
        /// </summary>
        /// <param name="commandName">The name of this <see cref="Command"/> to reference for its <see cref="IKeyboardButton"/>.</param>
        /// <returns>Returns a reference to this <see cref="Command"/> object's <see cref="IKeyboardButton"/>.</returns>
        public KeyboardButtonReference this[string commandName]
        {
            get
            {
                return new KeyboardButtonReference(commandName);
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="KeyboardBuilder"/>. Contains information to construct an <see cref="IReplyMarkup"/> of some kind.
        /// </summary>
        internal KeyboardBuilder() { }

        /// <summary>
        /// Instantiates a new <see cref="KeyboardBuilder"/>. Contains information to construct an <see cref="IReplyMarkup"/> of some kind.
        /// </summary>
        /// <param name="forceReplyMarkup"></param>
        /// <param name="selective"></param>
        internal KeyboardBuilder(ForceReplyMarkup forceReplyMarkup, bool selective = false)
        {
            ForceReply = forceReplyMarkup;
            Selective = selective;
        }

        /// <summary>
        /// Instantiates a new <see cref="KeyboardBuilder"/>. Contains information to construct an <see cref="IReplyMarkup"/> of some kind.
        /// </summary>
        /// <param name="replyKeyboardRemove"></param>
        /// <param name="selective"></param>
        internal KeyboardBuilder(ReplyKeyboardRemove replyKeyboardRemove, bool selective = false)
        {
            ReplyRemove = replyKeyboardRemove;
            Selective = selective;
        }

        /// <summary>
        /// Adds a row of <see cref="InlineKeyboardButton"/>[] to this <see cref="IKeyboardBuilder{TBuilder}"/>.
        /// <para><see cref="InlineKeyboardMarkup"/> objects may only contain a maximum of 13 rows. Each row cannot contain less than 1 button, or more than 8 buttons.</para>
        /// <para>(If you have a 13th row, it may only have a maximum of 4 buttons.)</para>
        /// </summary>
        /// <param name="buttons">The buttons to be added to <see cref="InlineRows"/>.</param>
        /// <exception cref="InvalidKeyboardRowException">Throws if conditions for <see cref="InlineKeyboardMarkup"/> are not met: no buttons; more than eight buttons; more than 13 rows; more than 4 buttons on the 13th row.</exception>
        /// <returns>Returns this <see cref="IInlineKeyboardBuilder"/>, allowing you to continue adding rows.</returns>
        public IInlineKeyboardBuilder AddRow(params InlineKeyboardButton[] buttons)
        {
            InlineRows.Add(buttons);

            // Exceptions...
            if (InlineRows.Count == 13 && buttons.Length > 4) throw new InvalidKeyboardRowException("An Inline Keyboard's 13th row may only contain 4 buttons.");
            else if (InlineRows.Count > 13) throw new InvalidKeyboardRowException("Inline Keyboards may only have a maximum of 13 rows.");
            else if (buttons.Length <= 0) throw new InvalidKeyboardRowException("Inline Keyboard rows must contain at least one button.");
            else if (buttons.Length > 8) throw new InvalidKeyboardRowException("Inline Keyboard rows may only have a maximum of 8 buttons.");

            return this;
        }

        /// <summary>
        /// Adds a row of <see cref="KeyboardButton"/>[] to this <see cref="IKeyboardBuilder{TBuilder}"/>.
        /// <para><see cref="ReplyKeyboardMarkup"/> objects may only contain a maximum of 25 rows. Each row cannot contain less than 1 button, or more than 12 buttons.</para>
        /// </summary>
        /// <param name="buttons">The buttons to be added to <see cref="ReplyRows"/>.</param>
        /// <exception cref="InvalidKeyboardRowException">Throws if conditions for <see cref="ReplyKeyboardMarkup"/> are not met: no buttons; more than 12 buttons; more than 25 rows.</exception>
        /// <returns>Returns this <see cref="IReplyKeyboardBuilder"/>, allowing you to continue adding rows.</returns>
        public IReplyKeyboardBuilder AddRow(params KeyboardButton[] buttons)
        {
            ReplyRows.Add(buttons);

            // Exceptions...
            if (ReplyRows.Count > 25) throw new InvalidKeyboardRowException("Reply Keyboards may only have a maximum of 25 rows.");
            else if (buttons.Length <= 0) throw new InvalidKeyboardRowException("Reply Keyboard rows must contain at least one button.");
            else if (buttons.Length > 12) throw new InvalidKeyboardRowException("Reply Keyboard rows may only have a maximum of 12 buttons.");

            return this;
        }

        /// <summary>
        /// Finalizes the keyboard with optional settings for the <see cref="ReplyKeyboardMarkup"/> that will be generated from this builder.
        /// <para>OneTimeKeyboard: Requests clients to hide the keyboard as soon as it's been used. </para>
        /// <para>ResizeKeyboard: Requests clients to resize the keyboard vertically for optimal fit.</para>
        /// <para>Selective: Use this parameter if you want to show the keyboard to specific users only.</para>
        /// </summary>
        /// <param name="oneTimeKeyboard">The boolean that will be assigned to <see cref="ReplyKeyboardMarkup.OneTimeKeyboard"/>.</param>
        /// <param name="resizeKeyboard">The boolean that will be assigned to <see cref="ReplyKeyboardMarkup.ResizeKeyboard"/>.</param>
        /// <param name="selective">The boolean that will be assigned to <see cref="ReplyMarkupBase.Selective"/>.</param>
        public void BuildWithSettings(bool oneTimeKeyboard = false, bool resizeKeyboard = false, bool selective = false)
        {
            OneTimeKeyboard = oneTimeKeyboard;
            ResizeKeyboard = resizeKeyboard;
            Selective = selective;
        }

        /// <summary>
        /// Sets inline rows. Explicit method meant to be called ONLY ONCE when updating keyboard rows.
        /// </summary>
        internal void UpdateInline()
        {
            if (!_rowsUpdated) 
            { 
                InlineRows = UpdateKeyboardRows(InlineRows); 
                _rowsUpdated = true; 
            }
        }

        /// <summary>
        /// Implicitly converts a <see cref="KeyboardBuilder"/> into an <see cref="InlineKeyboardMarkup"/>.
        /// </summary>
        /// <param name="k"></param>
        public static implicit operator InlineKeyboardMarkup(KeyboardBuilder k)
        {
            var rows = UpdateKeyboardRows(k?.InlineRows ?? new List<InlineKeyboardButton[]>());
            return new InlineKeyboardMarkup(rows);
        }

        /// <summary>
        /// Implicitly converts a <see cref="KeyboardBuilder"/> into a <see cref="ReplyKeyboardMarkup"/>.
        /// </summary>
        /// <param name="k"></param>
        public static implicit operator ReplyKeyboardMarkup(KeyboardBuilder k)
        {
            var rows = k?.ReplyRows ?? new List<KeyboardButton[]>();
            return new ReplyKeyboardMarkup(rows, k?.ResizeKeyboard ?? false, k?.OneTimeKeyboard ?? false)
            {
                Selective = k?.Selective ?? false
            };
        }

        /// <summary>Updates keyboard rows by iterating through each row and checking each button for an implicitly-converted KeybaordButtonReference.</summary>
        internal static List<InlineKeyboardButton[]> UpdateKeyboardRows(List<InlineKeyboardButton[]> rows)
        {
            List<InlineKeyboardButton[]> updatedKeyboardBuilder = new List<InlineKeyboardButton[]>();

            foreach (var row in rows)
            {
                var updatedKeyboardButtons = new List<InlineKeyboardButton>();

                foreach (var button in row)
                {
                    if (button is { }
                        && button.Text is { }
                        && button.Text.Contains("COMMANDBASEBUILDERREFERENCE"))
                    {
                        var match = FluentRegex.CheckButtonLinkedReference.Match(button.Text);
                        if (!match.Success) throw new ArgumentException($"Unknown error occurred while building keyboard(s): button contained reference text \"{button.Text}\". Please report this bug if you encounter it; it shouldn't exist.");
                        else UpdateButton(match);

                        // Locates the reference being pointed to by this TButton and updates it.
                        void UpdateButton(Match m)
                        {
                            InlineKeyboardButton? referencedButton;

                            string commandNameReference = m.Groups[1].Value ?? throw new ArgumentException($"An unknown error occurred while building keyboards (command Name Reference was null).");
                            string moduleTextReference = match.Groups[2].Value ?? throw new ArgumentException($"An unknown error occurred while building keyboard(s) (module text reference was null).");
                            string keyboardContainer = $"Command \"{commandNameReference ?? "NULL"}\"";
                            var referencedModule = Type.GetType(moduleTextReference, false);

                            if (referencedModule is null) throw new ArgumentException($"{keyboardContainer} has a KeyboardBuilder that references a module that doesn't appear to exist.");

                            if (!CommandService.AccessCommands().TryGetValue(referencedModule, out var commandDictionary)) throw new ArgumentException($"{keyboardContainer} has a KeyboardBuilder that references a module that doesn't appear to exist.");
                            if (!commandDictionary.TryGetValue(commandNameReference.AsMemory(), out var outCommand))
                                throw new ArgumentException($"{keyboardContainer} has a KeyboardBuilder that references a command that doesn't exist in linked module: {referencedModule?.FullName ?? "NULL"} (Please check the builder to make sure it exists, or remove the reference to \"{commandNameReference}\" in the keyboard builder.)");

                            referencedButton = outCommand.Button;

                            if (referencedButton is null)
                            {
                                if (!CommandService.GlobalConfig.BruteForceKeyboardReferences) throw new ArgumentException($"{keyboardContainer} has a KeyboardBuilder that references a command that doesn't have a keyboard button, and the configuration for the CommandService is not set to force building when this occurs. Please review your keyboard builders or enable BruteForceKeyboardReferences in your CommandServiceConfigBuilder.");
                                else
                                {
                                    // Attempts to create a reference to the command when a button reference isn't available.
                                    referencedButton = InlineKeyboardButton.WithCallbackData(commandNameReference, $"BUTTONREFERENCEDCOMMAND::{commandNameReference}::");
                                }
                            }

                            updatedKeyboardButtons.Add(referencedButton);
                        }
                    }
                    else updatedKeyboardButtons.Add(button);
                }

                updatedKeyboardBuilder.Add(updatedKeyboardButtons.ToArray());
            }

            return updatedKeyboardBuilder;
        }
    }
}
