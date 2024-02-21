using Newtonsoft.Json;
using OracleDotoBot.StratzApiParser.OutputDataTypes;

namespace OracleDotoBot.StratzApiParser.ResponseObjectModels
{
    public class LaningStatisticsResponse
    {
        public LaningData MidVsMid { get; set; }

        public LaningData CarryVsOfflane { get; set; }

        public LaningData CarryVsSupp4 { get; set; }

        public LaningData Supp5VsSupp4 { get; set; }

        public LaningData Supp5VsOfflane { get; set; }

        public LaningData OfflaneVsCarry { get; set; }

        public LaningData OfflaneVsSupp5 { get; set; }

        public LaningData Supp4VsSupp5 { get; set; }

        public LaningData Supp4VsCarry { get; set; }

        public LaningData RadiantCarrySupp5 { get; set; }

        public LaningData DireCarrySupp5 { get; set; } 

        public LaningData RadiantOfflaneSupp4 { get; set; }

        public LaningData DireOfflaneSupp4 { get; set; }
    }

    public class LaningData
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("heroStats")]
        public HeroStats HeroStats { get; set; }
    }

    public class HeroStats
    {
        [JsonProperty("laneOutcome")]
        public LaneOutcome[] LaneOutcome { get; set; }
    }

    public class LaneOutcome
    {
        [JsonProperty("winCount")]
        public int WinCount { get; set; }

        [JsonProperty("drawCount")]
        public int DrawCount { get; set; }

        [JsonProperty("lossCount")]
        public int LossCount { get; set; }

        [JsonProperty("stompWinCount")]
        public int StompWinCount { get; set; }

        [JsonProperty("stompLossCount")]
        public int StompLossCount { get; set; }

        [JsonProperty("heroId2")]
        public int HeroId2 { get; set; }
    }
}
