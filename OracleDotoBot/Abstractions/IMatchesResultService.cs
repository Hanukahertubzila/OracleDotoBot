using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;

namespace OracleDotoBot.Abstractions
{
    public interface IMatchesResultService
    {
        Task<string> AlterMatch(long chatId, Hero hero);

        Task<string> GetMatchResult(Match match);
        void NewMatch(long chatId);
    }
}