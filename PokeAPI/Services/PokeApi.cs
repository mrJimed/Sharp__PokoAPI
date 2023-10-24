using Newtonsoft.Json.Linq;
using PokeAPI.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PokeAPI.Services
{
    public class PokeApi : IPokeApi
    {
        private const string API_URL = "https://pokeapi.co/api/v2/pokemon";

        private async Task<int> GetPokemonsCount()
        {
            int count = 0;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(API_URL);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pokeData = JObject.Parse(content);
                    count = int.Parse((string)pokeData["count"]);
                }
            }
            return count;
        }

        private int GetId(string url)
        {
            var pattern = @"/(\d+)/$";
            var regex = new Regex(pattern);
            var match = regex.Match(url);

            if (match.Success)
                return int.Parse(match.Groups[1].Value);
            return -1;
        }

        public async Task<List<Pokemon>> GetPokemonList()
        {
            var limit = await GetPokemonsCount();
            var pokemons = new List<Pokemon>();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{API_URL}?limit={limit}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pokeData = JObject.Parse(content);
                    foreach (var poke in pokeData["results"])
                    {
                        pokemons.Add(new Pokemon()
                        {
                            Id = GetId((string)poke["url"]),
                            Name = (string)poke["name"]
                        });
                    }
                }
            }
            return pokemons;
        }

        public async Task<Pokemon> GetPokemonInfo(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{API_URL}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pokemon = JObject.Parse(content);
                    return new Pokemon()
                    {
                        Id = int.Parse((string)pokemon["id"]),
                        Name = (string)pokemon["name"],
                        Hp = int.Parse((string)pokemon["stats"][0]["base_stat"]),
                        AttackPower = int.Parse((string)pokemon["stats"][1]["base_stat"]),
                        Height = int.Parse((string)pokemon["height"]),
                        Weight = int.Parse((string)pokemon["weight"]),
                        Image = (string)pokemon["sprites"]["other"]["official-artwork"]["front_default"]
                    };
                }
                return null;
            }
        }

        public async Task<Pokemon> GetRandomPokemon()
        {
            var pokemons = await GetPokemonList();
            var rnd = new Random();
            return pokemons[rnd.Next(pokemons.Count)];
        }

        public Pokemon PokemonAttack(Pokemon pokemon, int attackPower)
        {
            pokemon.Hp = Math.Max(0, pokemon.Hp - attackPower);
            return pokemon;
        }

        private bool PokemonFastAttack(Pokemon attackingPoke, Pokemon attackedPoke)
        {
            attackedPoke = PokemonAttack(attackedPoke, attackingPoke.AttackPower);
            return attackedPoke.Hp == 0;
        }

        public List<object> PokemonFastFight(Pokemon myPoke, Pokemon enemyPoke)
        {
            var statistics = new List<object>();
            var rnd = new Random();
            int round = 0;
            bool isEndFight = false;

            while (!isEndFight)
            {
                round++;
                int myNomber = rnd.Next(1, 10);
                int enemyNumber = rnd.Next(1, 10);

                if(myNomber % 2 == 0 && enemyNumber % 2 == 0 || myNomber % 2 != 0 && enemyNumber % 2 != 0)
                {
                    isEndFight = PokemonFastAttack(myPoke, enemyPoke);
                    statistics.Add(new
                    {
                        AttackingPoke = myPoke.Clone(),
                        AttackedPoke = enemyPoke.Clone(),
                        Round = round,
                        IsMyPokeWin = true
                    });
                }
                else
                {
                    isEndFight = PokemonFastAttack(enemyPoke, myPoke);
                    statistics.Add(new
                    {
                        AttackingPoke = enemyPoke.Clone(),
                        AttackedPoke = myPoke.Clone(),
                        Round = round,
                        IsMyPokeWin = false
                    });
                }
            }
            return statistics;
        }
    }
}
