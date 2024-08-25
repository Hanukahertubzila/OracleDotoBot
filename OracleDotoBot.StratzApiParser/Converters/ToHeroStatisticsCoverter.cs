using OracleDotoBot.Domain.Models;
using OracleDotoBot.StratzApiParser.OutputDataTypes;
using OracleDotoBot.StratzApiParser.Response_Object_Models;

namespace OracleDotoBot.StratzApiParser.Parsers
{
    internal static class ToHeroStatisticsCoverter
    {
        public static List<HeroStatistics> Covert(MatchUpStatisticsResponse response, Match match)
        {
            var stats = new List<HeroStatistics>();
            var radiantTeam = match.HeroIds.Take(match.HeroIds.Count / 2).ToList();
            var direTeam = match.HeroIds.Skip(match.HeroIds.Count / 2).ToList();
            if (radiantTeam.Count < 5 || direTeam.Count < 5)
                return null;
            GetStatsByTeam(response.Data.Stats, 
                radiantTeam, 
                direTeam, 
                stats, 0);
            GetStatsByTeam(response.Data.Stats,                
                direTeam,
                radiantTeam
                , stats, 5);
            return stats;
        }

        private static void GetStatsByTeam(HeroStatsR heroStats, List<int> team1, List<int> team2, List<HeroStatistics> stats, int matchUpOffset)
        {
            for(int i = 0; i < team1.Count; i++)
            {
                var winsVs = heroStats.MatchUp[i + matchUpOffset].Vs
                    .Where(h => h.HeroId1 == team1[i] && team2.Contains(h.HeroId2))
                    .Select(h => h.WinsAverage)
                    .Average();
                var winsWith = heroStats.MatchUp[i + matchUpOffset].With
                    .Where(h => h.HeroId1 == team1[i] && team1.Contains(h.HeroId2))
                    .Select(h => h.WinsAverage)
                    .Average();
                var heroStatistics = new HeroStatistics()
                {
                    HeroId = team1[i],
                    WinRate = heroStats.MatchUp[i + matchUpOffset].Vs
                        .First(h => h.HeroId1 == team1[i])
                        .WinRateHeroId1,
                    WinsVs = winsVs,
                    WinsWith = winsWith
                };
                stats.Add(heroStatistics);
            }
        }
    }
}