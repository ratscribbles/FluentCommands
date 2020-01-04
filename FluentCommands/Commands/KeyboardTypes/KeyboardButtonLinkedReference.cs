using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces;
using FluentCommands.Utility;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Commands.KeyboardTypes
{
    public class KeyboardButtonLinkedReference : IFluentInterface
    {
        internal string Name { get; }
        internal Type LinkedModule { get; }

        internal KeyboardButtonLinkedReference(KeyboardButtonReference k, Type module) { Name = k.Name; LinkedModule = module; }

        /// <summary>
        /// Implicitly converts this <see cref="KeyboardButtonReference"/> into an <see cref="InlineKeyboardButton"/> to ease keyboard building.
        /// </summary>
        /// <param name="b">The <see cref="KeyboardButtonReference"/> to be converted into an <see cref="InlineKeyboardButton"/>.</param>
        public static implicit operator InlineKeyboardButton(KeyboardButtonLinkedReference b)
        {
            // Converts this base builder into a button with text that represents the key to access the actual Command.
            // This is done so that keyboards can reference commands even while they're being built; this is a reference that will be filled in later.
            var thisButton = new InlineKeyboardButton
            {
                Text = $"COMMANDBASEBUILDERREFERENCE::{b.Name}::{b.LinkedModule.Name}::"
            };
            return thisButton;
        }
        /// <summary>
        /// Implicitly converts this <see cref="KeyboardButtonReference"/> into a <see cref="KeyboardButton"/> to ease keyboard building.
        /// </summary>
        /// <param name="b">The <see cref="KeyboardButtonReference"/> to be converted into a <see cref="KeyboardButton"/>.</param>
        public static implicit operator KeyboardButton(KeyboardButtonLinkedReference b)
        {
            // Converts this base builder into a button with text that represents the key to access the actual Command.
            // This is done so that keyboards can reference commands even while they're being built; this is a reference that will be filled in later.
            var thisButton = new KeyboardButton
            {
                Text = $"COMMANDBASEBUILDERREFERENCE::{b.Name}::{b.LinkedModule.Name}::"
            };
            return thisButton;
        }
    }
}
