using AspNet.Security.OAuth.Validation;
using IdentityExample001.Core.Models;
using IdentityExample001.Core.ViewModels;
using IdentityExample001.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample001.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {

        private readonly AppDbContext _dbContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public ProductsController(AppDbContext dbContext, 
            UserManager<UserEntity> userManager,
            IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _authorizationService = authorizationService;
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

            var result = await _dbContext.SaveChangesAsync();

            if(result<=0)
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

            ProductEntity entity = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == updateProductViewModel.Id);

            if (entity == null) return BadRequest("Ürün bulunamadı");

            entity.Description = updateProductViewModel.Description;
            entity.Name = updateProductViewModel.Name;
            entity.LastUpdatedBy = user.UserName;
            entity.LastUpdatedAt = DateTimeOffset.UtcNow;

            var result = await _dbContext.SaveChangesAsync();

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

            //Guid entityId = new Guid(id);
            Guid entityId = id;

            var user = await _userManager.GetUserAsync(User);

            ProductEntity entity = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == entityId);

            if (entity == null) return BadRequest("Ürün bulunamadı");

            _dbContext.Products.Remove(entity);

            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }//DeleteProduct



        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpGet(Name = nameof(CreateProduct))]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _dbContext.Products.Include(p=>p.Organization).ToArrayAsync());
        }//GetProducts

    }//cs
}//ns
