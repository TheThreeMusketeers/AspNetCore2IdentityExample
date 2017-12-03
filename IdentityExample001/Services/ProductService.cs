using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityExample001.Core.Models;
using IdentityExample001.Persistence;
using Microsoft.EntityFrameworkCore;
using IdentityExample001.Core.Resources;
using System.Threading;

namespace IdentityExample001.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext dbContext;
        public ProductService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ProductEntity> AddAsync(ProductEntity product, UserEntity user)
        {
            product.CreatedAt = DateTimeOffset.UtcNow;
            product.CreatedBy = user.Id;
            product.OrganizationId = user.OrganizationId;

            await dbContext.Products.AddAsync(product);

            return product;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            ProductEntity entity = await dbContext.Products.SingleOrDefaultAsync(p => p.Id==id);
            if (entity == null) return false;

            dbContext.Products.Remove(entity);

            return true;
        }

        public async Task<ProductEntity> Get(Guid id)
        {
            return await dbContext.Products.Where(p => p.Id == id).SingleOrDefaultAsync();
        }

        public async Task<PagedResults<ProductEntity>> GetProductsAsync(
            PagingOptions pagingOptions,
            SortOptions<Product,ProductEntity> sortOptions,
            UserEntity user, 
            CancellationToken ct)
        {
            IQueryable<ProductEntity> query = dbContext.Products.Where(p=>p.OrganizationId == user.OrganizationId);

            query = sortOptions.Apply(query);

           // IEnumerable<ProductEntity> products = _dbContext.Products.Where(p => p.OrganizationId == user.OrganizationId).ToListAsync();
            var pagedProducts = await query.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value).ToListAsync();


            return new PagedResults<ProductEntity>
            {
                Items = pagedProducts,
                TotalSize =query.Count()
            };
        }

        public async Task<ProductEntity> UpdateAsync(ProductEntity product, UserEntity user)
        {
            ProductEntity entity = await dbContext.Products.SingleOrDefaultAsync(p => p.Id == product.Id);

            if (entity == null) return null;

            entity.Description = product.Description;
            entity.Name = product.Name;
            entity.LastUpdatedBy = user.Id;
            entity.LastUpdatedAt = DateTimeOffset.UtcNow;

            return entity;

        }
    }//cs
}//ns
