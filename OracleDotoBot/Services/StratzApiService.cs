using Microsoft.Extensions.Logging;
using OracleDotoBot.Abstractions.Services;
using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;
using OracleDotoBot.StratzApi.OutputDataTypes;
using OracleDotoBot.StratzApiParser.Api;
using OracleDotoBot.StratzApiParser.OutputDataTypes;

namespace OracleDotoBot.Services
{
    public class StratzApiService : IStratzApiService
    {
        public StratzApiService(string baseUrl, string stratzToken, 
            ILogger<IStratzApiService> logger, List<Hero> heroes)
        {
            _apiClient = new StratzDotaApi(baseUrl, stratzToken, heroes);
            _logger = logger;
        }

        private readonly StratzDotaApi _apiClient;
        private readonly ILogger<IStratzApiService> _logger;

        public async Task<Match?> GetMatchById(long id)
        {
            var match = await _apiClient.GetMatchById(id);
            return match.match;
        }

        public async Task<List<PlayerPerformance>> GetPlayerPerformance(Match match)
        {
            var matchWithPerformance = await _apiClient.GetPlayerHeroPerformance(match);
            if (!string.IsNullOrEmpty(matchWithPerformance.error))
            {
                _logger.LogError(matchWithPerformance.error);
                return matchWithPerformance.match;
            }
            return matchWithPerformance.match;
        }

        public async Task<List<HeroStatistics>> GetMatchupStatistics(Match match)
        {
            var stats = await _apiClient.GetMatchUpStatistics(match);
            if (!string.IsNullOrEmpty(stats.error))
            {
                _logger.LogError(stats.error);
                return new List<HeroStatistics>();
            }
            return stats.stats;
        }

        public async Task<LaningStatistics?> GetLaningStatistics(Match match)
        {
            var laning = await _apiClient.GetLaningStatistics(match);
            if (!string.IsNullOrEmpty(laning.error))
            {
                _logger.LogError(laning.error);
                return null;
            }
            return laning.stats;
        }
    }
}
