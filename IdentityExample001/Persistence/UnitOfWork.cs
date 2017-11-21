using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample001.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }
    }//cs
}//ns
