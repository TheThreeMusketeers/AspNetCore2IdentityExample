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
        Task<ProductEntity> Add(CreateProductViewModel createProductViewModel,UserEntity user);
    }//cs
}//ns
