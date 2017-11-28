using AspNet.Security.OAuth.Validation;
using IdentityExample001.Core.Models;
using IdentityExample001.Core.Resources;
using IdentityExample001.Core.ViewModels;
using IdentityExample001.Persistence;
using IdentityExample001.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _dbContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IProductService _productService;
        private readonly IUnitOfWork unitOfWork;

        public ProductsController(AppDbContext dbContext, 
            UserManager<UserEntity> userManager,
            IAuthorizationService authorizationService,
            IProductService productService,
            IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _productService = productService;
            this.unitOfWork = unitOfWork;
        }


        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost(Name = nameof(CreateProduct))]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductViewModel createProductViewModel)
        {
            if (User == null) return BadRequest();

            if(User.Identity.IsAuthenticated)
            {
                var canCreateProductPolicy = await _authorizationService.AuthorizeAsync(User, Common.Policies.Policies.CreateProductPolicy);
                if(!canCreateProductPolicy.Succeeded)
                {
                    return BadRequest("Kullanıcı yetkili değil");
                }
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);

            var entity = await _productService.AddAsync(createProductViewModel, user);

            int result = await unitOfWork.CompleteAsync();

            if (result<=0)
            {
                return BadRequest(ModelState);
            }
           
            return Ok(entity);
        }//CreateProduct

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPut(Name = nameof(UpdateProduct))]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductViewModel updateProductViewModel)
        {
            if (User == null) return BadRequest();

            if (User.Identity.IsAuthenticated)
            {
                var canCreateProductPolicy = await _authorizationService.AuthorizeAsync(User, Common.Policies.Policies.CreateProductPolicy);
                if (!canCreateProductPolicy.Succeeded)
                {
                    return BadRequest("Kullanıcı yetkili değil");
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);

            ProductEntity entity = await _productService.UpdateAsync(updateProductViewModel, user);

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
                var canCreateProductPolicy = await _authorizationService.AuthorizeAsync(User, Common.Policies.Policies.CreateProductPolicy);
                if (!canCreateProductPolicy.Succeeded)
                {
                    return BadRequest("Kullanıcı yetkili değil");
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

          
            var user = await _userManager.GetUserAsync(User);

            bool isDeleted = await _productService.DeleteAsync(id);

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
        public async Task<IActionResult> GetProducts([FromQuery]PagingOptions pagingOptions,CancellationToken ct)
        {
            if (User == null) return BadRequest();

            var user = await _userManager.GetUserAsync(User);

            IEnumerable<ProductEntity> products = await _productService.GetProductsAsync(pagingOptions,user,ct);

            var collection = new Collection<ProductEntity>
            {
                Value = products.ToArray()
            };

            return Ok(collection);
           
        }//GetProducts

    }//cs
}//ns
