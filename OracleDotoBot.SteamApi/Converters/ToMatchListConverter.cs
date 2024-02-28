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
                var rpos1 = m.Players
                    .FirstOrDefault(p => p.IsRadiant && p.TeamSlot == 1);
                if (rpos1 == null) continue;
                var rpos2 = m.Players
                    .FirstOrDefault(p => p.IsRadiant && p.TeamSlot == 2);
                if (rpos2 == null) continue;
                var rpos3 = m.Players
                    .FirstOrDefault(p => p.IsRadiant && p.TeamSlot == 3);
                if (rpos3 == null) continue;
                var rpos4 = m.Players
                    .FirstOrDefault(p => p.IsRadiant && p.TeamSlot == 4);
                if (rpos4 == null) continue;
                var rpos5 = m.Players
                    .FirstOrDefault(p => p.IsRadiant && p.TeamSlot == 5);
                if (rpos5 == null) continue;
                var dpos1 = m.Players
                    .FirstOrDefault(p => !p.IsRadiant && p.TeamSlot == 1);
                if (dpos1 == null) continue;
                var dpos2 = m.Players
                    .FirstOrDefault(p => !p.IsRadiant && p.TeamSlot == 2);
                if (dpos2 == null) continue;
                var dpos3 = m.Players
                    .FirstOrDefault(p => !p.IsRadiant && p.TeamSlot == 3);
                if (dpos3 == null) continue;
                var dpos4 = m.Players
                    .FirstOrDefault(p => !p.IsRadiant && p.TeamSlot == 4);
                if (dpos4 == null) continue;
                var dpos5 = m.Players
                    .FirstOrDefault(p => !p.IsRadiant && p.TeamSlot == 5);
                if (dpos5 == null) continue;
                var radiantTeam = new Team()
                {
                    Name = m.RadiantName,                   
                    Pos1 = new Player()
                    {
                        AccountId = rpos1.AccountId,
                        Hero = heroes.First(h => h.Id == rpos1.HeroId)
                    },
                    Pos2 = new Player()
                    {
                        AccountId = rpos2.AccountId,
                        Hero = heroes.First(h => h.Id == rpos2.HeroId)
                    },
                    Pos3 = new Player()
                    {
                        AccountId = rpos3.AccountId,
                        Hero = heroes.First(h => h.Id == rpos3.HeroId)
                    },
                    Pos4 = new Player()
                    {
                        AccountId = rpos4.AccountId,
                        Hero = heroes.First(h => h.Id == rpos4.HeroId)
                    },
                    Pos5 = new Player()
                    {
                        AccountId = rpos5.AccountId,
                        Hero = heroes.First(h => h.Id == rpos5.HeroId)
                    }
                };
                var direTeam = new Team()
                {
                    Name = m.DireName,
                    Pos1 = new Player()
                    {
                        AccountId = dpos1.AccountId,
                        Hero = heroes.First(h => h.Id == dpos1.HeroId)
                    },
                    Pos2 = new Player()
                    {
                        AccountId = dpos2.AccountId,
                        Hero = heroes.First(h => h.Id == dpos2.HeroId)
                    },
                    Pos3 = new Player()
                    {
                        AccountId = dpos3.AccountId,
                        Hero = heroes.First(h => h.Id == dpos3.HeroId)
                    },
                    Pos4 = new Player()
                    {
                        AccountId = dpos4.AccountId,
                        Hero = heroes.First(h => h.Id == dpos4.HeroId)
                    },
                    Pos5 = new Player()
                    {
                        AccountId = dpos5.AccountId,
                        Hero = heroes.First(h => h.Id == dpos5.HeroId)
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
