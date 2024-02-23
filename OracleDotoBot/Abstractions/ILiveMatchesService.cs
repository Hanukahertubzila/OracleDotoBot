using OracleDotoBot.Domain.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Abstractions
{
    public interface ILiveMatchesService
    {
        Task<ReplyKeyboardMarkup> GetLiveMatchesKeyboard();

        List<Match> LiveMatches { get; }
    }
}