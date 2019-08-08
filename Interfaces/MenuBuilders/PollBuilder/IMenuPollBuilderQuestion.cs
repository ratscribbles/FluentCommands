using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.PollBuilder
{
    public interface IMenuPollBuilderQuestion : IFluentInterface
    {
        IMenuPollOptionalBuilder Options(params string[] options);
        IMenuPollOptionalBuilder Options(IEnumerable<string> options);
    }
}
