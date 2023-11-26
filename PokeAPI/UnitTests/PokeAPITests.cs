using Xunit;
using PokeAPI.Controllers;
using PokeAPI.Models;
using PokeAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest
{
    public class Tests
    {
        private IPokeApi pokeService = new PokeApi(null);


        [Fact]
        public async Task PokemonListViewResultAsync()
        {
            var pokeApiController = new PokeApiController(pokeService);
            var result = await pokeApiController.PokemonList();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var pokemonList = Assert.IsAssignableFrom<IEnumerable<Pokemon>>(okResult.Value).ToList();
            Assert.NotEmpty(pokemonList);
            Assert.True(pokemonList.Count > 0);
        }

        [Fact]
        public async Task PokemonFastFightViewResultAsync()
        {
            var pokeApiController = new PokeApiController(pokeService);
            var result = await pokeApiController.PokemonFastFight(1, 12);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var historyList = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value).ToList();
            Assert.NotEmpty(historyList);
            Assert.True(historyList.Count > 0);

            result = await pokeApiController.PokemonFastFight(-1, 12);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PokemonFightViewResultAsync()
        {
            var pokeApiController = new PokeApiController(pokeService);
            var result = await pokeApiController.PokemonFight(1, 12);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var pokemonsList = Assert.IsAssignableFrom<IEnumerable<Pokemon>>(okResult.Value).ToList();
            Assert.NotEmpty(pokemonsList);
            Assert.True(pokemonsList.Count == 2);

            result = await pokeApiController.PokemonFight(-1, 12);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PokemonInfoViewResultAsync()
        {
            var pokeApiController = new PokeApiController(pokeService);
            var result = await pokeApiController.PokemonInfo(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var pokemon = Assert.IsAssignableFrom<Pokemon>(okResult.Value);
            Assert.NotNull(pokemon);
            Assert.True(pokemon.Id == 1);

            result = await pokeApiController.PokemonInfo(1000000000);
            okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async Task RandomPokemonViewResultAsync()
        {
            var pokeApiController = new PokeApiController(pokeService);
            var result = await pokeApiController.RandomPokemon();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var pokemon = Assert.IsAssignableFrom<Pokemon>(okResult.Value);
            Assert.NotNull(pokemon);
        }

        [Fact]
        public async Task PokemonAttackViewResultAsync()
        {
            var pokeApiController = new PokeApiController(pokeService);
            var pokemon = await pokeService.GetPokemonInfo(1);
            var result = await pokeApiController.PokemonAttack(12, pokemon.Clone() as Pokemon);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatePokemon = Assert.IsAssignableFrom<Pokemon>(okResult.Value);
            Assert.Equal(updatePokemon.Hp, pokemon.Hp - 12);
        }
    }
}
