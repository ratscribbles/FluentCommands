using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Cache;
using Microsoft.EntityFrameworkCore;

namespace FluentCommands.Tests.Unit
{
    class TestDbContext : DbContext, IFluentDbProvider
    {
        void IFluentDbProvider.AddOrUpdateState(FluentState state)
        {
            throw new NotImplementedException();
        }

        FluentState IFluentDbProvider.GetState(int id)
        {
            throw new NotImplementedException();
        }
    }
}
