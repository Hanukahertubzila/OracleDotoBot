namespace OracleDotoBot.Domain.Models
{
    public class Team
    {
        public string Name { get; set; } = string.Empty;

        public Player Pos1 { get; set; }

        public Player Pos2 { get; set; }

        public Player Pos3 { get; set; }

        public Player Pos4 { get; set; }

        public Player Pos5 { get; set; }
    }
}
