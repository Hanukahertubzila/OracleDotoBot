using OracleDotoBot.Abstractions;
using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;

namespace OracleDotoBot.Services
{
    public class MatchesResultService : IMatchesResultService
    {
        public MatchesResultService(IStratzApiService stratzApiService)
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

        public async Task<string> GetMatchResult(Match match)
        {
            var result = await _stratzApiService.GetStatisticsString(match);
            return result;
        }

        public async Task<string> AlterMatch(long chatId, Hero hero)
        {
            var match = _matches.FirstOrDefault(m => m.chatId == chatId);
            if (match.match == null)
            {
                NewMatch(chatId);
                match = _matches.FirstOrDefault(m => m.chatId == chatId);
            }

            if (match.match.HeroIds.Contains(hero.Id))
            {
                return "Один и тот же герой не может встречаться более 1 раза...";
            }

            match.match.HeroIds.Add(hero.Id);
            switch (match.match.HeroIds.Count)
            {
                case 1:
                    match.match.RadiantTeam.Pos1 = new Player() { Hero = hero };
                    return "Мидер команды сил света: ";
                case 2:
                    match.match.RadiantTeam.Pos2 = new Player() { Hero = hero };
                    return "Оффлейнер команды сил света: ";
                case 3:
                    match.match.RadiantTeam.Pos3 = new Player() { Hero = hero };
                    return "Саппорт 4 команды сил света: ";
                case 4:
                    match.match.RadiantTeam.Pos4 = new Player() { Hero = hero };
                    return "Саппорт 5 команды сил света: ";
                case 5:
                    match.match.RadiantTeam.Pos5 = new Player() { Hero = hero };
                    return "Керри команды сил тьмы: ";
                case 6:
                    match.match.DireTeam.Pos1 = new Player() { Hero = hero };
                    return "Мидер команды сил тьмы: ";
                case 7:
                    match.match.DireTeam.Pos2 = new Player() { Hero = hero };
                    return "Оффлейнер команды сил тьмы: ";
                case 8:
                    match.match.DireTeam.Pos3 = new Player() { Hero = hero };
                    return "Саппорт 4 команды сил тьмы: ";
                case 9:
                    match.match.DireTeam.Pos4 = new Player() { Hero = hero };
                    return "Саппорт 5 команды сил тьмы: ";
                default:
                    match.match.DireTeam.Pos5 = new Player() { Hero = hero };
                    return await GetMatchResult(_matches.First(c => c.chatId == chatId).match);
            }           
        }
    }
}
