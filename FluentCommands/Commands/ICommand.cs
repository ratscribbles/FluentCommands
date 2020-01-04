using FluentCommands.Commands.Steps;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Commands
{
    /// <summary>
    /// Marks an object as a valid <see cref="ICommand"/>. This interface is a marker for use in collections, and should only be used for <see cref="CommandBase{TContext, TArgs}"/> classes.
    /// </summary>
    internal interface ICommand
    {
        internal Type Module { get; }
        internal Type Context { get; }
        internal string Name { get; }
        internal string[] Aliases { get; }
        internal CommandType CommandType { get; }
        internal Permissions Permissions { get; }
        internal ISendableMenu Description { get; }
        internal ISendableMenu? ErrorMsg { get; }
        internal InlineKeyboardButton? Button { get; }
    }
}
