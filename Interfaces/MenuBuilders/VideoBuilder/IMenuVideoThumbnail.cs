using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.VideoBuilder
{
    public interface IMenuVideoThumbnail : IFluentInterface, IMenuItem
    {
        IMenuItem Width(int width);
    }
}
