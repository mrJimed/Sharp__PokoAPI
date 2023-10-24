using Microsoft.EntityFrameworkCore;
using PokeAPI.Models;
using PokeAPI.Services.DbContexts;

namespace PokeAPI.Services
{
    public class FightStatService : IFightStatService
    {
        private PokeDbContext pokeDb;

        public FightStatService(PokeDbContext pokeDb)
        {
            this.pokeDb = pokeDb;
        }

        public async Task AddFightStatistic(FightStat statistic)
        {
            await pokeDb.Statistics.AddAsync(statistic);
            await pokeDb.SaveChangesAsync();
        }

        public async Task<List<FightStat>> GetFightStatistics()
        {
            var statistics = await pokeDb.Statistics.ToListAsync();
            return statistics;
        }
    }
}
