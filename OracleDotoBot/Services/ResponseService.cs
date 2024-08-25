using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
            IUsersService usersService)
        {
            _heroes = heroes;
            _matchesResultService = matchesResultService;
            _liveMatchesService = liveMatchesService;
            _client = client;
            _userService = usersService;
        }

        private readonly IOptions<List<Hero>> _heroes;
        private readonly IMatchesResultService _matchesResultService;
        private readonly ILiveMatchesService _liveMatchesService;
        private readonly ITelegramBotClient _client;
        private readonly IUsersService _userService;

        public async Task GetResponse(string messageText, long chatId, long userId, string userName)
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
                    new KeyboardButton("Подписка"),
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

            // add X days to XXX
            // get users count
            // 17579317059
            if (_userService.Users.FirstOrDefault(u => u.Id == userId)?.Role == DAL.Enums.Roles.Admin)
            {
                if (messageText.Contains("days"))
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
                else if (messageText == "get users count")
                {
                    var count = await _userService.GetTotalUserCount();
                    await _client.SendTextMessageAsync(chatId, $"Общее число пользователей: {count}", replyMarkup: replyKeyboard, parseMode: ParseMode.Markdown);
                    return;
                }
                else if (messageText.Contains("add vip") && messageText.Split(' ').Length > 2)
                {
                    _userService.VipUsers.Add(messageText.Split(' ')[2]);
                    await _client.SendTextMessageAsync(chatId, "Vip добавлен");
                    return;
                }
                else
                {
                    long id = 0;
                    if (long.TryParse(messageText, out id))
                    {
                        await _client.SendTextMessageAsync(chatId, "Готовим аналитику...");
                        await _client.SendTextMessageAsync(chatId, await _matchesResultService.GetMatchResultById(id), replyMarkup: replyKeyboard, parseMode: ParseMode.Markdown);
                        return;
                    }
                }
            }

            switch (messageText)
            {
                case "/start":
                    responseText = @"Привет лудик! 
ВАЖНО ПРОЧИТАТЬ: 
Бот служит помощником в анализе драфта, и хотя я не учил его проигрывать, никаких 100% результатов он не дает, внимательно читай аналитику и принимай решение сам, основываясь на статистике. Оценка драфта и формирование результатов может занять некоторое время... 
удачи <3";
                    await _client.SendTextMessageAsync(chatId, responseText);
                    responseText = "Если вы используете бота впервые, у вас есть 2 бесплатных дня аналитики, по их прошествию, если вы захотите дальше пользоваться ботом, вам придется купить подписку";
                    break;
                case "Помощь":
                    responseText = $@"
*ЛАЙВ МАТЧИ* - список актуальных матчей с аналитикой
*ВВЕСТИ ГЕРОЕВ* - чтобы получить анализ по драфту, введи имена персонажей по их позиции и команде (имена нужно вводить без ошибок на английском как они записаны в самой доте)
*СТАТИСТИКА* - статистика бота за определенный период/турнир (собирается только на турнирах 1/2 дивизионов)
*ПОДПИСКА* - проверить дату окончания текущей подписки или оплатить новую

В случае технических проблем пишите: @Hanukahertubzila";
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
                    if (_userService.VipUsers.Contains(userName))
                    {
                        responseText = "У вас неограниченная подписка на бота";
                        break;
                    }
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
                    new KeyboardButton("Продлить на месяц: 599₽")
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
                case "no match":
                    responseText = "Нет матча...";
                    break;
                case "Продлить на неделю: 249₽":
                    var providerData = "{\r\n    \"receipt\": {\r\n        \"items\": [{\r\n            \"description\": \"Оплата подписки на 7 дней\",\r\n            \"quantity\": \"1.00\",\r\n            \"amount\": {\r\n                \"value\": \"249.00\",\r\n                \"currency\": \"RUB\"\r\n            },\r\n            \"vat_code\": 1\r\n        }]\r\n    }\r\n}";
                    await _client.SendInvoiceAsync(chatId, 
                        "Оплата подписки на 7 дней", 
                        "Продлить подписку на неделю (возможность пользоваться ботом 7 дней, если ваша текущая подписка еще не закончилась, время будет продлено)", 
                        "7", "390540012:LIVE:48849", "RUB", 
                        new List<LabeledPrice>() { new LabeledPrice("Руб", 24900) }, 
                        needEmail: true, sendEmailToProvider: true,
                        providerData: providerData);
                    return;
                case "Продлить на две недели: 399₽":
                    providerData = "{\r\n    \"receipt\": {\r\n        \"items\": [{\r\n            \"description\": \"Оплата подписки на 14 дней\",\r\n            \"quantity\": \"1.00\",\r\n            \"amount\": {\r\n                \"value\": \"399.00\",\r\n                \"currency\": \"RUB\"\r\n            },\r\n            \"vat_code\": 1\r\n        }]\r\n    }\r\n}";
                    await _client.SendInvoiceAsync(chatId, 
                        "Оплата подписки на 14 дней", 
                        "Продлить подписку на две недели (возможность пользоваться ботом 14 дней, если ваша текущая подписка еще не закончилась, время будет продлено)", 
                        "14", "390540012:LIVE:48849", "RUB", 
                        new List<LabeledPrice>() { new LabeledPrice("Руб", 39900) }, 
                        needEmail: true, sendEmailToProvider: true,
                        providerData: providerData);
                    return;
                case "Продлить на месяц: 599₽":
                    providerData = "{\r\n    \"receipt\": {\r\n        \"items\": [{\r\n            \"description\": \"Оплата подписки на 30 дней\",\r\n            \"quantity\": \"1.00\",\r\n            \"amount\": {\r\n                \"value\": \"599.00\",\r\n                \"currency\": \"RUB\"\r\n            },\r\n            \"vat_code\": 1\r\n        }]\r\n    }\r\n}";
                    await _client.SendInvoiceAsync(chatId, 
                        "Оплата подписки на 30 дней", 
                        "Продлить подписку на месяц (возможность пользоваться ботом 30 дней, если ваша текущая подписка еще не закончилась, время будет продлено)", 
                        "30", "390540012:LIVE:48849", "RUB", 
                        new List<LabeledPrice>() { new LabeledPrice("Руб", 59900) }, 
                        needEmail: true, sendEmailToProvider: true,
                        providerData: providerData);
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
                    var messageMatch = messageText.Replace(" ", string.Empty);

                    var matchCommand = _liveMatchesService.LiveMatches
                        .FirstOrDefault(m => messageMatch.Contains(m.match.RadiantTeam.Name.Replace(" ", string.Empty)) && 
                        messageMatch.Contains(m.match.DireTeam.Name.Replace(" ", string.Empty)));
                    if (matchCommand.match != null)
                    {
                        await _client.SendTextMessageAsync(chatId, "Готовим аналитику...");
                        responseText = matchCommand.analitics;
                        break;
                    }
                    responseText = "Неизвестная команда...";
                    break;
            }
            await _client.SendTextMessageAsync(chatId, responseText, replyMarkup: replyKeyboard, parseMode: ParseMode.Markdown);
        }
    }
}
