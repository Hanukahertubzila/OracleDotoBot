using OracleDotoBot.Domain.Models;
using OracleDotoBot.StratzApiParser.OutputDataTypes;

namespace OracleDotoBot.Abstractions
{
    public interface IStratzApiService
    {
        Task<LaningStatistics?> GetLaningStatistics(Match match);

        Task<List<HeroStatistics>> GetMatchupStatistics(Match match);
    }
}