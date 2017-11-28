using IdentityExample001.Core.Models;
using IdentityExample001.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample001.Services
{
    public interface IProductService
    {
        Task<ProductEntity> AddAsync(CreateProductViewModel createProductViewModel,UserEntity user);
        Task<ProductEntity> UpdateAsync(UpdateProductViewModel model, UserEntity user);
        Task<Boolean> DeleteAsync(Guid id);
        Task<IEnumerable<ProductEntity>> GetProductsAsync(UserEntity user);
    }//cs
}//ns
