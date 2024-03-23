using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;

namespace OracleDotoBot.Abstractions.Services
{
    public interface IMatchesResultService
    {
        Task<string> AlterMatch(long chatId, Hero hero);

        Task<string> GetMatchResultById(long id);

        Task<string> GetMatchResult(Match match, bool includeLaning, bool includePlayerPerformance);
        void NewMatch(long chatId);

        public List<(Match match, long chatId)> Matches { get; }
    }
}