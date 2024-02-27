using Newtonsoft.Json;

namespace OracleDotoBot.OpenDotaApiParser.ResponseObjectModels
{
    public class LiveMatchesResponse
    {
        [JsonProperty("match_id", NullValueHandling = NullValueHandling.Ignore)]
        public long MatchId { get; set; } = 0;

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public long Duration { get; set; } = 0;

        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public long StartTime { get; set; } = 0;

        [JsonProperty("radiant_team_id", NullValueHandling = NullValueHandling.Ignore)]
        public long RadiantTeamId { get; set; } = 0;

        [JsonProperty("radiant_name", NullValueHandling = NullValueHandling.Ignore)]
        public string RadiantName { get; set; }

        [JsonProperty("dire_team_id", NullValueHandling = NullValueHandling.Ignore)]
        public long DireTeamId { get; set; } = 0;

        [JsonProperty("dire_name", NullValueHandling = NullValueHandling.Ignore)]
        public string DireName { get; set; }

        [JsonProperty("leagueid", NullValueHandling = NullValueHandling.Ignore)]
        public long LeagueId { get; set; } = 0;

        [JsonProperty("league_name", NullValueHandling = NullValueHandling.Ignore)]
        public string LeagueName { get; set; }

        [JsonProperty("series_id", NullValueHandling = NullValueHandling.Ignore)]
        public long SeriesId { get; set; } = 0;

        [JsonProperty("series_type", NullValueHandling = NullValueHandling.Ignore)]
        public int SeriesType { get; set; } = 0;

        [JsonProperty("radiant_score", NullValueHandling = NullValueHandling.Ignore)]
        public int RadiantScore { get; set; } = 0;

        [JsonProperty("dire_score", NullValueHandling = NullValueHandling.Ignore)]
        public int DireScore { get; set; } = 0;

        [JsonProperty("radiant_win", NullValueHandling = NullValueHandling.Ignore)]
        public bool RadiantWin { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public int Version { get; set; } = 0;
    }
}
