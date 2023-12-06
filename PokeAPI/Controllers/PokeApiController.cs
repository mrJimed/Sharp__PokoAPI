using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using PokeAPI.Models;
using PokeAPI.Services;
using System.Security.Claims;
using System.Web;

namespace PokeAPI.Controllers
{
    public class PokeApiController : Controller
    {
        private readonly IPokeApi pokeApi;

        public PokeApiController(IPokeApi pokeApi)
        {
            this.pokeApi = pokeApi;
        }

        [HttpGet("api/pokemon/list")]
        public async Task<IActionResult> PokemonList([FromQuery] string name = "")
        {
            var pokemonList = await pokeApi.GetPokemonList();
            return Ok(pokemonList.Where(poke => poke.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase)));
        }

        [HttpGet("api/pokemon/{id:int}")]
        public async Task<IActionResult> PokemonInfo(int id)
        {
            var pokemon = await pokeApi.GetPokemonInfo(id);
            return Ok(pokemon);
        }

        [HttpGet("api/pokemon/random")]
        public async Task<IActionResult> RandomPokemon()
        {
            var pokemon = await pokeApi.GetRandomPokemon();
            return Ok(pokemon);
        }

        [HttpGet("api/pokemon/fight")]
        public async Task<IActionResult> PokemonFight([FromQuery] int myPokeId, [FromQuery] int enemyPokeId)
        {
            var myPokemon = await pokeApi.GetPokemonInfo(myPokeId);
            var enemyPokemon = await pokeApi.GetPokemonInfo(enemyPokeId);

            if (myPokemon != null && enemyPokemon != null)
                return Ok(new List<Pokemon>
                {
                    myPokemon,
                    enemyPokemon
                });
            return NotFound();
        }

        [HttpPost("api/pokemon/fight/{attackPower:int}")]
        public async Task<IActionResult> PokemonAttack(int attackPower, [FromBody] Pokemon pokemon)
        {
            pokemon = pokeApi.PokemonAttack(pokemon, attackPower);
            return Ok(pokemon);
        }

        [HttpGet("api/fight/fast")]
        public async Task<IActionResult> PokemonFastFight([FromQuery] int myPokeId, [FromQuery] int enemyPokeId)
        {
            var myPokemon = await pokeApi.GetPokemonInfo(myPokeId);
            var enemyPokemon = await pokeApi.GetPokemonInfo(enemyPokeId);

            if (myPokemon != null && enemyPokemon != null)
                return Ok(pokeApi.PokemonFastFight(myPokemon, enemyPokemon));
            return NotFound();
        }

        [Authorize]
        [HttpPost("api/ftp")]
        public async Task<IActionResult> AddFtp([FromBody] Pokemon pokeData, [FromServices] IFtpService ftpService, [FromServices] IUserService userService)
        {
            var pokemon = await pokeApi.GetPokemonInfo(pokeData.Id);
            var currentUser = userService.GetUser(User.Identity.Name);
            await ftpService.SaveMarkdownFile(currentUser.Email, currentUser.Password, pokemon);
            return Ok();
        }
    }
}
