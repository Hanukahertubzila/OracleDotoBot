namespace OracleDotoBot.Domain.Models
{
    public class League
    {
        public string Name { get; set; } = string.Empty;

        public List<(string name, long id)> Matches { get; set; } = new List<(string name, long id)>();
    }
}
