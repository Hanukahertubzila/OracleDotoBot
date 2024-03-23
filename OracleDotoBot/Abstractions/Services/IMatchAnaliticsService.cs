using OracleDotoBot.Domain.Models;

namespace OracleDotoBot.Abstractions.Services
{
    public interface IMatchAnaliticsService
    {
        Task<string> GetMatchAnalitics(Match match, bool includeMatchUp, bool includeLaning, bool includePlayerPerformance);
    }
}