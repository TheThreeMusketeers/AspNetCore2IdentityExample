using IdentityExample001.Core.Models;
using IdentityExample001.Core.ViewModels;
using IdentityExample001.Persistence;
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
    public class UsersController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly AppDbContext _dbContext;
        public UsersController(UserManager<UserEntity> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userManager.Users.ToArrayAsync());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserViewModel registerUserViewModel)
        {
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

            return Ok(entity);
        }
    }//cs
}//ns
