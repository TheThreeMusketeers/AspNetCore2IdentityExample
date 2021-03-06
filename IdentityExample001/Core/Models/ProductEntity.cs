﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample001.Core.Models
{
    [Table("Products")]
    public class ProductEntity : AuditEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public Guid OrganizationId { get; set; }

        public OrganizationEntity Organization { get; set; }
    }
}
