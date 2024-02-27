using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;
using OracleDotoBot.StratzApiParser.ResponseObjectModels;

namespace OracleDotoBot.StratzApiParser.Converters
{
    public static class ToMatchConverter
    {
        public static Match Convert(MatchResponse response, List<Hero> heroes)
        {
            var m = response.Data.Match;
            var heroIds = m.Players.Select(p => (int)p.HeroId).ToList();
            var radiantTeam = new Team()
            {
                Name = response.Data.Match.RadiantTeam.Name,
                Pos1 = new Player()
                {
                    Hero = heroes
                        .First(h =>
                        h.Id == m.Players
                        .First(h => h.IsRadiant &&
                        h.Position == Enums.Positions.POSITION_1).HeroId)
                },
                Pos2 = new Player()
                {
                    Hero = heroes
                        .First(h =>
                        h.Id == m.Players
                        .First(h => h.IsRadiant &&
                        h.Position == Enums.Positions.POSITION_2).HeroId)
                },
                Pos3 = new Player()
                {
                    Hero = heroes
                        .First(h =>
                        h.Id == m.Players
                        .First(h => h.IsRadiant &&
                        h.Position == Enums.Positions.POSITION_3).HeroId)
                },
                Pos4 = new Player()
                {
                    Hero = heroes
                        .First(h =>
                        h.Id == m.Players
                        .First(h => h.IsRadiant &&
                        h.Position == Enums.Positions.POSITION_4).HeroId)
                },
                Pos5 = new Player()
                {
                    Hero = heroes
                        .First(h =>
                        h.Id == m.Players
                        .First(h => h.IsRadiant &&
                        h.Position == Enums.Positions.POSITION_5).HeroId)
                }
            };
            var direTeam = new Team()
            {
                Name = response.Data.Match.DireTeam.Name,
                Pos1 = new Player()
                {
                    Hero = heroes
                        .First(h =>
                        h.Id == m.Players
                        .First(h => !h.IsRadiant &&
                        h.Position == Enums.Positions.POSITION_1).HeroId)
                },
                Pos2 = new Player()
                {
                    Hero = heroes
                        .First(h =>
                        h.Id == m.Players
                        .First(h => !h.IsRadiant &&
                        h.Position == Enums.Positions.POSITION_2).HeroId)
                },
                Pos3 = new Player()
                {
                    Hero = heroes
                        .First(h =>
                        h.Id == m.Players
                        .First(h => !h.IsRadiant &&
                        h.Position == Enums.Positions.POSITION_3).HeroId)
                },
                Pos4 = new Player()
                {
                    Hero = heroes
                        .First(h =>
                        h.Id == m.Players
                        .First(h => !h.IsRadiant &&
                        h.Position == Enums.Positions.POSITION_4).HeroId)
                },
                Pos5 = new Player()
                {
                    Hero = heroes
                        .First(h =>
                        h.Id == m.Players
                        .First(h => !h.IsRadiant &&
                        h.Position == Enums.Positions.POSITION_5).HeroId)
                }
            };
            var match = new Match()
            {
                RadiantTeam = radiantTeam,
                DireTeam = direTeam,
                HeroIds = heroIds
            };
            return match;
        }
    }
}
