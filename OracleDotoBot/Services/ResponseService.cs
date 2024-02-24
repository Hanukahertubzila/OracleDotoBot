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
                    return ("Привет лудик! \nВАЖНО ПРОЧИТАТЬ: \nБот служит помощником в анализе драфта, и хотя я не учил его проигрывать, никаких 100% результатов он не дает, внимательно читай аналитику и принимай решение сам, основываясь на цифрах. Оценка драфта и формирование результатов может занять некоторое время... \nудачи <3", replyKeyboard);
                case "Помощь":
                    responseText = $@"*ПРЕДСКАЗАТЬ ПОБЕДУ* - чтобы получить анализ по драфту введи имена персонажей по их позиции и команде (имена нужно вводить без ошибок на английском как они записаны в самой доте)
*ЛАЙВ МАТЧИ* - список актуальных матчей с аналитикой
*СТАТИСТИКА* - статистика бота за определенный период/турнир (собирается только на турнирах 1/2 дивизионов)";
                    return (responseText, replyKeyboard);
                case "Предсказать победу":
                    _matchesResultService.NewMatch(chatId);
                    responseText = "Керри команды сил света: ";
                    return (responseText, null);
                case "Лайв матчи":
                    var liveMatchesKeyboard = await _liveMatchesService.GetLiveMatchesKeyboard();
                    return ("Лайв матчи: ", liveMatchesKeyboard);
                case "Статистика бота":
                    responseText = @"В моем коде заложена функция 
function SetWinChanсes() {
    let winChance = 100%
    let proebChance = 0%
}
поэтому проиграть невозможно, сегодня онли вин.....
    ";
                    return (responseText, replyKeyboard);
                case "Назад":
                    return ("Меню: ", replyKeyboard);
                default:
                    var heroCommand = _heroes.Value
                        .FirstOrDefault(h => h.Name == messageText
                        || h.LocalizedName == messageText);
                    if (heroCommand != null)
                    {
                        responseText = await _matchesResultService.AlterMatch(chatId, heroCommand);
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
