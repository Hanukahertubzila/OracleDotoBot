using OracleDotoBot.StratzApiParser.Client;
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

        public async Task<MatchUpStatisticsResponse> GetMatchUpStatistics(int[] heroIds)
        {
            var idsString = "[";
            for (int i = 0; i < heroIds.Length; i++)
            {
                idsString += heroIds[i];
                if (i == heroIds.Length - 1)
                    idsString += "]";
                else
                    idsString += ",";
            }
            string query = $@"
                {{
                  heroStats {{
		                matchUp(heroIds: {idsString} take: 140 bracketBasicIds: DIVINE_IMMORTAL){{
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
            return response.data;
        }
    }
}
