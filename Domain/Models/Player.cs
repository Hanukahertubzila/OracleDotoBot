using OracleDotoBot.Models;

namespace OracleDotoBot.Domain.Models
{
    public class Player
    {
        public Hero Hero { get; set; }

        public long AccountId { get; set; }

        public int HeroTotalMatchCount { get; set; }

        public int HeroWinMatchCount { get; set; }
    }
}
