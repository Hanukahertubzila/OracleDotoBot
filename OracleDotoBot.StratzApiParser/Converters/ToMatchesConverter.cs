using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;
using OracleDotoBot.StratzApiParser.ResponseObjectModels;

namespace OracleDotoBot.StratzApiParser.Converters
{
    public static class ToMatchesConverter
    {
        public static List<Match> Convert(LiveMatchesResponse response, List<Hero> heroes)
        {
            var matches = new List<Match>();
            foreach (var m in response.Data.Live.Matches)
            {
                if ((m.GameState == "STRATEGY_TIME" || 
                    m.GameState == "PRE_GAME" || 
                    m.GameState == "GAME_IN_PROGRESS" ||
                    m.GameState == "POST_GAME") && 
                    m.RadiantTeam.Name != null && 
                    m.DireTeam.Name != null)
                {
                    var match = new Match();
                    match.RadiantTeam.Name = m.RadiantTeam.Name;
                    match.RadiantTeam.Pos1 = new Player()
                    {
                        Hero = heroes
                            .First(h => 
                            h.Id == m.Players
                            .First(h => h.IsRadiant && 
                            h.Position == Enums.Positions.POSITION_1).HeroId)
                    };
                    match.HeroIds.Add(match.RadiantTeam.Pos1.Hero.Id);
                    match.RadiantTeam.Pos2 = new Player()
                    {
                        Hero = heroes
                            .First(h =>
                            h.Id == m.Players
                            .First(h => h.IsRadiant &&
                            h.Position == Enums.Positions.POSITION_2).HeroId)
                    };
                    match.HeroIds.Add(match.RadiantTeam.Pos2.Hero.Id);
                    match.RadiantTeam.Pos3 = new Player()
                    {
                        Hero = heroes
                            .First(h =>
                            h.Id == m.Players
                            .First(h => h.IsRadiant &&
                            h.Position == Enums.Positions.POSITION_3).HeroId)
                    };
                    match.HeroIds.Add(match.RadiantTeam.Pos3.Hero.Id);
                    match.RadiantTeam.Pos4 = new Player()
                    {
                        Hero = heroes
                            .First(h =>
                            h.Id == m.Players
                            .First(h => h.IsRadiant &&
                            h.Position == Enums.Positions.POSITION_4).HeroId)
                    };
                    match.HeroIds.Add(match.RadiantTeam.Pos4.Hero.Id);
                    match.RadiantTeam.Pos5 = new Player()
                    {
                        Hero = heroes
                            .First(h =>
                            h.Id == m.Players
                            .First(h => h.IsRadiant &&
                            h.Position == Enums.Positions.POSITION_5).HeroId)
                    };
                    match.HeroIds.Add(match.RadiantTeam.Pos5.Hero.Id);

                    match.DireTeam.Name = m.DireTeam.Name;
                    match.DireTeam.Pos1 = new Player()
                    {
                        Hero = heroes
                            .First(h =>
                            h.Id == m.Players
                            .First(h => !h.IsRadiant &&
                            h.Position == Enums.Positions.POSITION_1).HeroId)
                    };
                    match.HeroIds.Add(match.DireTeam.Pos1.Hero.Id);
                    match.DireTeam.Pos2 = new Player()
                    {
                        Hero = heroes
                            .First(h =>
                            h.Id == m.Players
                            .First(h => !h.IsRadiant &&
                            h.Position == Enums.Positions.POSITION_2).HeroId)
                    };
                    match.HeroIds.Add(match.DireTeam.Pos2.Hero.Id);
                    match.DireTeam.Pos3 = new Player()
                    {
                        Hero = heroes
                            .First(h =>
                            h.Id == m.Players
                            .First(h => !h.IsRadiant &&
                            h.Position == Enums.Positions.POSITION_3).HeroId)
                    };
                    match.HeroIds.Add(match.DireTeam.Pos3.Hero.Id);
                    match.DireTeam.Pos4 = new Player()
                    {
                        Hero = heroes
                            .First(h =>
                            h.Id == m.Players
                            .First(h => !h.IsRadiant &&
                            h.Position == Enums.Positions.POSITION_4).HeroId)
                    };
                    match.HeroIds.Add(match.DireTeam.Pos4.Hero.Id);
                    match.DireTeam.Pos5 = new Player()
                    {
                        Hero = heroes
                            .First(h =>
                            h.Id == m.Players
                            .First(h => !h.IsRadiant &&
                            h.Position == Enums.Positions.POSITION_5).HeroId)
                    };
                    match.HeroIds.Add(match.DireTeam.Pos5.Hero.Id);

                    matches.Add(match);
                }
            }
            return matches;
        }
    }
}
