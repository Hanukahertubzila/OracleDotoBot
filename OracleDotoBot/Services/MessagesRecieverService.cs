using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OracleDotoBot.Abstractions;
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
            ILogger<MessagesRecieverService> logger)
        {
            _client = client;
            _responseService = responseService;
            _logger = logger;
        }

        private readonly ITelegramBotClient _client;
        private readonly IResponseService _responseService;
        private readonly ILogger<MessagesRecieverService> _logger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = new[]
    {
                UpdateType.Message
            },
                ThrowPendingUpdates = true
            };
            using var cts = new CancellationTokenSource();
            _client.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cts.Token);
            var me = await _client.GetMeAsync();
            _logger.LogInformation(me.Username + " started!");

            Console.ReadLine();
            cts.Cancel();
        }

        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cts)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        var message = update.Message;
                        var chat = message.Chat;

                        _logger.LogInformation("Message recieved! [" + message.Text + "]");

                        var (responseText, replyMarkup) = await _responseService.GetResponse(message.Text, chat.Id);
                        await _client.SendTextMessageAsync(chat.Id, responseText, replyMarkup: replyMarkup, parseMode: ParseMode.Markdown);
                        return;
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
