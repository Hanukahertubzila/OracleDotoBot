using Newtonsoft.Json;

namespace OracleDotoBot.StratzApiParser.Response_Object_Models
{
    public class MatchUpStatisticsResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {

        [JsonProperty("heroStats")]
        public HeroStats Stats { get; set; }
    }

    public class HeroStats
    {
        [JsonProperty("matchUp")]
        public MatchUp[] MatchUp { get; set; }
    }

    public class MatchUp
    {
        [JsonProperty("with")]
        public Stats[] With { get; set; }

        [JsonProperty("vs")]
        public Stats[] Vs { get; set; }
    }

    public class Stats
    {
        [JsonProperty("heroId1")]
        public int HeroId1 { get; set; }

        [JsonProperty("heroId2")]
        public int HeroId2 { get; set; }

        [JsonProperty("winsAverage")]
        public double WinsAverage { get; set; }

        [JsonProperty("winRateHeroId1")]
        public double WinRateHeroId1 { get; set; }

        [JsonProperty("winRateHeroId2")]
        public double WinRateHeroId2 { get; set; }
    }
}
