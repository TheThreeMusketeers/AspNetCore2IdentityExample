using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample001.Persistence
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
    }//cs
}//ns
