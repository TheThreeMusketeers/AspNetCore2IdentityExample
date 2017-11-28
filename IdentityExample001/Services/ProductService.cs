using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityExample001.Core.Models;
using IdentityExample001.Core.ViewModels;
using IdentityExample001.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdentityExample001.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        public ProductService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProductEntity> AddAsync(CreateProductViewModel createProductViewModel, UserEntity user)
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            ProductEntity entity = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id==id);
            if (entity == null) return false;

            _dbContext.Products.Remove(entity);

            return true;
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsAsync(UserEntity user)
        {
            IEnumerable<ProductEntity> products = await _dbContext.Products.Where(p => p.OrganizationId == user.OrganizationId).ToListAsync();

            return products;
        }

        public async Task<ProductEntity> UpdateAsync(UpdateProductViewModel model, UserEntity user)
        {
            ProductEntity entity = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == model.Id);

            if (entity == null) return null;

            entity.Description = model.Description;
            entity.Name = model.Name;
            entity.LastUpdatedBy = user.UserName;
            entity.LastUpdatedAt = DateTimeOffset.UtcNow;

            return entity;

        }
    }//cs
}//ns
