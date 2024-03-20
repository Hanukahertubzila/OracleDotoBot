using Newtonsoft.Json;
using OracleDotoBot.StratzApiParser.Enums;

namespace OracleDotoBot.StratzApiParser.ResponseObjectModels
{
    public class MatchResponse
    {
        [JsonProperty("data")]
        public DataR Data { get; set; }
    }

    public partial class DataR
    {
        [JsonProperty("match")]
        public MatchR Match { get; set; }
    }

    public partial class MatchR
    {
        [JsonProperty("radiantTeam")]
        public TeamR RadiantTeam { get; set; }

        [JsonProperty("direTeam")]
        public TeamR DireTeam { get; set; }

        [JsonProperty("players")]
        public List<PlayerR> Players { get; set; }
    }

    public partial class TeamR
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class PlayerR
    {
        [JsonProperty("isRadiant")]
        public bool IsRadiant { get; set; }

        [JsonProperty("position")]
        public Positions Position { get; set; }

        [JsonProperty("heroId")]
        public long HeroId { get; set; }
    }
}
