using AspNet.Security.OAuth.Validation;
using IdentityExample001.Core.Models;
using IdentityExample001.Core.ViewModels;
using IdentityExample001.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    }//cs
}//ns
