using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menus;
using FluentCommands.Interfaces.MenuBuilders.PollBuilder;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuPollBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the Question for this Poll.
        /// <para>1-255 characters.</para>
        /// </summary>
        /// <param name="question"></param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPollBuilderQuestion Question(string question);
    }
}
