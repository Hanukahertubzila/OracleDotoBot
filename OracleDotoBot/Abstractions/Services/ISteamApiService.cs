using OracleDotoBot.Domain.Models;

namespace OracleDotoBot.Abstractions.Services
{
    public interface ISteamApiService
    {
        Task<List<Match>> GetLiveMatches();
    }
}