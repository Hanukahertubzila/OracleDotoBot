using Microsoft.Extensions.Logging;
using OracleDotoBot.Abstractions.Services;
using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;
using OracleDotoBot.SteamApi.Api;

namespace OracleDotoBot.Services
{
    public class SteamApiService : ISteamApiService
    {
        public SteamApiService(string token, List<Hero> heroes, ILogger<ISteamApiService> logger)
        {
            _client = new SteamDotaApi(token, heroes);
            _logger = logger;
        }

        private readonly SteamDotaApi _client;
        private readonly ILogger<ISteamApiService> _logger;

        public async Task<List<Match>> GetLiveMatches()
        {
            var matches = await _client.GetLiveMatches();

            if (!string.IsNullOrEmpty(matches.error))
            {
                _logger.LogError(matches.error);
                return new List<Match>();
            }
            if (matches.data.Count > 7)
                return matches.data.Take(7).ToList();
            return matches.data;
        }
    }
}
