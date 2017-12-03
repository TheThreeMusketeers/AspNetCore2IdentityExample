using IdentityExample001.Core.Models;
using IdentityExample001.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityExample001.Services
{
    public interface IProductService
    {
        Task<ProductEntity> AddAsync(ProductEntity product,UserEntity user);
        Task<ProductEntity> UpdateAsync(ProductEntity product, UserEntity user);
        Task<Boolean> DeleteAsync(Guid id);
        Task<PagedResults<ProductEntity>> GetProductsAsync(PagingOptions pagingOptions,SortOptions<Product,ProductEntity> sortOptions,UserEntity user,CancellationToken ct);
        Task<ProductEntity> Get(Guid id);
    }//cs
}//ns
