using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using PokeAPI.Services;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Linq;

namespace PokeAPI.Controllers
{
    public class AuthorizationController : AbstractPageController
    {
        private IUserService userService;

        public AuthorizationController(IFileProvider fileProvider, IUserService userService) : base(fileProvider)
        {
            this.userService = userService;
        }

        [HttpGet("/login")]
        public IActionResult Index()
        {
            return GetPage("authorization");
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Вы вышли из системы");
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Autorization([FromForm] string username, [FromForm] string password)
        {
            var user = await userService.GetUser(username, password);

            if (user != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect("/");
            }
            return Unauthorized("Неправильный логин или пароль");
        }
    }
}
