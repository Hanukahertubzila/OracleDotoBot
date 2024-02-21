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

        public async Task<string> GetStatisticsString(Match match)
        {
            var stats = await _apiClient.GetMatchUpStatistics(match);
            if (!string.IsNullOrEmpty(stats.error))
            {
                _logger.LogError(stats.error);
                return stats.error;
            }
            var responseStatistics = GetBriefMatchupStatisticsString(stats.stats);

            var laning = await _apiClient.GetLaningStatistics(match);
            if (!string.IsNullOrEmpty(laning.error))
            {
                _logger.LogError(laning.error);
                return responseStatistics + laning.error;
            }

            responseStatistics += GetLaningStatisticsString(laning.stats);

            return responseStatistics;
        }

        private string GetBriefMatchupStatisticsString(List<HeroStatistics> stats)
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

            var statistics = @$"*Метовость драфта: *
*Свет: *{ radiantWinrate }% *Тьма: *{direWinrate}%; 
*Винрейт против драфта противника: *
*Свет: *{radiantMatchupVsWinrate}% *Тьма: *{ direMatchupVsWinrate }%;
*Синергия драфта: *
*Свет: *{ radiantMatchupWithWinrate }% *Тьма: *{ direMatchupWithWinrate }%;";

            return statistics;
        }

        private string GetLaningStatisticsString(LaningStatistics stats)
        {
            var statistics = "\n*Лейнинг: *\n";

            statistics += GetBriefLaneResult(stats.Mid, "Миде") + "\n";

            statistics += GetBriefLaneResult(stats.EasyLane, "Боте") + "\n";

            statistics += GetBriefLaneResult(stats.HardLane, "Топе") + "\n";

            return statistics;
        }

        private string GetBriefLaneResult(LaningWDL wdl, string lane)
        {
            if (wdl.WinCount == wdl.DrawCount && wdl.WinCount == wdl.LossCount)
                return "Cилы команд примерно равны (обе команды могут как выиграть, проиграть, так и отстоять ровно " + lane;

            var sum = wdl.WinCount + wdl.DrawCount + wdl.LossCount;
            var max = Math.Max(wdl.DrawCount, Math.Max(wdl.WinCount, wdl.LossCount));

            var strengthDiff = GetLaneStrengthDiff(max, sum);

            if (max == wdl.WinCount)
                return "Силы Света " + strengthDiff + " на " + lane;
            else if (max == wdl.DrawCount)
                return "Вероятность постоять ровно " + strengthDiff + " на " + lane;
            return "Силы Тьмы " + strengthDiff + " на " + lane;
        }

        private string GetLaneStrengthDiff(int max, int sum)
        {
            var diff = Math.Round((double)max / sum * 100, 2) - 33.3d;
            if (diff <= 5)
                return "*НЕМНОГО СИЛЬНЕЕ* (отклонение не более 5%)";
            if (diff <= 15)
                return "*СИЛЬНЕЕ* (отклонение не более 15%)";
            return "*НАМНОГО СИЛЬНЕЕ* (отклонение более 15%)";
        }
    }
}
