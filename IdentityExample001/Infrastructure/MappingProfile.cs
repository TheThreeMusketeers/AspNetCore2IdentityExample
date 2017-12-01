using AutoMapper;
using IdentityExample001.Core.Models;
using IdentityExample001.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample001.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Resources to entity
            CreateMap<Audit, AuditEntity>();
            CreateMap<Organization, OrganizationEntity>();
            CreateMap<Product, ProductEntity>();

            //Entity to resources
            CreateMap<AuditEntity, Audit>();
            CreateMap<OrganizationEntity, Organization>();
            CreateMap<ProductEntity, Product>();

        }
    }//cs
}//ns
