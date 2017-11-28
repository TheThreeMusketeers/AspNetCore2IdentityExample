using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample001.Core.Resources
{
    public class PagedResults<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalSize { get; set; }
    }//cs
}//ns
