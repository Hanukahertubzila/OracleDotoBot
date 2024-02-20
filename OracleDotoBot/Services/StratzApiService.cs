using Microsoft.Extensions.Logging;
using OracleDotoBot.Abstractions;
using OracleDotoBot.Domain.Models;
using OracleDotoBot.StratzApiParser.Api;
using OracleDotoBot.StratzApiParser.OutputDataTypes;

namespace OracleDotoBot.Services
{
    public class StratzApiService : IStratzApiService
    {
        public StratzApiService(string baseUrl, string stratzToken, 
            ILogger<IStratzApiService> logger)
        {
            _apiClient = new DotaApi(baseUrl, stratzToken);
            _logger = logger;
        }

        private readonly DotaApi _apiClient;
        private readonly ILogger<IStratzApiService> _logger;

        public async Task<string> GetMatchUpStatistics(Match match)
        {
            var stats = await _apiClient.GetMatchUpStatistics(match);
            if (!string.IsNullOrEmpty(stats.error))
            {
                _logger.LogError(stats.error);
                return stats.error;
            }
            var responseStatistics = GetMatchupStatisticsString(stats.stats);
            var laning = await _apiClient.GetLaningStatistics(match);
            if (!string.IsNullOrEmpty(laning.error))
            {
                _logger.LogError(laning.error);
                return responseStatistics + laning.error;
            }

            responseStatistics += "WinCount: " + laning.stats.Mid.WinCount + 
                " LossCount: " + laning.stats.Mid.LossCount + 
                " DrawCount: " + laning.stats.Mid.DrawCount;

            responseStatistics += "WinCount: " + laning.stats.CarryOfflane.WinCount +
                " LossCount: " + laning.stats.CarryOfflane.LossCount +
                " DrawCount: " + laning.stats.CarryOfflane.DrawCount;

            responseStatistics += "WinCount: " + laning.stats.Supp5Supp4.WinCount +
                " LossCount: " + laning.stats.Supp5Supp4.LossCount +
                " DrawCount: " + laning.stats.Supp5Supp4.DrawCount;

            responseStatistics += "WinCount: " + laning.stats.OfflaneCarry.WinCount +
                " LossCount: " + laning.stats.OfflaneCarry.LossCount +
                " DrawCount: " + laning.stats.OfflaneCarry.DrawCount;

            responseStatistics += "WinCount: " + laning.stats.Mid.WinCount +
                " LossCount: " + laning.stats.Supp4Supp5.LossCount +
                " DrawCount: " + laning.stats.Supp4Supp5.DrawCount;

            return responseStatistics;
        }

        private string GetMatchupStatisticsString(List<HeroStatistics> stats)
        {
            var radiantWinrate = Math.Round(stats
                .Select(h => h.WinRate)
                .Take(stats.Count / 2)
                .Sum() / (stats.Count / 2) * 100, 2);
            var direWinrate = Math.Round(stats
                .Select(h => h.WinRate)
                .Skip(stats.Count / 2)
                .Sum() / (stats.Count / 2) * 100, 2);

            var radiantMatchupVsWinrate = Math.Round(stats
                .Select(h => h.WinsVs)
                .Take(stats.Count / 2)
                .Sum() / (stats.Count / 2) * 100, 2);
            var direMatchupVsWinrate = Math.Round(stats
                .Select(h => h.WinsVs)
                .Skip(stats.Count / 2)
                .Sum() / (stats.Count / 2) * 100, 2);

            var radiantMatchupWithWinrate = Math.Round(stats
                .Select(h => h.WinsWith)
                .Take(stats.Count / 2)
                .Sum() / (stats.Count / 2) * 100, 2);
            var direMatchupWithWinrate = Math.Round(stats
                .Select(h => h.WinsWith)
                .Skip(stats.Count / 2)
                .Sum() / (stats.Count / 2) * 100, 2);

            var statistics = @$"Свет метовость драфта: { radiantWinrate }%; 
Тьма метовость драфта: { direWinrate }%;
Свет  вирнейт против драфта Тьмы: { radiantMatchupVsWinrate }%;
Тьма вирнейт против драфта Света: { direMatchupVsWinrate }%;
Свет  синергия драфта: { radiantMatchupWithWinrate }%;
Тьма синергия драфта: { direMatchupWithWinrate }%;";

            return statistics;
        }
    }
}
