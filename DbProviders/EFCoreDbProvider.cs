using FluentCommands.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.DbProviders
{
    internal class EFCoreDbProvider : IFluentDbProvider
    {
        public TContext DbContext { get; private set; }

        public EFCoreDbProvider(TContext dbContext) => DbContext = dbContext;

        internal void SetContext<TContext>() where TContext : Microsoft.EntityFrameworkCore.DbContext, new()
        {

        }

        async Task IFluentDbProvider.AddOrUpdateState(FluentState state)
        {
            using var db = new TContext();
            //: do checks etc
            db.Add(state);
            await db.SaveChangesAsync();
        }

        Task<FluentState> IFluentDbProvider.GetState(int id)
        {
            throw new NotImplementedException();
        }
    }
}
