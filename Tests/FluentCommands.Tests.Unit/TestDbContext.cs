using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentCommands.Cache;
using Microsoft.EntityFrameworkCore;

namespace FluentCommands.Tests.Unit
{
    class TestDbContext : IFluentDatabase
    {
        public Task AddOrUpdateState(FluentState state)
        {
            return Task.CompletedTask;
        }

        public async Task<FluentState> GetState(int id)
        {
            return await Task.Run(() => { return FluentState.Default<StepState>(); });
        }
    }
}
