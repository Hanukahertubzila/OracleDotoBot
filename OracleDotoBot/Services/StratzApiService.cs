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
            _apiClient = new DotaApi(baseUrl, stratzToken, heroes);
            _logger = logger;
            _heroes = heroes;
        }

        private readonly DotaApi _apiClient;
        private readonly ILogger<IStratzApiService> _logger;
        private readonly List<Hero> _heroes;

        public async Task<List<Match>> GetLiveMatches()
        {
            var matches = await _apiClient.GetLiveMatches();

            if (!string.IsNullOrEmpty(matches.error))
                _logger.LogError(matches.error);

            return matches.matches;
        }

        public async Task<string> GetStatisticsString(Match match)
        {
            var responseStatistics = "*БРИФ: *\n\n";
            var stats = await _apiClient.GetMatchUpStatistics(match);
            if (!string.IsNullOrEmpty(stats.error))
            {
                _logger.LogError(stats.error);
                return stats.error;
            }

            responseStatistics += GetBriefMatchupStatisticsString(stats.stats);

            var laning = await _apiClient.GetLaningStatistics(match);
            if (!string.IsNullOrEmpty(laning.error))
            {
                _logger.LogError(laning.error);
                return responseStatistics + laning.error;
            }

            responseStatistics += GetBriefLaningStatisticsString(laning.stats);

            responseStatistics += "\n*ПОДРОБНЕЕ: *\n";

            responseStatistics += GetFullMatchupStatisticsString(stats.stats);

            responseStatistics += GetFullLaningStatisticsString(laning.stats);

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

            var radiantPower = radiantWinrate / 2 + radiantMatchupWithWinrate + radiantMatchupVsWinrate;
            var direPower = direWinrate / 2 + direMatchupWithWinrate + direMatchupVsWinrate;

            var strongerDraft = radiantPower > direPower ? "СВЕТА" : "ТЬМЫ";

            var statistics = @$"*ДРАФТ СИЛ { strongerDraft } СИЛЬНЕЕ*

*Метовость драфта: *
*Свет: *{ radiantWinrate }% *Тьма: *{direWinrate}%; 
*Винрейт против драфта противника: *
*Свет: *{radiantMatchupVsWinrate}% *Тьма: *{ direMatchupVsWinrate }%;
*Синергия драфта: *
*Свет: *{ radiantMatchupWithWinrate }% *Тьма: *{ direMatchupWithWinrate }%;";

            return statistics;
        }

        private string GetBriefLaningStatisticsString(LaningStatistics stats)
        {
            var statistics = "\n*Лейнинг: *\n";

            statistics += GetBriefLaneResult(stats.Mid, "*миде*") + "\n";

            statistics += GetBriefLaneResult(stats.EasyLane, "*боте*") + "\n";

            statistics += GetBriefLaneResult(stats.HardLane, "*топе*") + "\n";

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

        private string GetFullMatchupStatisticsString(List<HeroStatistics> stats)
        {
            var statistics = "\n*Матч апы: *\n";

            foreach (var hero in stats)
            {
                var heroStats = "\n*" + _heroes.First(h => h.Id == hero.HeroId).LocalizedName + "*";
                heroStats += "\nВинрейт в патче: " + Math.Round(hero.WinRate * 100, 2) + "%";
                heroStats += "\nСинергия героя: " + Math.Round(hero.WinsWith * 100, 2) + "%";
                heroStats += "\nХорошая игра для героя на: " + Math.Round(hero.WinsVs * 100, 2) + "%\n";
                statistics += heroStats;
            }
            return statistics;
        }

        private string GetFullLaningStatisticsString(LaningStatistics stats)
        {
            var statistics = "\n*Лейнинг: *\n";

            statistics += GetFullLaneResult(stats.Mid, "миде");
            statistics += GetFullLaneResult(stats.EasyLane, "боте");
            statistics += GetFullLaneResult(stats.HardLane, "топе");

            return statistics;
        }

        private string GetFullLaneResult(LaningWDL wdl, string lane)
        {
            var wdlSum = wdl.WinCount + wdl.DrawCount + wdl.LossCount;
            var laneResult = "*Свет* выигрывает в " + lane + " в " + 
                Math.Round((double)wdl.WinCount / wdlSum * 100, 2) + "%\n";
            laneResult += "*Ничья* происходит в " + lane + " в " +
                Math.Round((double)wdl.DrawCount / wdlSum * 100, 2) + "%\n";
            laneResult += "*Тьма* выигрывает в " + lane + " в " +
                Math.Round((double)wdl.LossCount / wdlSum * 100, 2) + "%\n\n";
            return laneResult;
        }
    }
}
