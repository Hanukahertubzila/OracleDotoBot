using Newtonsoft.Json;
using OracleDotoBot.StratzApiParser.Enums;

namespace OracleDotoBot.StratzApiParser.ResponseObjectModels
{
    public class MatchResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("match")]
        public MatchA Match { get; set; }
    }

    public partial class MatchA
    {
        [JsonProperty("radiantTeam")]
        public TeamA RadiantTeam { get; set; }

        [JsonProperty("direTeam")]
        public TeamA DireTeam { get; set; }

        [JsonProperty("players")]
        public List<PlayerA> Players { get; set; }
    }

    public partial class TeamA
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class PlayerA
    {
        [JsonProperty("isRadiant")]
        public bool IsRadiant { get; set; }

        [JsonProperty("position")]
        public Positions Position { get; set; }

        [JsonProperty("heroId")]
        public long HeroId { get; set; }
    }
}
