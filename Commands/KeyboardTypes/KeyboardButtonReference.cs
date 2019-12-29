using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Commands.KeyboardTypes
{
    public class KeyboardButtonReference : IFluentInterface
    {
        internal string Name { get; private set; }

        internal KeyboardButtonReference(string commandName) => Name = commandName;

        /// <summary>
        /// Implicitly converts this <see cref="KeyboardButtonReference"/> into an <see cref="InlineKeyboardButton"/> to ease keyboard building.
        /// </summary>
        /// <param name="b">The <see cref="KeyboardButtonReference"/> to be converted into an <see cref="InlineKeyboardButton"/>.</param>
        public static implicit operator InlineKeyboardButton(KeyboardButtonReference b)
        {
            // Converts this base builder into a button with text that represents the key to access the actual Command.
            // This is done so that keyboards can reference commands even while they're being built; this is a reference that will be filled in later.
            var thisButton = new InlineKeyboardButton
            {
                Text = $"COMMANDBASEBUILDERREFERENCE::{b.Name}::"
            };
            return thisButton;
        }
        /// <summary>
        /// Implicitly converts this <see cref="KeyboardButtonReference"/> into a <see cref="KeyboardButton"/> to ease keyboard building.
        /// </summary>
        /// <param name="b">The <see cref="KeyboardButtonReference"/> to be converted into a <see cref="KeyboardButton"/>.</param>
        public static implicit operator KeyboardButton(KeyboardButtonReference b)
        {
            // Converts this base builder into a button with text that represents the key to access the actual Command.
            // This is done so that keyboards can reference commands even while they're being built; this is a reference that will be filled in later.
            var thisButton = new KeyboardButton
            {
                Text = $"COMMANDBASEBUILDERREFERENCE::{b.Name}::"
            };
            return thisButton;
        }

        /// <summary>
        /// Creates a reference to a <see cref="Command"/> object's button (or actual command if no button is set for it) that exists in another module.
        /// </summary>
        /// <param name="module">The command module containing this <see cref="Command"/>.</param>
        /// <returns></returns>
        public KeyboardButtonLinkedReference InModule(Type module) => new KeyboardButtonLinkedReference(this, module);

        /// <summary>
        /// Creates a reference to a <see cref="Command"/> object's button (or actual command if no button is set for it) that exists in another module.
        /// </summary>
        /// <typeparam name="TCommand">The command module containing this <see cref="Command"/>.</typeparam>
        /// <returns></returns>
        public KeyboardButtonLinkedReference InModule<TCommand>() where TCommand : class => new KeyboardButtonLinkedReference(this, typeof(TCommand));
    }
}
