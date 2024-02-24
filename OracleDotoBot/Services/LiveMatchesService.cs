using Microsoft.Extensions.Logging;
using OracleDotoBot.Abstractions;
using OracleDotoBot.Domain.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Services
{
    public class LiveMatchesService : ILiveMatchesService
    {
        public LiveMatchesService(IStratzApiService stratzApiService,
            ILogger<LiveMatchesService> logger)
        {
            LiveMatches = new List<Match>();
            _stratzApiService = stratzApiService;
            _logger = logger;
        }

        public List<Match> LiveMatches { get; private set; }

        private readonly IStratzApiService _stratzApiService;
        private readonly ILogger<LiveMatchesService> _logger;

        public async Task<ReplyKeyboardMarkup> GetLiveMatchesKeyboard()
        {
            var matches = await _stratzApiService.GetLiveMatches();

            LiveMatches = matches;

            if (matches.Count < 5)
                _logger.LogInformation("Matches count is less than expected");

            var keyboard = new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                new KeyboardButton[]
                {
                    new KeyboardButton(matches.Count > 0 ?
                        $"{ matches[0].RadiantTeam.Name } VS {matches[0].DireTeam.Name }" : "no match"),
                    new KeyboardButton(matches.Count > 1 ?
                        $"{ matches[1].RadiantTeam.Name } VS {matches[1].DireTeam.Name }" : "no match")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(matches.Count > 2 ?
                        $"{ matches[2].RadiantTeam.Name } VS {matches[2].DireTeam.Name }" : "no match"),
                    new KeyboardButton(matches.Count > 3 ?
                        $"{ matches[3].RadiantTeam.Name } VS {matches[3].DireTeam.Name }" : "no match")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(matches.Count > 4 ?
                        $"{ matches[4].RadiantTeam.Name } VS {matches[4].DireTeam.Name }" : "no match"),
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
