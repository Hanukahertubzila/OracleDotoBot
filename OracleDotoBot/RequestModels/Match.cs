namespace OracleDotoBot.RequestModels
{
    public class Match
    {
        public Team RadiantTeam { get; set; } = new Team();

        public Team DireTeam { get; set; } = new Team();

        public HashSet<int> HeroIds { get; set; } = new HashSet<int>();
    }
}
