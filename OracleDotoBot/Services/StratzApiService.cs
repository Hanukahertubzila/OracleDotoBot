using Microsoft.Extensions.Logging;
using OracleDotoBot.Abstractions;
using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;
using OracleDotoBot.StratzApiParser.Api;
using OracleDotoBot.StratzApiParser.OutputDataTypes;

namespace OracleDotoBot.Services
{
    public class StratzApiService : IStratzApiService
    {
        public StratzApiService(string baseUrl, string stratzToken, 
            ILogger<IStratzApiService> logger,
            List<Hero> heroes)
        {
            _apiClient = new StratzDotaApi(baseUrl, stratzToken, heroes);
            _logger = logger;
        }

        private readonly StratzDotaApi _apiClient;
        private readonly ILogger<IStratzApiService> _logger;


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
