using Newtonsoft.Json;

namespace OracleDotoBot.StratzApi.ResponseObjectModels
{
    public class HeroPerformanceResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("player")]
        public Player Player { get; set; }
    }

    public partial class Player
    {
        [JsonProperty("heroPerformance")]
        public HeroPerformance HeroPerformance { get; set; } = new HeroPerformance();
    }

    public partial class HeroPerformance
    {
        [JsonProperty("winCount")]
        public int WinCount { get; set; } = 0;

        [JsonProperty("matchCount")]
        public int MatchCount { get; set; } = 0;
    }
}
