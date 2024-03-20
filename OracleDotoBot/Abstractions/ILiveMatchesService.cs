using OracleDotoBot.Domain.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace OracleDotoBot.Abstractions
{
    public interface ILiveMatchesService
    {
        ReplyKeyboardMarkup GetLiveMatchesKeyboard();

        Task UpdateLiveMatches();

        List<(Match match, string analitics)> LiveMatches { get; }
    }
}