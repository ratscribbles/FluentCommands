using FluentCommands.Interfaces.MenuBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors
{
    public interface ICommandBaseBuilder : IFluentInterface
    {
        internal string[] Out_Aliases { get; }
        internal ISendableMenu? Out_HelpDescription { get; }
        internal ISendableMenu? Out_ErrorMessage { get; }
        internal InlineKeyboardButton? Out_Button { get; }
    }
}
