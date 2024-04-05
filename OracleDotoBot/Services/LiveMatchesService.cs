using Microsoft.Extensions.Logging;
using OracleDotoBot.Abstractions.Services;
using OracleDotoBot.Domain.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Services
{
    public class LiveMatchesService : ILiveMatchesService
    {
        public LiveMatchesService(ISteamApiService steamApiService, 
            IMatchesResultService matchesResultService,
            ILogger<LiveMatchesService> logger)
        {
            _steamApiService = steamApiService;
            _matchesResultService = matchesResultService;
            _logger = logger;
            LiveMatches = new List<(Match match, string analitics)>();
        }

        public List<(Match match, string analitics)> LiveMatches {  get; private set; }

        private readonly ISteamApiService _steamApiService;
        private readonly IMatchesResultService _matchesResultService;
        private readonly ILogger<LiveMatchesService> _logger;

        public async Task UpdateLiveMatches()
        {
            var matches = await _steamApiService.GetLiveMatches();

            var liveMatches = new List<(Match match, string analitics)>();
            var updatedMatches = 0;
            foreach (var m in matches)
            {
                var liveMatch = LiveMatches.FirstOrDefault(l => l.match.Id == m.Id);
                if (liveMatch != default)
                    liveMatches.Add((m, liveMatch.analitics));
                else
                {
                    var analitics = await _matchesResultService.GetMatchResult(m, false, true);
                    liveMatches.Add((m, analitics));
                    updatedMatches += 1;
                }
            }
            _logger.LogInformation($"Live matches: {liveMatches.Count}; Updated matches: {updatedMatches}");
            LiveMatches = liveMatches;
        }

        public ReplyKeyboardMarkup GetLiveMatchesKeyboard()
        {
            var keyboard = new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                new KeyboardButton[]
                {
                    new KeyboardButton(LiveMatches.Count > 0 ?
                        $"{ LiveMatches[0].match.RadiantTeam.Name } VS { LiveMatches[0].match.DireTeam.Name }" : "no match"),
                    new KeyboardButton(LiveMatches.Count > 1 ?
                        $"{ LiveMatches[1].match.RadiantTeam.Name } VS { LiveMatches[1].match.DireTeam.Name }" : "no match")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(LiveMatches.Count > 2 ?
                        $"{LiveMatches[2].match.RadiantTeam.Name} VS {LiveMatches[2].match.DireTeam.Name}" : "no match"),
                    new KeyboardButton(LiveMatches.Count > 3 ?
                        $"{LiveMatches[3].match.RadiantTeam.Name} VS {LiveMatches[3].match.DireTeam.Name}" : "no match")
                },
                new KeyboardButton[]
                {       
                    new KeyboardButton(LiveMatches.Count > 4 ?
                        $"{ LiveMatches[4].match.RadiantTeam.Name } VS { LiveMatches[4].match.DireTeam.Name }" : "no match"),
                    new KeyboardButton(LiveMatches.Count > 5 ?
                        $"{ LiveMatches[5].match.RadiantTeam.Name } VS { LiveMatches[5].match.DireTeam.Name }" : "no match")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(LiveMatches.Count > 6 ?
                        $"{LiveMatches[6].match.RadiantTeam.Name} VS {LiveMatches[6].match.DireTeam.Name}" : "no match"),
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
