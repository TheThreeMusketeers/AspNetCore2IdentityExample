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
using System.Threading;
using System.Threading.Tasks;

namespace IdentityExample001.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly AppDbContext _dbContext;
        public UsersController(UserManager<UserEntity> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet(Name = nameof(GetUsersAsync))]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await _userManager.Users.ToArrayAsync());
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpGet("me", Name = nameof(GetMeAsync))]
        public async Task<IActionResult> GetMeAsync(CancellationToken ct)
        {
            if (User == null) return BadRequest();
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserViewModel registerUserViewModel)
        {
            if (!ModelState.IsValid) return BadRequest("Giriş bilgileri yetersiz.!");

            var entity = new UserEntity
            {
                Email = registerUserViewModel.Email,
                UserName = registerUserViewModel.Email,
                FirstName = registerUserViewModel.FirstName,
                LastName = registerUserViewModel.LastName,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var result = await _userManager.CreateAsync(entity, registerUserViewModel.Password);
            if(!result.Succeeded)
            {
                return BadRequest(ModelState);
            }

            OrganizationEntity orgEntity = new OrganizationEntity
            {
                Id = Guid.NewGuid(),
                Name = registerUserViewModel.Email,
                Description = "Otomatik olarak organizasyon oluşturuldu",
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = registerUserViewModel.Email
            };

            await _dbContext.Organizations.AddAsync(orgEntity);

            var orgResult =  await _dbContext.SaveChangesAsync();

            if (orgResult <= 0)
                return BadRequest("Organizasyon oluşturulamadı");

            return Ok(entity);
        }
    }//cs
}//ns
