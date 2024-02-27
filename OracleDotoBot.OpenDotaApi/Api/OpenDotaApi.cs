using OracleDotoBot.Domain.Models;
using OracleDotoBot.OpenDotaApiParser.Client;
using OracleDotoBot.OpenDotaApiParser.ResponseObjectModels;

namespace OracleDotoBot.OpenDotaApiParser.Api
{
    public class OpenDotaApi
    {
        public OpenDotaApi()
        {
            _apiClient = new ApiClient();
        }

        private readonly ApiClient _apiClient;

        public async Task<(List<League> data, string error)> GetLiveLeagues()
        {
            var response = await _apiClient.Request<List<LiveMatchesResponse>>("https://api.opendota.com/api/proMatches");

            if (!string.IsNullOrEmpty(response.error))
                return (new List<League>(), response.error);

            var leagues = response.data
                .GroupBy(x => x.LeagueName)
                .Select(d =>
                new League()
                {
                    Name = d.Key,
                    Matches = d.Select(x => ($"{ x.RadiantName } VS { x.DireName }", x.MatchId)).ToList()
                })
                .ToList();

            return (leagues, "");
        }
    }
}
