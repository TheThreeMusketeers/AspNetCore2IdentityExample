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
            CreateMap<SaveProduct, ProductEntity>()
                .ForMember(p => p.Id, opt => opt.Ignore());

            //Entity to resources
            CreateMap<AuditEntity, Audit>();
            CreateMap<OrganizationEntity, Organization>();
            CreateMap<ProductEntity, Product>();
            CreateMap(typeof(PagedResults<>), typeof(PagedResults<>));
        }
    }//cs
}//ns
