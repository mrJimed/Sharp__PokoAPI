using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using PokeAPI.Services;
using System.Diagnostics;
using System.Web;

namespace PokeAPI.Controllers
{
    public class PageController : AbstractPageController
    {
        public PageController(IFileProvider fileProvider) : base(fileProvider)
        {

        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return GetPage("pokeList");
        }

        [HttpGet("/pokemon/{id:int}")]
        public IActionResult PokemonInfo(int id)
        {
            return GetPage("pokeInfo");
        }

        [HttpGet("pokemon/fight")]
        public IActionResult PokemonFight()
        {
            return GetPage("pokeFight");
        }
    }
}