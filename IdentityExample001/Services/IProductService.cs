using IdentityExample001.Core.Models;
using IdentityExample001.Core.Resources;
using IdentityExample001.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityExample001.Services
{
    public interface IProductService
    {
        Task<ProductEntity> AddAsync(CreateProductViewModel createProductViewModel,UserEntity user);
        Task<ProductEntity> UpdateAsync(UpdateProductViewModel model, UserEntity user);
        Task<Boolean> DeleteAsync(Guid id);
        Task<PagedResults<ProductEntity>> GetProductsAsync(PagingOptions pagingOptions,SortOptions<Product,ProductEntity> sortOptions,UserEntity user,CancellationToken ct);
    }//cs
}//ns
