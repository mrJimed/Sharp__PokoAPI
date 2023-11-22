using Moq;
using Xunit;
using PokeAPI.Controllers;
using PokeAPI.Models;
using PokeAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Sprache;

namespace UnitTest
{
    public class Tests
    {
        [Fact]
        public async Task PokemonListViewResultAsync()
        {
            var expectedPokemonList = GetPokemonListTest();
            var mock = new Mock<IPokeApi>();
            mock.Setup(pokeApi => pokeApi.GetPokemonList()).ReturnsAsync(expectedPokemonList);

            var pokeApiController = new PokeApiController(mock.Object);
            var result = await pokeApiController.PokemonList();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var pokemonList = Assert.IsAssignableFrom<IEnumerable<Pokemon>>(okResult.Value).ToList();

            Assert.Equal(expectedPokemonList.Count, pokemonList.Count);
        }

        [Fact]
        public async Task PokemonFastFightViewResultAsync()
        {
            var myPokeId = 1;
            var enemyPokeId = 4;

            var pokeList = GetPokemonListTest();
            var mock = new Mock<IPokeApi>();
            mock.Setup(pokeApi => pokeApi.GetPokemonInfo(myPokeId)).ReturnsAsync((int id) => pokeList.FirstOrDefault(poke => poke.Id == id));
            mock.Setup(pokeApi => pokeApi.GetPokemonInfo(enemyPokeId)).ReturnsAsync((int id) => pokeList.FirstOrDefault(poke => poke.Id == id));

            var pokeApiController = new PokeApiController(mock.Object);
            var result = await pokeApiController.PokemonFastFight(myPokeId, enemyPokeId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PokemonFightViewResultAsync()
        {
            var myPokeId = 1;
            var enemyPokeId = 3;
            var pokeList = GetPokemonListTest();

            var mock = new Mock<IPokeApi>();
            mock.Setup(pokeApi => pokeApi.GetPokemonInfo(myPokeId)).ReturnsAsync((int id) => pokeList.FirstOrDefault(poke => poke.Id == id));
            mock.Setup(pokeApi => pokeApi.GetPokemonInfo(enemyPokeId)).ReturnsAsync((int id) => pokeList.FirstOrDefault(poke => poke.Id == id));

            var pokeApiController = new PokeApiController(mock.Object);
            var result = await pokeApiController.PokemonFight(myPokeId, enemyPokeId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PokemonInfoViewResultAsync()
        {
            var pokeId = 1;
            var pokeList = GetPokemonListTest();
            var mock = new Mock<IPokeApi>();
            mock.Setup(pokeApi => pokeApi.GetPokemonInfo(pokeId)).ReturnsAsync((int id) => pokeList.First(poke => poke.Id == id));

            var pokeApiController = new PokeApiController(mock.Object);
            var result = await pokeApiController.PokemonInfo(pokeId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var pokemon = Assert.IsAssignableFrom<Pokemon>(okResult.Value);

            Assert.Equal(pokemon, pokeList.First(poke => poke.Id == pokeId));
        }

        [Fact]
        public async Task RandomPokemonViewResultAsync()
        {
            var pokeList = GetPokemonListTest();
            var minId = pokeList.Select(p => p.Id).Min();
            var maxId = pokeList.Select(p => p.Id).Max();
            var randId = new Random().Next(minId, maxId + 1);

            var mock = new Mock<IPokeApi>();
            mock.Setup(pokeApi => pokeApi.GetRandomPokemon()).ReturnsAsync(() => pokeList.FirstOrDefault(poke => poke.Id == randId));

            var pokeApiController = new PokeApiController(mock.Object);
            var result = await pokeApiController.RandomPokemon();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var pokemon = Assert.IsAssignableFrom<Pokemon>(okResult.Value);

            Assert.NotNull(pokemon);
            Assert.Equal(pokemon, pokeList.First(poke => poke.Id == randId));
        }

        [Fact]
        public async Task PokemonAttackViewResultAsync()
        {
            var attackPower = 12;
            var pokemon = GetPokemonListTest().First();

            var mock = new Mock<IPokeApi>();
            mock.Setup(pokeApi => pokeApi.PokemonAttack(pokemon, attackPower)).Returns((Pokemon p, int power) => new Pokemon()
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Hp = pokemon.Hp - attackPower,
                Weight = pokemon.Weight,
                Height = pokemon.Height,
                AttackPower = pokemon.AttackPower,
                Image = pokemon.Image
            });

            var pokeApiController = new PokeApiController(mock.Object);
            var result = await pokeApiController.PokemonAttack(attackPower, pokemon);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedPokemon = Assert.IsAssignableFrom<Pokemon>(okResult.Value);

            Assert.Equal(pokemon.Hp - attackPower, updatedPokemon.Hp);
        }

        private List<Pokemon> GetPokemonListTest()
        {
            return new List<Pokemon>()
            {
                new()
                {
                    Id = 1,
                    Name = "bulbasaur",
                    AttackPower = 12,
                    Hp = 100,
                    Height = 12,
                    Weight = 13,
                    Image = ""
                },
                new()
                {
                    Id = 2,
                    Name = "ditto",
                    AttackPower = 13,
                    Hp = 50,
                    Height = 12,
                    Weight = 13,
                    Image = ""
                }
            };
        }
    }
}
