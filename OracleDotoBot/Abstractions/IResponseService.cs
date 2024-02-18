using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Abstractions
{
    public interface IResponseService
    {
        Task<(string text, IReplyMarkup? replyMarkup)> GetResponse(string messageText, long chatId);
    }
}