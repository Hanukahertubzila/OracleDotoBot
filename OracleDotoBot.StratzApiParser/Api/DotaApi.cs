using OracleDotoBot.StratzApiParser.Client;
using OracleDotoBot.StratzApiParser.OutputDataTypes;
using OracleDotoBot.StratzApiParser.Parsers;
using OracleDotoBot.StratzApiParser.Response_Object_Models;

namespace OracleDotoBot.StratzApiParser.Api
{
    public class DotaApi
    {
        public DotaApi(string baseUrl, string token)
        {
            _client = new ApiClient(baseUrl, token);
        }

        private ApiClient _client;

        public async Task<(List<HeroStatistics> stats, string error)> GetMatchUpStatistics(List<int> heroIds)
        {
            var idsString = "[";
            for (int i = 0; i < heroIds.Count; i++)
            {
                idsString += heroIds[i];
                if (i == heroIds.Count - 1)
                    idsString += "]";
                else
                    idsString += ",";
            }
            string query = $@"
                {{
                  heroStats {{
		                matchUp(heroIds: {idsString} take: 10000 bracketBasicIds: DIVINE_IMMORTAL){{
     	                vs {{
                        heroId1
                        heroId2
                        winsAverage
                        winRateHeroId1
                        winRateHeroId2
                      }}
                      with {{
                        heroId1
                        heroId2
                        winsAverage
                        winRateHeroId1
                        winRateHeroId2
                      }}
                    }}    	
                  }}
                }}
            ";

            var response = await _client.Request<MatchUpStatisticsResponse>(query);
            if (!string.IsNullOrEmpty(response.error))
                return (new List<HeroStatistics>(), response.error);
            
            var stats = ToHeroStatisticsCoverter.Covert(response.data, heroIds);
            return (stats, "");
        }
    }
}
