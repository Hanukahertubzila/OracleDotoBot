using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;
using OracleDotoBot.SteamApi.Client;
using OracleDotoBot.SteamApi.Converters;
using OracleDotoBot.SteamApi.ResponseObjectModels;

namespace OracleDotoBot.SteamApi.Api
{
    public class SteamDotaApi
    {
        public SteamDotaApi(string token, List<Hero> heroes)
        {
            _client = new ApiClient(token);
            _heroes = heroes;
        }

        private readonly ApiClient _client;
        private readonly List<Hero> _heroes;

        public async Task<(List<Match> data, string error)> GetLiveMatches()
        {
            var response = await _client.Request<LiveMatchesResponse>("https://api.steampowered.com/IDOTA2Match_570/GetTopLiveGame/v1/");

            if (!string.IsNullOrEmpty(response.error))
                return (new List<Match>(), response.error);

            var matches = ToMatchListConverter.Convert(response.data, _heroes);

            return (matches, "");
        }
    }
}
