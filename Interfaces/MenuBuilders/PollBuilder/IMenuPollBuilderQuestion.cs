using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.PollBuilder
{
    public interface IMenuPollBuilderQuestion : IFluentInterface
    {
        /// <summary>
        /// Required. List of answer options, 2-10 strings 1-100 characters each.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPollOptionalBuilder Options(params string[] options);
        /// <summary>
        /// Required. List of answer options, 2-10 strings 1-100 characters each.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPollOptionalBuilder Options(IEnumerable<string> options);
    }
}
