﻿using IdentityExample001.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample001.Persistence
{
    public class AppDbContext : IdentityDbContext<UserEntity, UserRoleEntity,Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.UseOpenIddict<Guid>();
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrganizationEntity> Organizations { get; set; }
    }//cs
}//ns
