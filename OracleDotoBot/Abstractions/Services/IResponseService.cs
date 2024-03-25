using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Abstractions.Services
{
    public interface IResponseService
    {
        Task GetResponse(string messageText, long chatId, long userId);
    }
}