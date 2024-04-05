namespace OracleDotoBot.Domain.Models
{
    public class Match
    {
        public long Id { get ; set; }

        public Team RadiantTeam { get; set; } = new Team();

        public Team DireTeam { get; set; } = new Team();

        public List<int> HeroIds { get; set; } = new List<int>();
    }
}
