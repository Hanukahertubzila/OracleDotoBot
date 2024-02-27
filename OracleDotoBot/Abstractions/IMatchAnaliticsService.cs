using OracleDotoBot.Domain.Models;

namespace OracleDotoBot.Abstractions
{
    public interface IMatchAnaliticsService
    {
        Task<string> GetMatchAnalitics(Match match, bool includeMatchUp, bool includeLaning);
    }
}