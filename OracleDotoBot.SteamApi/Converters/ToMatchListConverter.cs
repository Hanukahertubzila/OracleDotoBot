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
                var heroIds = new List<int>();
                var rpos1 = m.Players
                    .FirstOrDefault(p => !p.IsRadiant && p.TeamSlot == 1 && p.HeroId != 0);
                if (rpos1 == null) continue;
                heroIds.Add(rpos1.HeroId);
                var rpos2 = m.Players
                    .FirstOrDefault(p => !p.IsRadiant && p.TeamSlot == 2 && p.HeroId != 0);
                if (rpos2 == null) continue;
                heroIds.Add(rpos2.HeroId);
                var rpos3 = m.Players
                    .FirstOrDefault(p => !p.IsRadiant && p.TeamSlot == 3 && p.HeroId != 0);
                if (rpos3 == null) continue;
                heroIds.Add(rpos3.HeroId);
                var rpos4 = m.Players
                    .FirstOrDefault(p => !p.IsRadiant && p.TeamSlot == 4 && p.HeroId != 0);
                if (rpos4 == null) continue;
                heroIds.Add(rpos4.HeroId);
                var rpos5 = m.Players
                    .FirstOrDefault(p => !p.IsRadiant && p.TeamSlot == 5 && p.HeroId != 0);
                if (rpos5 == null) continue;
                heroIds.Add(rpos5.HeroId);
                var dpos1 = m.Players
                    .FirstOrDefault(p => p.IsRadiant && p.TeamSlot == 1 && p.HeroId != 0);
                if (dpos1 == null) continue;
                heroIds.Add(dpos1.HeroId);
                var dpos2 = m.Players
                    .FirstOrDefault(p => p.IsRadiant && p.TeamSlot == 2 && p.HeroId != 0);
                if (dpos2 == null) continue;
                heroIds.Add(dpos2.HeroId);
                var dpos3 = m.Players
                    .FirstOrDefault(p => p.IsRadiant && p.TeamSlot == 3 && p.HeroId != 0);
                if (dpos3 == null) continue;
                heroIds.Add(dpos3.HeroId);
                var dpos4 = m.Players
                    .FirstOrDefault(p => p.IsRadiant && p.TeamSlot == 4 && p.HeroId != 0);
                if (dpos4 == null) continue;
                heroIds.Add(dpos4.HeroId);
                var dpos5 = m.Players
                    .FirstOrDefault(p => p.IsRadiant && p.TeamSlot == 5 && p.HeroId != 0);
                if (dpos5 == null) continue;
                heroIds.Add(dpos5.HeroId);
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
