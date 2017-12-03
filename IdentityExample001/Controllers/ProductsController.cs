using AspNet.Security.OAuth.Validation;
using AutoMapper;
using IdentityExample001.Core.Models;
using IdentityExample001.Core.Resources;
using IdentityExample001.Persistence;
using IdentityExample001.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityExample001.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
      
        private readonly UserManager<UserEntity> userManager;
        private readonly IAuthorizationService authorizationService;
        private readonly IProductService productService;
        private readonly IUnitOfWork unitOfWork;
        private readonly PagingOptions defaultPagingOptions;

        public ProductsController(
            UserManager<UserEntity> userManager,
            IAuthorizationService authorizationService,
            IProductService productService,
            IUnitOfWork unitOfWork,
            IOptions<PagingOptions> defaultPagingOptions)
        {
          
            this.userManager = userManager;
            this.authorizationService = authorizationService;
            this.productService = productService;
            this.unitOfWork = unitOfWork;
            this.defaultPagingOptions = defaultPagingOptions.Value;
        }


        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost(Name = nameof(CreateProduct))]
        public async Task<IActionResult> CreateProduct([FromBody] SaveProduct product)
        {
            if (User == null) return BadRequest();

            if(User.Identity.IsAuthenticated)
            {
                var canCreateProductPolicy = await authorizationService.AuthorizeAsync(User, Common.Policies.Policies.CreateProductPolicy);
                if(!canCreateProductPolicy.Succeeded)
                {
                    return BadRequest("Kullanıcı yetkili değil");
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(new ApiError(ModelState));

            var user = await userManager.GetUserAsync(User);

            var productEntity = Mapper.Map<SaveProduct, ProductEntity>(product);

            productEntity = await productService.AddAsync(productEntity, user);

            int result = await unitOfWork.CompleteAsync();

            if (result<=0)
            {
                return BadRequest(ModelState);
            }
           
            return Ok(productEntity);
        }//CreateProduct

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPut("{id}", Name = nameof(UpdateProduct))]
        public async Task<IActionResult> UpdateProduct(Guid id,[FromBody] SaveProduct product)
        {
            if (User == null) return BadRequest();

            if (User.Identity.IsAuthenticated)
            {
                var canCreateProductPolicy = await authorizationService.AuthorizeAsync(User, Common.Policies.Policies.CreateProductPolicy);
                if (!canCreateProductPolicy.Succeeded)
                {
                    return BadRequest("Kullanıcı yetkili değil");
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);

            ProductEntity entity = await productService.Get(id);

            if (entity == null) return BadRequest(new ApiError { Message = "Product not found!" });

            entity = await productService.UpdateAsync(entity, user);

            if (entity == null) return BadRequest("Ürün bulunamadı");


            var result = await unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                return BadRequest(ModelState);
            }


            return Ok(entity);
        }//UpdateProduct

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpDelete("{id}",Name = nameof(DeleteProduct))]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            if (User == null) return BadRequest();

            if (User.Identity.IsAuthenticated)
            {
                var canCreateProductPolicy = await authorizationService.AuthorizeAsync(User, Common.Policies.Policies.CreateProductPolicy);
                if (!canCreateProductPolicy.Succeeded)
                {
                    return BadRequest("Kullanıcı yetkili değil");
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

          
            var user = await userManager.GetUserAsync(User);

            bool isDeleted = await productService.DeleteAsync(id);

            if (!isDeleted) return BadRequest("Ürün bulunamadı");

            var result = await unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }//DeleteProduct



        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpGet(Name = nameof(GetProducts))]
        public async Task<IActionResult> GetProducts(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<Product,ProductEntity> sortOptions,
            CancellationToken ct)
        {

            if (User == null) return BadRequest();

            var user = await userManager.GetUserAsync(User);

            pagingOptions.Offset = pagingOptions.Offset ?? defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? defaultPagingOptions.Limit;

            PagedResults<ProductEntity> productEntities = await productService.GetProductsAsync(pagingOptions,sortOptions,user,ct);

            PagedResults<Product> products = Mapper.Map<PagedResults<ProductEntity>, PagedResults<Product>>(productEntities);

            var collection = new PagedCollection<Product>
            {
                Value = products.Items.ToArray(),
                Size = products.TotalSize,
                Offset = pagingOptions.Offset.Value,
                Limit = pagingOptions.Limit.Value
            };

            return Ok(collection);
           
        }//GetProducts

    }//cs
}//ns
