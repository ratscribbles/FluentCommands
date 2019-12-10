using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentCommands.Cache;
using Microsoft.EntityFrameworkCore;

namespace FluentCommands.Tests.Unit
{
    class TestDbContext : DbContext, IFluentDatabase
    {
        public Task AddOrUpdateState(FluentState state)
        {
            throw new NotImplementedException();
        }

        public Task<FluentState> GetState(int id)
        {
            throw new NotImplementedException();
        }
    }
}
