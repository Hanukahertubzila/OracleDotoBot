using OracleDotoBot.Abstractions;
using OracleDotoBot.Domain.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Services
{
    public class LiveMatchesService : ILiveMatchesService
    {
        public LiveMatchesService(ISteamApiService steamApiService)
        {
            _steamApiService = steamApiService;
            LiveMatches = new List<Match>();
        }

        public List<Match> LiveMatches {  get; private set; }

        private readonly ISteamApiService _steamApiService;

        public async Task<ReplyKeyboardMarkup> GetLiveMatchesKeyboard()
        {
            var matches = await _steamApiService.GetLiveMatches();

            LiveMatches = matches;

            var keyboard = new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                new KeyboardButton[]
                {
                    new KeyboardButton(matches.Count > 0 ?
                        $"{ matches[0].RadiantTeam.Name } VS { matches[0].DireTeam.Name }" : "no match"),
                    new KeyboardButton(matches.Count > 1 ?
                        $"{ matches[1].RadiantTeam.Name } VS { matches[1].DireTeam.Name }" : "no match")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(matches.Count > 2 ?
                        $"{matches[2].RadiantTeam.Name} VS {matches[2].DireTeam.Name}" : "no match"),
                    new KeyboardButton(matches.Count > 3 ?
                        $"{matches[3].RadiantTeam.Name} VS {matches[3].DireTeam.Name}" : "no match")
                },
                new KeyboardButton[]
                {       
                    new KeyboardButton(matches.Count > 4 ?
                        $"{ matches[4].RadiantTeam.Name } VS { matches[4].DireTeam.Name }" : "no match"),
                    new KeyboardButton(matches.Count > 5 ?
                        $"{ matches[5].RadiantTeam.Name } VS { matches[5].DireTeam.Name }" : "no match")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(matches.Count > 6 ?
                        $"{matches[6].RadiantTeam.Name} VS {matches[6].DireTeam.Name}" : "no match"),
                    new KeyboardButton("Назад")
                }
                })
            {
                ResizeKeyboard = true,
            };

            return keyboard;
        }
    }
}
