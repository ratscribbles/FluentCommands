using FluentCommands.Interfaces.MenuBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors
{
    public interface IModuleBuilder : IFluentInterface
    {
        internal string[] In_Aliases { get; set; }
        internal ISendableMenu? In_HelpDescription { get; set; }
        internal ISendableMenu In_ErrorMessage { get; set; }
        internal InlineKeyboardButton In_Button { get; set; }

        internal string[] Out_Aliases() => In_Aliases;
        internal ISendableMenu? Out_HelpDescription() => In_HelpDescription;
        internal ISendableMenu? Out_ErrorMessage() => In_ErrorMessage;
        internal InlineKeyboardButton Out_Button() => In_Button;
    }
}
