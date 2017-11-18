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

        public ProductsController(AppDbContext dbContext, UserManager<UserEntity> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }


        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost(Name = nameof(CreateProduct))]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductViewModel createProductViewModel)
        {
            if (User == null) return BadRequest();
            
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
