using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityExample001.Core.Models;
using IdentityExample001.Core.ViewModels;
using IdentityExample001.Persistence;

namespace IdentityExample001.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        public ProductService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProductEntity> Add(CreateProductViewModel createProductViewModel, UserEntity user)
        {
            ProductEntity entity = new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = createProductViewModel.Name,
                Description = createProductViewModel.Description,
                CreatedBy = user.UserName,
                CreatedAt = DateTimeOffset.UtcNow,
                OrganizationId = user.OrganizationId
            };

            await _dbContext.Products.AddAsync(entity);

            return entity;
        }
    }//cs
}//ns
