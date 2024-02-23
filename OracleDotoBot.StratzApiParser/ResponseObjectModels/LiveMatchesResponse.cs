using Newtonsoft.Json;
using OracleDotoBot.StratzApiParser.Enums;

namespace OracleDotoBot.StratzApiParser.ResponseObjectModels
{
    public class LiveMatchesResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("live")]
        public Live Live { get; set; }
    }

    public partial class Live
    {
        [JsonProperty("matches", NullValueHandling = NullValueHandling.Ignore)]
        public List<LiveMatch> Matches { get; set; }
    }

    public partial class LiveMatch
    {
        [JsonProperty("gameState")]
        public string GameState { get; set; }

        [JsonProperty("radiantTeam", NullValueHandling = NullValueHandling.Ignore)]
        public Team RadiantTeam { get; set; }

        [JsonProperty("direTeam", NullValueHandling = NullValueHandling.Ignore)]
        public Team DireTeam { get; set; }

        [JsonProperty("players", NullValueHandling = NullValueHandling.Ignore)]
        public List<LivePlayer> Players { get; set; }
    }

    public partial class Team
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class LivePlayer
    {
        [JsonProperty("isRadiant")]
        public bool IsRadiant { get; set; }

        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public Positions Position { get; set; }

        [JsonProperty("heroId")]
        public long HeroId { get; set; }
    }
}
