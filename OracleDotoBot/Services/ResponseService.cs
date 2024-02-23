using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OracleDotoBot.Abstractions;
using OracleDotoBot.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Services
{
    public class ResponseService : IResponseService
    {
        public ResponseService(IOptions<List<Hero>> heroes, 
            IMatchesResultService matchesResultService,
            ILiveMatchesService liveMatchesService,
            ILogger<ResponseService> logger)
        {
            _heroes = heroes;
            _matchesResultService = matchesResultService;
            _liveMatchesService = liveMatchesService;
            _logger = logger;
        }

        private readonly IOptions<List<Hero>> _heroes;
        private readonly IMatchesResultService _matchesResultService;
        private readonly ILiveMatchesService _liveMatchesService;
        private readonly ILogger<ResponseService> _logger;

        public async Task<(string text, IReplyMarkup? replyMarkup)> GetResponse(string messageText, long chatId)
        {
            var responseText = "";
            var replyKeyboard = new ReplyKeyboardMarkup(
                                        new List<KeyboardButton[]>()
                                        {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Предсказать победу"),
                                            new KeyboardButton("Лайв матчи"),
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Статистика бота")
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Помощь")
                                        }
                                        })
            {
                ResizeKeyboard = true,
            };
            switch (messageText)
            {
                case "/start":
                    return ("Привет лудик!", replyKeyboard);
                case "Помощь":
                    responseText = $@"ПРЕДСКАЗАТЬ ПОБЕДУ - чтобы получить анализ по драфту введи имена персонажей по их позиции и команде
ТУРНИРЫ - список актуальных турниров и текущих игр с аналитикой по драфту
СТАТИСТИКА - статистика бота за определенный период/турнир (собирается только на турнирах 1/2 дивизионов)";
                    return (responseText, replyKeyboard);
                case "Предсказать победу":
                    _matchesResultService.NewMatch(chatId);
                    responseText = "Керри команды сил света: ";
                    return (responseText, null);
                case "Лайв матчи":
                    var liveMatchesKeyboard = await _liveMatchesService.GetLiveMatchesKeyboard();
                    return ("Лайв матчи: ", liveMatchesKeyboard);
                default:
                    var heroCommand = _heroes.Value
                        .FirstOrDefault(h => h.Name == messageText
                        || h.LocalizedName == messageText);
                    if (heroCommand != null)
                    {
                        responseText = _matchesResultService.AlterMatch(chatId, heroCommand);
                        return (responseText, null);
                    }

                    var matchCommand = _liveMatchesService.LiveMatches
                        .FirstOrDefault(m =>
                        $"{m.RadiantTeam.Name} VS {m.DireTeam.Name}" == messageText);
                    if (matchCommand != null)
                    {
                        responseText = await _matchesResultService.GetMatchResult(matchCommand);
                        return (responseText, replyKeyboard);
                    }
                    return ("Неизвестная команда...", replyKeyboard);
            }
        }
    }
}
