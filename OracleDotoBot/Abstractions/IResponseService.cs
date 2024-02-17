using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Abstractions
{
    public interface IResponseService
    {
        (string text, IReplyMarkup? replyMarkup) GetResponse(string messageText);
    }
}