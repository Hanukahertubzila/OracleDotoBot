using Newtonsoft.Json;

namespace OracleDotoBot.StratzApiParser.Response_Object_Models
{
    public class MatchUpStatisticsResponse
    {
        [JsonProperty("data")]
        public DataR Data { get; set; }
    }

    public partial class DataR
    {

        [JsonProperty("heroStats")]
        public HeroStatsR Stats { get; set; }
    }

    public class HeroStatsR
    {
        [JsonProperty("matchUp")]
        public MatchUpR[] MatchUp { get; set; }
    }

    public class MatchUpR
    {
        [JsonProperty("with")]
        public StatsR[] With { get; set; }

        [JsonProperty("vs")]
        public StatsR[] Vs { get; set; }
    }

    public class StatsR
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
