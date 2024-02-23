using OracleDotoBot.Domain.Models;
using OracleDotoBot.StratzApiParser.OutputDataTypes;
using OracleDotoBot.StratzApiParser.ResponseObjectModels;

namespace OracleDotoBot.StratzApiParser.Converters
{
    public static class ToLaningStatisticsConverter
    {
        public static (LaningStatistics? stats, string error) Covert(LaningStatisticsResponse response, Match match)
        {
            var midWDL = GetLaningWDL(response.MidVsMid, match.DireTeam.Pos2.Hero.Id, false);
            var carryVsOfflaneWDL = GetLaningWDL(response.CarryVsOfflane, match.DireTeam.Pos3.Hero.Id, false);
            var carryVsSupp4WDL = GetLaningWDL(response.CarryVsSupp4, match.DireTeam.Pos4.Hero.Id, false);
            var supp5VsSupp4WDL = GetLaningWDL(response.Supp5VsSupp4, match.DireTeam.Pos4.Hero.Id, false);
            var supp5VsOfflaneWDL = GetLaningWDL(response.Supp5VsOfflane, match.DireTeam.Pos3.Hero.Id, false);
            var offlaneVsCarryWDL = GetLaningWDL(response.OfflaneVsCarry, match.DireTeam.Pos1.Hero.Id, false);
            var offlaneVsSupp5WDL = GetLaningWDL(response.OfflaneVsSupp5, match.DireTeam.Pos5.Hero.Id, false);
            var supp4VsSupp5WDL = GetLaningWDL(response.Supp4VsSupp5, match.DireTeam.Pos5.Hero.Id, false);
            var supp4VsCarryWDL = GetLaningWDL(response.Supp4VsCarry, match.DireTeam.Pos1.Hero.Id, false);
            var radiantCarrySupp5WDL = GetLaningWDL(response.RadiantCarrySupp5, match.RadiantTeam.Pos5.Hero.Id, false);
            var direCarrySupp5WDL = GetLaningWDL(response.DireCarrySupp5, match.DireTeam.Pos5.Hero.Id, true);
            var radiantOfflaneSupp4WDL = GetLaningWDL(response.RadiantOfflaneSupp4, match.RadiantTeam.Pos4.Hero.Id, false);
            var direOfflaneSupp4WDL = GetLaningWDL(response.DireOfflaneSupp4, match.DireTeam.Pos4.Hero.Id, true);

            if (!string.IsNullOrEmpty(midWDL.error))
                return (null, midWDL.error);

            if (!string.IsNullOrEmpty(carryVsOfflaneWDL.error))
                return (null, carryVsOfflaneWDL.error);

            if (!string.IsNullOrEmpty(carryVsSupp4WDL.error))
                return (null, carryVsSupp4WDL.error);

            if (!string.IsNullOrEmpty(supp5VsSupp4WDL.error))
                return (null, supp5VsSupp4WDL.error);

            if (!string.IsNullOrEmpty(supp5VsOfflaneWDL.error))
                return (null, supp5VsOfflaneWDL.error);

            if (!string.IsNullOrEmpty(offlaneVsCarryWDL.error))
                return (null, offlaneVsCarryWDL.error);

            if (!string.IsNullOrEmpty(offlaneVsCarryWDL.error))
                return (null, offlaneVsCarryWDL.error);

            if (!string.IsNullOrEmpty(supp4VsSupp5WDL.error))
                return (null, supp4VsSupp5WDL.error);

            if (!string.IsNullOrEmpty(radiantCarrySupp5WDL.error))
                return (null, radiantCarrySupp5WDL.error);

            if (!string.IsNullOrEmpty(direCarrySupp5WDL.error))
                return (null, direCarrySupp5WDL.error);

            if (!string.IsNullOrEmpty(radiantOfflaneSupp4WDL.error))
                return (null, radiantOfflaneSupp4WDL.error);

            if (!string.IsNullOrEmpty(direOfflaneSupp4WDL.error))
                return (null, direOfflaneSupp4WDL.error);

            var laningStatistics = new LaningStatistics()
            {
                Mid = midWDL.wdl,
                EasyLane = carryVsOfflaneWDL.wdl
                    .GetWDLSum(carryVsSupp4WDL.wdl, 
                    supp5VsOfflaneWDL.wdl, 
                    supp5VsSupp4WDL.wdl,
                    radiantCarrySupp5WDL.wdl,
                    direOfflaneSupp4WDL.wdl),
                HardLane = offlaneVsCarryWDL.wdl
                    .GetWDLSum(offlaneVsSupp5WDL.wdl,
                    supp4VsCarryWDL.wdl,
                    supp4VsSupp5WDL.wdl,
                    radiantOfflaneSupp4WDL.wdl,
                    direCarrySupp5WDL.wdl)
            };

            return (laningStatistics, "");
        }

        private static (LaningWDL? wdl, string error) GetLaningWDL(LaningData data, int heroId, bool isDire)
        {
            var stats = data.Data.HeroStats.LaneOutcome
                .FirstOrDefault(h => h.HeroId2 == heroId);
            if (stats == null)
                return (null, "laning not found for " + heroId);
            return (new LaningWDL()
            {
                WinCount = isDire ? stats.LossCount + stats.StompLossCount * 2 : stats.WinCount + stats.StompWinCount * 2,
                DrawCount = stats.DrawCount,
                LossCount = isDire ? stats.WinCount + stats.StompWinCount * 2 : stats.LossCount + stats.StompLossCount * 2
            }, "");
        }
    }
}
