using Microsoft.Extensions.Options;
using OracleDotoBot.Abstractions;
using OracleDotoBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Services
{
    public class ResponseService : IResponseService
    {
        public ResponseService(IOptions<List<Hero>> heroes, 
            IMatchesResultService matchesResultService,
            ILiveMatchesService liveMatchesService,
            ITelegramBotClient client)
        {
            _heroes = heroes;
            _matchesResultService = matchesResultService;
            _liveMatchesService = liveMatchesService;
            _client = client;
        }

        private readonly IOptions<List<Hero>> _heroes;
        private readonly IMatchesResultService _matchesResultService;
        private readonly ILiveMatchesService _liveMatchesService;
        private readonly ITelegramBotClient _client;

        public async Task GetResponse(string messageText, long chatId)
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
                    responseText = "Привет лудик! \nВАЖНО ПРОЧИТАТЬ: \nБот служит помощником в анализе драфта, и хотя я не учил его проигрывать, никаких 100% результатов он не дает, внимательно читай аналитику и принимай решение сам, основываясь на цифрах. Оценка драфта и формирование результатов может занять некоторое время... \nудачи <3";
                    break;
                case "Помощь":
                    responseText = $@"*ПРЕДСКАЗАТЬ ПОБЕДУ* - чтобы получить анализ по драфту введи имена персонажей по их позиции и команде (имена нужно вводить без ошибок на английском как они записаны в самой доте)
*ЛАЙВ МАТЧИ* - список актуальных матчей с аналитикой
*СТАТИСТИКА* - статистика бота за определенный период/турнир (собирается только на турнирах 1/2 дивизионов)";
                    break;
                case "Предсказать победу":
                    _matchesResultService.NewMatch(chatId);
                    responseText = "Керри команды сил света: ";
                    break;
                case "Лайв матчи":
                    responseText = "Лайв матчи: ";
                    replyKeyboard = await _liveMatchesService.GetLiveMatchesKeyboard();
                    break;
                case "Статистика бота":
                    responseText = @"В моем коде заложена функция 
function SetWinChanсes() {
    let winChance = 100%
    let proebChance = 0%
}
поэтому проиграть невозможно, сегодня онли вин.....";
                    break;
                case "Назад":
                    responseText = "Меню: ";
                    break;
                default:
                    var heroCommand = _heroes.Value
                        .FirstOrDefault(h => h.Name == messageText
                        || h.LocalizedName == messageText);
                    if (heroCommand != null)
                    {
                        if (_matchesResultService.Matches.FirstOrDefault(m => m.chatId == chatId) != default &&
                            _matchesResultService.Matches.FirstOrDefault(m => m.chatId == chatId).match.HeroIds.Count == 9)
                        {
                            await _client.SendTextMessageAsync(chatId, "Подождите...");
                        }
                        responseText = await _matchesResultService.AlterMatch(chatId, heroCommand);
                        replyKeyboard = null;
                        break;
                    }

                    var matchCommand = _liveMatchesService.LiveMatches
                        .FirstOrDefault(m => $"{ m.RadiantTeam.Name } VS { m.DireTeam.Name }" == messageText);
                    if (matchCommand != null)
                    {
                        await _client.SendTextMessageAsync(chatId, "Подождите...");
                        responseText = await _matchesResultService.GetMatchResult(matchCommand, false);
                        break;
                    }
                    responseText = "Неизвестная команда...";
                    break;
            }
            await _client.SendTextMessageAsync(chatId, responseText, replyMarkup: replyKeyboard, parseMode: ParseMode.Markdown);
        }
    }
}
