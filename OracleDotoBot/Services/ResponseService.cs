using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OracleDotoBot.Abstractions;
using OracleDotoBot.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Services
{
    public class ResponseService : IResponseService
    {
        public ResponseService(IOptions<List<Hero>> heroes, 
            IUserMatchesService userMatchesService,
            ILogger<ResponseService> logger)
        {
            _heroes = heroes;
            _userMatchesService = userMatchesService;
            _logger = logger;
        }

        private readonly IOptions<List<Hero>> _heroes;
        private readonly IUserMatchesService _userMatchesService;
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
                                            new KeyboardButton("Турниры"),
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
                    _userMatchesService.NewMatch(chatId);
                    responseText = "Керри команды сил света: ";
                    return (responseText, null);
                default:
                    var command = _heroes.Value
                        .FirstOrDefault(h => h.Name == messageText
                        || h.LocalizedName == messageText);
                    if (command != null)
                    {
                        responseText = await _userMatchesService.AlterMatch(chatId, command.Id);
                        return (responseText, null);
                    }
                    return ("Неизвестная команда...", replyKeyboard);
            }
        }
    }
}
