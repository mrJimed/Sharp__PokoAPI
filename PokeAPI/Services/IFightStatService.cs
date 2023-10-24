using PokeAPI.Models;

namespace PokeAPI.Services
{
    public interface IFightStatService
    {
        Task AddFightStatistic(FightStat statistic);

        Task<List<FightStat>> GetFightStatistics();
    }
}
