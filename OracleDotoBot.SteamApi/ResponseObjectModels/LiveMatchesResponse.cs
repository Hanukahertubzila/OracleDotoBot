using Newtonsoft.Json;

namespace OracleDotoBot.SteamApi.ResponseObjectModels
{
    public class LiveMatchesResponse
    {
        [JsonProperty("game_list")]
        public List<GameResponse> GameList { get; set; }
    }

    public class GameResponse
    {
        [JsonProperty("team_name_radiant")]
        public string RadiantName { get; set; } = string.Empty;

        [JsonProperty("team_name_dire")]
        public string DireName { get; set; } = string.Empty;

        [JsonProperty("players")]
        public List<PlayerResponse> Players { get; set; } = new List<PlayerResponse>();
    }

    public class PlayerResponse
    {
        [JsonProperty("account_id")]
        public long AccountId { get; set; }

        [JsonProperty("hero_id")]
        public int HeroId { get; set; }

        [JsonProperty("team_slot")]
        public int TeamSlot { get; set; }

        [JsonProperty("team")]
        public bool IsRadiant { get; set; } 
    }
}
