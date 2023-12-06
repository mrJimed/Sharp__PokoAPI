using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using PokeAPI.Models;
using PokeAPI.Services;

namespace PokeAPI.Controllers
{
    public class FightStatControllerr : AbstractPageController
    {
        private readonly IFightStatService fightStatService;

        public FightStatControllerr(IFileProvider fileProvider, IFightStatService fightStatService) : base(fileProvider) 
        {
            this.fightStatService = fightStatService;
        }

        [HttpGet("fight/stat")]
        public IActionResult Index()
        {
            return GetPage("fightStat");
        }

        [HttpPost("fight/stat")]
        public async Task<IActionResult> AddFightStat([FromServices] IUserService userService)
        {
            if(User.Identity.Name != null && User.Identity.Name != "")
            {
                var statistic = await HttpContext.Request.ReadFromJsonAsync<FightStat>();
                var user = userService.GetUser(User.Identity.Name);
                statistic.UserId = user.UserId;
                await fightStatService.AddFightStatistic(statistic);
            }
            return Ok();
        }

        [HttpGet("fight/stat/data")]
        public async Task<IActionResult> FightStatData()
        {
            var statistics = await fightStatService.GetFightStatistics();
            return Ok(statistics);
        }

        [HttpPost("fight/stat/send-email")]
        public async Task<IActionResult> SendEmail([FromServices] Emailer emailer, [FromBody] FightStatMail mail)
        {
            await emailer.SendEmail(mail.Email, mail.Message, "Fight stat");
            return Ok();
        }
    }
}
