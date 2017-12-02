using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample001.Core.Resources
{
    public class Audit
    {
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public DateTimeOffset CreatedAt { get; set; }
        public Guid LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }//cs
}//ns
