using PokeAPI.Models;

namespace PokeAPI.Services
{
    public interface IPokeApi
    {
        Task<Pokemon> GetPokemonInfo(int id);
        Task<List<Pokemon>> GetPokemonList();
        Task<Pokemon> GetRandomPokemon();
        Pokemon PokemonAttack(Pokemon pokemon, int attackPower);
        List<object> PokemonFastFight(Pokemon myPoke, Pokemon enemyPoke);
    }
}
