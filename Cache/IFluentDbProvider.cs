using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Cache
{
    public interface IFluentDbProvider
    {
        Task AddOrUpdateState(FluentState state);
        Task<FluentState> GetState(int id);
    }
}
