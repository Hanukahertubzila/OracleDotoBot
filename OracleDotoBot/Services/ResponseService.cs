using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OracleDotoBot.Abstractions.Services;
using OracleDotoBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Services
{
    public class ResponseService : IResponseService
    {
        public ResponseService(IOptions<List<Hero>> heroes, 
            IMatchesResultService matchesResultService,
            ILiveMatchesService liveMatchesService,
            ITelegramBotClient client,
            IConfiguration configuration,
            IUsersService usersService)
        {
            _heroes = heroes;
            _matchesResultService = matchesResultService;
            _liveMatchesService = liveMatchesService;
            _client = client;
            _configuration = configuration;
            _userService = usersService;
        }

        private readonly IOptions<List<Hero>> _heroes;
        private readonly IMatchesResultService _matchesResultService;
        private readonly ILiveMatchesService _liveMatchesService;
        private readonly ITelegramBotClient _client;
        private readonly IConfiguration _configuration;
        private readonly IUsersService _userService;

        public async Task GetResponse(string messageText, long chatId, long userId)
        {
            var responseText = "";
            var replyKeyboard = new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                new KeyboardButton[]
                {
                    new KeyboardButton("Лайв матчи"),
                    new KeyboardButton("Ввести героев"),
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Статистика бота"),
                    new KeyboardButton("Подписка")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Помощь")
                }
                })
            {
                ResizeKeyboard = true,
            };

            // add X days to XXX
            if (messageText.Contains("days") && _userService.Users.First(u => u.Id == userId).Role == DAL.Enums.Roles.Admin)
            {
                var m = messageText.Split(' ');
                if (m.Length > 4)
                {
                    var daysCount = int.Parse(m[1]);
                    var id = long.Parse(m[4]);
                    await _userService.UpdateSubscription(daysCount, id);
                    await _client.SendTextMessageAsync(chatId, $"{id} подписка была продлена на {daysCount} дней", replyMarkup: replyKeyboard, parseMode: ParseMode.Markdown);
                    return;
                }
            }

            switch (messageText)
            {
                case "/start":
                    responseText = @"Привет лудик! 
ВАЖНО ПРОЧИТАТЬ: 
Бот служит помощником в анализе драфта, и хотя я не учил его проигрывать, никаких 100% результатов он не дает, внимательно читай аналитику и принимай решение сам, основываясь на цифрах. Оценка драфта и формирование результатов может занять некоторое время... 
удачи <3";
                    break;
                case "Помощь":
                    responseText = $@"
*ЛАЙВ МАТЧИ* - список актуальных матчей с аналитикой
*ВВЕСТИ ГЕРОЕВ* - чтобы получить анализ по драфту, введи имена персонажей по их позиции и команде (имена нужно вводить без ошибок на английском как они записаны в самой доте)
*СТАТИСТИКА* - статистика бота за определенный период/турнир (собирается только на турнирах 1/2 дивизионов)
*ПОДПИСКА* - проверить дату окончания текущей подписки или оплатить новую";
                    break;
                case "Ввести героев":
                    _matchesResultService.NewMatch(chatId);
                    responseText = "Керри команды сил света: ";
                    break;
                case "Лайв матчи":
                    responseText = "Лайв матчи: ";
                    replyKeyboard = _liveMatchesService.GetLiveMatchesKeyboard();
                    break;
                case "Статистика бота":
                    responseText = @"Вся статистика бота, а также новости о скидках и обновлениях в тг канале: 
https://t.me/WiseOracleIsHere";
                    break;
                case "Подписка":
                    responseText = await _userService.GetSubscriptionPeriod(userId);
                    replyKeyboard = new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                new KeyboardButton[]
                {
                    new KeyboardButton("Продлить на неделю: 249₽")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Продлить на две недели: 399₽")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Продлить на месяц: 699₽")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Назад")
                }
                })
                    {
                        ResizeKeyboard = true,
                    };
                    break;
                case "Назад":
                    responseText = "Меню: ";
                    break;
                case "Продлить на неделю: 249₽":
                    await _client.SendInvoiceAsync(chatId, "Оплата подписки", "Продлить подписку на неделю", "7", "1744374395:TEST:cee68c72e453188fd521", "RUB", new List<LabeledPrice>() { new LabeledPrice("Руб", 24900) });
                    return;
                case "Продлить на две недели: 399₽":
                    await _client.SendInvoiceAsync(chatId, "Оплата подписки", "Продлить подписку на две недели", "14", "1744374395:TEST:cee68c72e453188fd521", "RUB", new List<LabeledPrice>() { new LabeledPrice("Руб", 39900) });
                    return;
                case "Продлить на месяц: 699₽":
                    await _client.SendInvoiceAsync(chatId, "Оплата подписки", "Продлить подписку на месяц", "30", "1744374395:TEST:cee68c72e453188fd521", "RUB", new List<LabeledPrice>() { new LabeledPrice("Руб", 69900) });
                    return;
                case "get my id":
                    responseText = userId.ToString();
                    break;
                default:
                    var heroCommand = _heroes.Value
                        .FirstOrDefault(h => h.Name == messageText
                        || h.LocalizedName == messageText);
                    if (heroCommand != null)
                    {
                        if (_matchesResultService.Matches.FirstOrDefault(m => m.chatId == chatId) != default &&
                            _matchesResultService.Matches.FirstOrDefault(m => m.chatId == chatId).match.HeroIds.Count == 9 &&
                            !_matchesResultService.Matches.FirstOrDefault(m => m.chatId == chatId).match.HeroIds.Contains(heroCommand.Id))
                        {
                            await _client.SendTextMessageAsync(chatId, "Готовим аналитику...");
                        }
                        responseText = await _matchesResultService.AlterMatch(chatId, heroCommand);
                        replyKeyboard = null;
                        break;
                    }

                    var matchCommand = _liveMatchesService.LiveMatches
                        .FirstOrDefault(m => $"{ m.match.RadiantTeam.Name } VS { m.match.DireTeam.Name }" == messageText);
                    if (matchCommand.match != null)
                    {
                        await _client.SendTextMessageAsync(chatId, "Готовим аналитику...");
                        responseText = matchCommand.analitics;
                        break;
                    }
                    long id = 0;
                    if (long.TryParse(messageText, out id))
                    {
                        await _client.SendTextMessageAsync(chatId, "Готовим аналитику...");
                        responseText = await _matchesResultService.GetMatchResultById(id);
                        break;
                    }
                    responseText = "Неизвестная команда...";
                    break;
            }
            await _client.SendTextMessageAsync(chatId, responseText, replyMarkup: replyKeyboard, parseMode: ParseMode.Markdown);
        }
    }
}
