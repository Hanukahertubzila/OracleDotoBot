using OracleDotoBot.Abstractions;
using OracleDotoBot.Domain.Models;

namespace OracleDotoBot.Services
{
    public class UserMatchesService : IUserMatchesService
    {
        public UserMatchesService(IStratzApiService stratzApiService)
        {
            _matches = new List<(Match match, long chatId)>();
            _stratzApiService = stratzApiService;
        }

        private readonly List<(Match match, long chatId)> _matches;
        private readonly IStratzApiService _stratzApiService;

        public void NewMatch(long chatId)
        {
            var match = _matches.FirstOrDefault(m => m.chatId == chatId);
            if (match.match != null)
                _matches.Remove(match);
            _matches.Add((new Match(), chatId));
        }

        public async Task<string> AlterMatch(long chatId, int heroId)
        {
            var match = _matches.FirstOrDefault(m => m.chatId == chatId);
            if (match.match == null)
            {
                NewMatch(chatId);
                match = _matches.FirstOrDefault(m => m.chatId == chatId);
            }
            if (match.match.RadiantTeam.Pos1HeroId == 0 && !match.match.HeroIds.Contains(heroId))
            {
                match.match.HeroIds.Add(heroId);
                match.match.RadiantTeam.Pos1HeroId = heroId;
                return "Мидер команды сил света: ";
            }
            else if (match.match.RadiantTeam.Pos2HeroId == 0 && !match.match.HeroIds.Contains(heroId))
            {
                match.match.HeroIds.Add(heroId);
                match.match.RadiantTeam.Pos2HeroId = heroId;
                return "Оффлейнер команды сил света: ";
            }
            else if (match.match.RadiantTeam.Pos3HeroId == 0 && !match.match.HeroIds.Contains(heroId))
            {
                match.match.HeroIds.Add(heroId);
                match.match.RadiantTeam.Pos3HeroId = heroId;
                return "Саппорт 4 команды сил света: ";
            }
            else if (match.match.RadiantTeam.Pos4HeroId == 0 && !match.match.HeroIds.Contains(heroId))
            {
                match.match.HeroIds.Add(heroId);
                match.match.RadiantTeam.Pos4HeroId = heroId;
                return "Саппорт 5 команды сил света: ";
            }
            else if (match.match.RadiantTeam.Pos5HeroId == 0 && !match.match.HeroIds.Contains(heroId))
            {
                match.match.HeroIds.Add(heroId);
                match.match.RadiantTeam.Pos5HeroId = heroId;
                return "Керри команды сил тьмы: ";
            }
            else if (match.match.DireTeam.Pos1HeroId == 0 && !match.match.HeroIds.Contains(heroId))
            {
                match.match.HeroIds.Add(heroId);
                match.match.DireTeam.Pos1HeroId = heroId;
                return "Мидер команды сил тьмы: ";
            }
            else if (match.match.DireTeam.Pos2HeroId == 0 && !match.match.HeroIds.Contains(heroId))
            {
                match.match.HeroIds.Add(heroId);
                match.match.DireTeam.Pos2HeroId = heroId;
                return "Оффлейнер команды сил тьмы: ";
            }
            else if (match.match.DireTeam.Pos3HeroId == 0 && !match.match.HeroIds.Contains(heroId))
            {
                match.match.HeroIds.Add(heroId);
                match.match.DireTeam.Pos3HeroId = heroId;
                return "Саппорт 4 команды сил тьмы: ";
            }
            else if (match.match.DireTeam.Pos4HeroId == 0 && !match.match.HeroIds.Contains(heroId))
            {
                match.match.HeroIds.Add(heroId);
                match.match.DireTeam.Pos4HeroId = heroId;
                return "Саппорт 5 команды сил тьмы: ";
            }
            else if (match.match.DireTeam.Pos5HeroId == 0 && !match.match.HeroIds.Contains(heroId))
            {
                match.match.HeroIds.Add(heroId);
                match.match.DireTeam.Pos5HeroId = heroId;
                var stats = await _stratzApiService.GetStatisticsString(match.match);
                return stats;
            }
            else
            {
                return "Один и тот же герой не может встречаться более 1 раза...";
            }
        }
    }
}
