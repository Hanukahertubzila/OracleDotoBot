using OracleDotoBot.Domain.Models;
using OracleDotoBot.StratzApi.OutputDataTypes;
using OracleDotoBot.StratzApiParser.OutputDataTypes;

namespace OracleDotoBot.Abstractions
{
    public interface IStratzApiService
    {
        Task<Match?> GetMatchById(long id);

        Task<LaningStatistics?> GetLaningStatistics(Match match);

        Task<List<HeroStatistics>> GetMatchupStatistics(Match match);

        Task<List<PlayerPerformance>> GetPlayerPerformance(Match match);
    }
}