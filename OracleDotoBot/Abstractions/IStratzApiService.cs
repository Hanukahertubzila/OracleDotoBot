using OracleDotoBot.Domain.Models;

namespace OracleDotoBot.Abstractions
{
    public interface IStratzApiService
    {
        Task<string> GetStatisticsString(Match match);
    }
}