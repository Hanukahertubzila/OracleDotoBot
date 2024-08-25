using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OracleDotoBot.Abstractions.Services;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Services
{
    public class MessagesRecieverService : IHostedService
    {
        public MessagesRecieverService(ITelegramBotClient client, 
            IResponseService responseService,
            ILiveMatchesService liveMatchesService,
            IUsersService usersService,
            ILogger<MessagesRecieverService> logger)
        {
            _client = client;
            _responseService = responseService;
            _liveMatchesService = liveMatchesService;
            _usersService = usersService;
            _logger = logger;
        }

        private readonly ITelegramBotClient _client;
        private readonly IResponseService _responseService;
        private readonly ILiveMatchesService _liveMatchesService;
        private readonly IUsersService _usersService;
        private readonly ILogger<MessagesRecieverService> _logger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = new[]
            {           
                UpdateType.Message,
                UpdateType.PreCheckoutQuery
            },
                ThrowPendingUpdates = true
            };
            using var cts = new CancellationTokenSource();
            _client.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cts.Token);
            var me = await _client.GetMeAsync();
            _logger.LogInformation(me.Username + " started!");

            await _usersService.UpdateActiveUsersList();
            await _liveMatchesService.UpdateLiveMatches();

            var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

            while (await timer.WaitForNextTickAsync() && !Console.KeyAvailable)
                await _liveMatchesService.UpdateLiveMatches();
            
            cts.Cancel();
        }

        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cts)
        {
            try
            {
                var message = update.Message;
                switch (update.Type)
                {
                    case UpdateType.Message:
                        var chat = message.Chat;
                        _logger.LogInformation("Message recieved! [" + message.Text + "]");
                        if (message.SuccessfulPayment != null)
                        {
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
                            var subscriptionResponse = await _usersService.UpdateSubscription(int.Parse(message.SuccessfulPayment.InvoicePayload), message.From.Id);
                            await _client.SendTextMessageAsync(chat.Id, subscriptionResponse, replyMarkup: replyKeyboard);
                            break;
                        }
                        var unsubCommands = new List<string>()
                        {
                            "Подписка",
                            "Назад",
                            "get my id",
                            "Помощь",
                            "Статистика",
                            "Продлить на неделю: 249₽",
                            "Продлить на две недели: 399₽",
                            "Продлить на месяц: 699₽"
                        };
                        if (await _usersService.CheckSubscription(message.From.Id) || unsubCommands.Contains(message.Text)
                            || _usersService.VipUsers.Contains(message.From.FirstName))
                            await _responseService.GetResponse(message.Text, chat.Id, message.From.Id, message.From.Username);
                        else
                            await _client.SendTextMessageAsync(chat.Id, "Ваша подписка на бота закончилась! Чтобы снова начать пользоваться функциями бота произведите оплату...");
                        break;
                    case UpdateType.PreCheckoutQuery:
                        await _client.AnswerPreCheckoutQueryAsync(update.PreCheckoutQuery.Id);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        private Task ErrorHandler(ITelegramBotClient client, Exception error, CancellationToken cts)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            _logger.LogError(ErrorMessage);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
