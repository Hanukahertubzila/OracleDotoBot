using OracleDotoBot.Domain.Models;
using OracleDotoBot.StratzApiParser.OutputDataTypes;
using OracleDotoBot.StratzApiParser.ResponseObjectModels;

namespace OracleDotoBot.StratzApiParser.Converters
{
    public static class ToLaningStatisticsConverter
    {
        public static (LaningStatistics? stats, string error) Covert(LaningStatisticsResponse response, Match match)
        {
            var midWDL = GetLaningWDL(response.MidVsMid, match.DireTeam.Pos2HeroId);
            var carryOfflaneWDL = GetLaningWDL(response.CarryVsOfflane, match.DireTeam.Pos3HeroId);
            var supp5Supp4WDL = GetLaningWDL(response.Supp5VsSupp4, match.DireTeam.Pos4HeroId);
            var offlaneCarryWDL = GetLaningWDL(response.OfflaneVsCarry, match.DireTeam.Pos1HeroId);
            var supp4Supp5WDL = GetLaningWDL(response.Supp4VsSupp5, match.DireTeam.Pos5HeroId);
            
            if (!string.IsNullOrEmpty(midWDL.error))
                return (null, midWDL.error);

            if (!string.IsNullOrEmpty(carryOfflaneWDL.error))
                return (null, carryOfflaneWDL.error);

            if (!string.IsNullOrEmpty(supp5Supp4WDL.error))
                return (null, supp5Supp4WDL.error);

            if (!string.IsNullOrEmpty(offlaneCarryWDL.error))
                return (null, offlaneCarryWDL.error);

            if (!string.IsNullOrEmpty(supp4Supp5WDL.error))
                return (null, supp4Supp5WDL.error);

            var laningStatistics = new LaningStatistics()
            {
                Mid = midWDL.wdl,
                CarryOfflane = carryOfflaneWDL.wdl,
                Supp5Supp4 = supp5Supp4WDL.wdl,
                OfflaneCarry = offlaneCarryWDL.wdl,
                Supp4Supp5 = supp4Supp5WDL.wdl
            };

            return (laningStatistics, "");
        }

        private static (LaningWDL? wdl, string error) GetLaningWDL(LaningData data, int heroId)
        {
            var stats = data.Data.HeroStats.LaneOutcome
                .FirstOrDefault(h => h.HeroId2 == heroId);
            if (stats == null)
                return (null, "laning not found for " + heroId);
            return (new LaningWDL()
            {
                WinCount = stats.WinCount + stats.StompWinCount * 2,
                DrawCount = stats.DrawCount,
                LossCount = stats.LossCount + stats.StompLossCount * 2
            }, "");
        }
    }
}
