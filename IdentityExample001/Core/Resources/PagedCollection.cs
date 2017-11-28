using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample001.Core.Resources
{
    public class PagedCollection<T>:Collection<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Offset { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Limit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Size { get; set; }
    }//cs
}//ns
