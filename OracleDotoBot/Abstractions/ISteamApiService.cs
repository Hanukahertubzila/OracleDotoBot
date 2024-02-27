using OracleDotoBot.Domain.Models;

namespace OracleDotoBot.Abstractions
{
    public interface ISteamApiService
    {
        Task<List<Match>> GetLiveMatches();
    }
}