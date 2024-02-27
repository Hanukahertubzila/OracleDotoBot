using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;
using OracleDotoBot.SteamApi.ResponseObjectModels;

namespace OracleDotoBot.SteamApi.Converters
{
    public static class ToMatchListConverter
    {
        public static List<Match> Convert(LiveMatchesResponse response, List<Hero> heroes)
        {
            var matches = response.GameList
                .Where(m => !string.IsNullOrEmpty(m.RadiantName) &&
                !string.IsNullOrEmpty(m.DireName) && m.Players.Count == 10).ToList();

            var liveProMatches = new List<Match>();
            foreach(var m in matches)
            {
                var radiantTeam = new Team()
                {
                    Name = m.RadiantName,
                    Pos1 = new Player()
                    {
                        AccountId = m.Players
                            .First(p => p.IsRadiant && p.TeamSlot == 1)
                            .AccountId,
                        Hero = heroes.First(h => h.Id == m.Players
                            .First(p => p.IsRadiant && p.TeamSlot == 1)
                            .HeroId)
                    },
                    Pos2 = new Player()
                    {
                        AccountId = m.Players
                            .First(p => p.IsRadiant && p.TeamSlot == 2)
                            .AccountId,
                        Hero = heroes.First(h => h.Id == m.Players
                            .First(p => p.IsRadiant && p.TeamSlot == 2)
                            .HeroId)
                    },
                    Pos3 = new Player()
                    {
                        AccountId = m.Players
                            .First(p => p.IsRadiant && p.TeamSlot == 3)
                            .AccountId,
                        Hero = heroes.First(h => h.Id == m.Players
                            .First(p => p.IsRadiant && p.TeamSlot == 3)
                            .HeroId)
                    },
                    Pos4 = new Player()
                    {
                        AccountId = m.Players
                            .First(p => p.IsRadiant && p.TeamSlot == 4)
                            .AccountId,
                        Hero = heroes.First(h => h.Id == m.Players
                            .First(p => p.IsRadiant && p.TeamSlot == 4)
                            .HeroId)
                    },
                    Pos5 = new Player()
                    {
                        AccountId = m.Players
                            .First(p => p.IsRadiant && p.TeamSlot == 5)
                            .AccountId,
                        Hero = heroes.First(h => h.Id == m.Players
                            .First(p => p.IsRadiant && p.TeamSlot == 5)
                            .HeroId)
                    }
                };
                var direTeam = new Team()
                {
                    Name = m.DireName,
                    Pos1 = new Player()
                    {
                        AccountId = m.Players
                            .First(p => !p.IsRadiant && p.TeamSlot == 1)
                            .AccountId,
                        Hero = heroes.First(h => h.Id == m.Players
                            .First(p => !p.IsRadiant && p.TeamSlot == 1)
                            .HeroId)
                    },
                    Pos2 = new Player()
                    {
                        AccountId = m.Players
                            .First(p => !p.IsRadiant && p.TeamSlot == 2)
                            .AccountId,
                        Hero = heroes.First(h => h.Id == m.Players
                            .First(p => !p.IsRadiant && p.TeamSlot == 2)
                            .HeroId)
                    },
                    Pos3 = new Player()
                    {
                        AccountId = m.Players
                            .First(p => !p.IsRadiant && p.TeamSlot == 3)
                            .AccountId,
                        Hero = heroes.First(h => h.Id == m.Players
                            .First(p => !p.IsRadiant && p.TeamSlot == 3)
                            .HeroId)
                    },
                    Pos4 = new Player()
                    {
                        AccountId = m.Players
                            .First(p => !p.IsRadiant && p.TeamSlot == 4)
                            .AccountId,
                        Hero = heroes.First(h => h.Id == m.Players
                            .First(p => !p.IsRadiant && p.TeamSlot == 4)
                            .HeroId)
                    },
                    Pos5 = new Player()
                    {
                        AccountId = m.Players
                            .First(p => !p.IsRadiant && p.TeamSlot == 5)
                            .AccountId,
                        Hero = heroes.First(h => h.Id == m.Players
                            .First(p => !p.IsRadiant && p.TeamSlot == 5)
                            .HeroId)
                    }
                };
                var heroIds = m.Players.Select(p => p.HeroId).ToList();
                var match = new Match()
                {
                    RadiantTeam = radiantTeam,
                    DireTeam = direTeam,
                    HeroIds = heroIds
                };
                liveProMatches.Add(match);
            }
            return liveProMatches;
        }
    }
}
