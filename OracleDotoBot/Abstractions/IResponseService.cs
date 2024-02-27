using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Abstractions
{
    public interface IResponseService
    {
        Task GetResponse(string messageText, long chatId);
    }
}