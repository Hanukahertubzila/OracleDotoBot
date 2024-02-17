using Microsoft.Extensions.Logging;
using OracleDotoBot.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Services
{
    public class ResponseService : IResponseService
    {
        public ResponseService(ILogger<ResponseService> logger)
        {
            _logger = logger;
        }

        private readonly ILogger<ResponseService> _logger;

        public (string text, IReplyMarkup? replyMarkup) GetResponse(string messageText)
        {
            switch (messageText)
            {
                case "/start":
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

                    return ("Привет лудик!", replyKeyboard);
                default:
                    return ("AHAHAHHA", null);
            }
        }
    }
}
