using AspNet.Security.OAuth.Validation;
using IdentityExample001.Core.Models;
using IdentityExample001.Core.Resources;
using IdentityExample001.Persistence;
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
    public class UsersController : Controller
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly AppDbContext dbContext;
        public UsersController(UserManager<UserEntity> userManager, AppDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        [HttpGet(Name = nameof(GetUsersAsync))]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await userManager.Users.ToArrayAsync());
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpGet("me", Name = nameof(GetMeAsync))]
        public async Task<IActionResult> GetMeAsync(CancellationToken ct)
        {
            if (User == null) return BadRequest();
            var user = await userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            var organization = await dbContext.Organizations.SingleOrDefaultAsync(o => o.Id == user.OrganizationId);

            user.Organization = organization;

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest("Giriş bilgileri yetersiz.!");

            var entity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = user.Email,
                UserName = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var result = await userManager.CreateAsync(entity, user.Password);
            if(!result.Succeeded)
            {
                return BadRequest(ModelState);
            }

            OrganizationEntity orgEntity = new OrganizationEntity
            {
                Id = Guid.NewGuid(),
                Name = user.Email,
                Description = "Otomatik olarak organizasyon oluşturuldu",
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = entity.Id
            };

            await dbContext.Organizations.AddAsync(orgEntity);

            var orgResult =  await dbContext.SaveChangesAsync();

            if (orgResult <= 0)
                return BadRequest("Organizasyon oluşturulamadı");

            return Ok(entity);
        }
    }//cs
}//ns
