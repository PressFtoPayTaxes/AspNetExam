using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetExam.DataAccess;
using AspNetExam.DTOs;
using AspNetExam.Models;
using AspNetExam.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetExam.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IAuthService authService;

        public AuthController(DataContext context, IAuthService authService)
        {
            this.context = context;
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var newUser = new User
            {
                Login = user.Login,
                Password = user.Password,
                FullName = user.FullName,
                Age = user.Age
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(AuthDTO authDTO)
        {
            var token = await authService.Authenticate(authDTO.Login, authDTO.Password);

            if(token == null)
            {
                return Unauthorized("Invalid login/password combination");
            }

            return Ok(token);
        }
    }
}