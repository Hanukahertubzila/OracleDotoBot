using OracleDotoBot.RequestModels;

namespace OracleDotoBot.Abstractions
{
    public interface IStratzApiService
    {
        Task<string> GetMatchUpStatistics(Match match);
    }
}