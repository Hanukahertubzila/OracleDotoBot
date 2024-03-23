using Microsoft.Extensions.Options;
using OracleDotoBot.Abstractions.Services;
using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;
using OracleDotoBot.StratzApi.OutputDataTypes;
using OracleDotoBot.StratzApiParser.OutputDataTypes;

namespace OracleDotoBot.Services
{
    public class MatchAnaliticsService : IMatchAnaliticsService
    {
        public MatchAnaliticsService(IStratzApiService stratzApiService,
            IOptions<List<Hero>> heroes)
        {
            _stratzApiService = stratzApiService;
            _heroes = heroes.Value;
        }

        private readonly IStratzApiService _stratzApiService;
        private readonly List<Hero> _heroes;

        public async Task<string> GetMatchAnalitics(Match match, bool includeMatchUp, bool includeLaning, bool includePlayerPerformance)
        {
            var analitics = "";
            var matchups = new List<HeroStatistics>();
            var laning = default(LaningStatistics);
            var playerPerformance = new List<PlayerPerformance>();
            if (includePlayerPerformance)
            {
                playerPerformance = await _stratzApiService.GetPlayerPerformance(match);
            }
            if (includeMatchUp)
            {
                matchups = await _stratzApiService.GetMatchupStatistics(match);
                analitics += GetBriefMatchupStatisticsString(matchups, match);
            }
            if (includeLaning)
            {
                laning = await _stratzApiService.GetLaningStatistics(match);
                analitics += GetBriefLaningStatisticsString(laning);
            }
            analitics += "\n\n*ПОДРОБНЕЕ: *\n";
            if (includeMatchUp)
            {
                analitics += GetFullMatchupStatisticsString(matchups, playerPerformance, includePlayerPerformance);
            }
            if (includeLaning)
            {
                analitics += GetFullLaningStatisticsString(laning);
            }
            return analitics;
        }

        private string GetBriefMatchupStatisticsString(List<HeroStatistics> stats, Match match)
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

            var strongerDraft = string.Empty;
            if (!string.IsNullOrEmpty(match.RadiantTeam.Name) && !string.IsNullOrEmpty(match.DireTeam.Name))
                strongerDraft = radiantPower > direPower ? $"{match.RadiantTeam.Name} (свет) " 
                    : $"{match.DireTeam.Name} (тьма) ";
            else
                strongerDraft = radiantPower > direPower ? "СВЕТА" : "ТЬМЫ";

            var statistics = @$"*ДРАФТ {strongerDraft} СИЛЬНЕЕ*

*Метовость драфта: *
*Свет: *{radiantWinrate}% *Тьма: *{direWinrate}%; 
*Винрейт против драфта противника: *
*Свет: *{radiantMatchupVsWinrate}% *Тьма: *{direMatchupVsWinrate}%;
*Синергия драфта: *
*Свет: *{radiantMatchupWithWinrate}% *Тьма: *{direMatchupWithWinrate}%;";

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

        private string GetFullMatchupStatisticsString(List<HeroStatistics> stats, List<PlayerPerformance> performances, bool includePlayerPerformance)
        {
            var statistics = "\n*Матч апы: *\n";

            foreach (var hero in stats)
            {
                var heroStats = "\n*" + _heroes.First(h => h.Id == hero.HeroId).LocalizedName + "*";
                heroStats += "\nВинрейт в патче: " + Math.Round(hero.WinRate * 100, 2) + "%";
                heroStats += "\nСинергия героя: " + Math.Round(hero.WinsWith * 100, 2) + "%";
                heroStats += "\nХорошая игра для героя на: " + Math.Round(hero.WinsVs * 100, 2) + "%";
                if (includePlayerPerformance)
                {
                    var p = performances.FirstOrDefault(p => p.HeroId == hero.HeroId);
                    if (p != null)
                    {
                        heroStats += "\nКоличество матчей игрока на герое: " + p.TotalMatchCount;
                        if (p.TotalMatchCount > 0)
                            heroStats += "\nВинрейт игрока на герое: " 
                                + Math.Round((double)p.WinMatchCount / p.TotalMatchCount * 100, 2) + "%\n";
                        else
                            heroStats += "\n";
                    }
                }
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
