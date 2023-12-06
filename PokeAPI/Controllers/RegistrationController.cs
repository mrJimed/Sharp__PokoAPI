using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using PokeAPI.Services;

namespace PokeAPI.Controllers
{
    [Route("/registration")]
    public class RegistrationController : AbstractPageController
    {
        private IAuthenticationService authenticationService;

        public RegistrationController([FromServices] IAuthenticationService authenticationService, IFileProvider fileProvider) : base(fileProvider)
        {
            this.authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Page()
        {
            return GetPage("registration");
        }

        [HttpPost]
        public IActionResult Registration([FromForm] string email, [FromForm] string password)
        {
            if (!authenticationService.Registration(email, password))
                return Conflict(new { Message = "Пользователь с таким email уже существует" });
            return RedirectPermanent("/");
        }
    }
}
