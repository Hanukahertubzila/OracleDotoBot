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

            var laningStatistics = new LaningStatistics()
            {
                Mid = midWDL,
                EasyLane = carryVsOfflaneWDL
                    .GetWDLSum(carryVsSupp4WDL, 
                    supp5VsOfflaneWDL, 
                    supp5VsSupp4WDL,
                    radiantCarrySupp5WDL,
                    direOfflaneSupp4WDL),
                HardLane = offlaneVsCarryWDL
                    .GetWDLSum(offlaneVsSupp5WDL,
                    supp4VsCarryWDL,
                    supp4VsSupp5WDL,
                    radiantOfflaneSupp4WDL,
                    direCarrySupp5WDL)
            };

            return (laningStatistics, "");
        }

        private static LaningWDL GetLaningWDL(LaningData data, int heroId, bool isDire)
        {
            var stats = data.Data.HeroStats.LaneOutcome
                .FirstOrDefault(h => h.HeroId2 == heroId);
            if (stats == null)
                return new LaningWDL()
                {
                    WinCount = 0,
                    DrawCount = 0,
                    LossCount = 0
                };
            return new LaningWDL()
            {
                WinCount = isDire ? stats.LossCount + stats.StompLossCount * 2 : stats.WinCount + stats.StompWinCount * 2,
                DrawCount = stats.DrawCount,
                LossCount = isDire ? stats.WinCount + stats.StompWinCount * 2 : stats.LossCount + stats.StompLossCount * 2
            };
        }
    }
}
