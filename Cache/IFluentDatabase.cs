using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Cache
{
    public interface IFluentDatabase
    {
        Task AddOrUpdateState(FluentState state);
        Task<FluentState> GetState(long chatId, int userId);
    }
}
