using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using PokeAPI.Models;
using PokeAPI.Services;
using System.Security.Claims;

namespace PokeAPI.Controllers
{
    public class YandexController : AbstractPageController
    {
        public YandexController(IFileProvider fileProvider) : base(fileProvider)
        {
        }


        [HttpPost("/yandex-login")]
        public async Task<IActionResult> YandexLogin([FromServices] Services.IAuthenticationService authenticationService, [FromBody] User user)
        {
            authenticationService.Registration(user.Email, "123");
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };    
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return Redirect("/");
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Вы вышли из системы");
        }

        [HttpGet("/current")]
        public IActionResult Current()
        {
            return Ok(new { UserName = User.Identity.Name });
        }

        [HttpGet("/yandex-data")]
        public IActionResult YandexData([FromServices] YandexApi yandexApi)
        {
            return Ok(new
            {
                ClientId = yandexApi.ClientId,
                ResponseType = yandexApi.ResponseType,
                RedirectUri = yandexApi.RedirectUri,
                TokenPageOrigin = yandexApi.TokenPageOrigin
            });
        }

        [HttpGet("/yandex-test")]
        public IActionResult YandexTest()
        {
            return GetPage("test");
        }
    }
}
