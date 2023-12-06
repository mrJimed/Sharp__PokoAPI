using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.FileProviders;
using PokeAPI.Models;
using PokeAPI.Services;
using System.Security.Claims;

namespace PokeAPI.Controllers
{
    public class AuthorizationController : AbstractPageController
    {
        private Services.IAuthenticationService authenticationService;
        private IDistributedCache cache;
        private Emailer emailer;

        public AuthorizationController(IFileProvider fileProvider, Services.IAuthenticationService authenticationService,
            IDistributedCache cache, Emailer emailer) : base(fileProvider)
        {
            this.authenticationService = authenticationService;
            this.emailer = emailer;
            this.cache = cache;
        }

        [HttpGet("/login")]
        public IActionResult Index()
        {
            return GetPage("authorization");
        }

        [HttpPost("/pass-change")]
        public async Task<IActionResult> Pass([FromBody] User user, [FromServices] Emailer emailer)
        {
            var newPass = new Random().Next(10_000, 100_000);
            authenticationService.ChangePassword(user.Email, newPass.ToString());
            await emailer.SendEmail(user.Email, $"New pass: {newPass}", "New pass");
            return Ok();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if(!authenticationService.Authorization(user.Email, user.Password))
                return BadRequest(new { Message = "Неправильный логин или пароль" });

            var code = new Random().Next(1000, 10000);
            await HashCode(user.Email, code);
            await emailer.SendEmail(user.Email, $"Code: {code}", "Authorization Code");
            return Ok();
        }

        [HttpPost("/code")]
        public async Task<IActionResult> Code([FromForm] string email, [FromForm] string code)
        {
            var origCode = await cache.GetStringAsync(email);
            if (!origCode.Equals(code))
                return BadRequest();
            cache.Remove(email);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, email) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return Redirect("/");
        }

        [NonAction]
        private async Task HashCode(string email, int code)
        {
            await cache.SetStringAsync(email, $"{code}", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(4)
            });
        }

        //[HttpGet("/logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return Ok("Вы вышли из системы");
        //}

        //[HttpPost("/login")]
        //public async Task<IActionResult> Autorization([FromForm] string username, [FromForm] string password)
        //{
        //    var user = await userService.GetUser(username, password);

        //        if (user != null)
        //        {
        //            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
        //    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        //            return Redirect("/");
        //}
        //    return Unauthorized("Неправильный логин или пароль");
        //}
    }
}
