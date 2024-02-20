using OracleDotoBot.Domain.Models;
using OracleDotoBot.StratzApiParser.Client;
using OracleDotoBot.StratzApiParser.Converters;
using OracleDotoBot.StratzApiParser.Enums;
using OracleDotoBot.StratzApiParser.OutputDataTypes;
using OracleDotoBot.StratzApiParser.Parsers;
using OracleDotoBot.StratzApiParser.Response_Object_Models;
using OracleDotoBot.StratzApiParser.ResponseObjectModels;

namespace OracleDotoBot.StratzApiParser.Api
{
    public class DotaApi
    {
        public DotaApi(string baseUrl, string token)
        {
            _client = new ApiClient(baseUrl, token);
        }

        private ApiClient _client;

        public async Task<(List<HeroStatistics> stats, string error)> GetMatchUpStatistics(Match match)
        {
            var idsString = "[";
            for (int i = 0; i < match.HeroIds.Count; i++)
            {
                idsString += match.HeroIds[i];
                if (i == match.HeroIds.Count - 1)
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
            
            var stats = ToHeroStatisticsCoverter.Covert(response.data, match);
            return (stats, "");
        }

        //TODO reafctor this
        public async Task<(LaningStatistics? stats, string error)> GetLaningStatistics(Match match)
        {
            var laningStatisticsResponse = new LaningStatisticsResponse();

            var responseMidVsMid = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos2HeroId, Positions.POSITION_2, false));
            if (!string.IsNullOrEmpty(responseMidVsMid.error))
                return (null, responseMidVsMid.error);
            laningStatisticsResponse.MidVsMid = responseMidVsMid.data;

            var responseCarryVsOfflane = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos1HeroId, Positions.POSITION_1, false));
            if (!string.IsNullOrEmpty(responseCarryVsOfflane.error))
                return (null, responseCarryVsOfflane.error);
            laningStatisticsResponse.CarryVsOfflane = responseCarryVsOfflane.data;

            var responseCarryVsSupp4 = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos1HeroId, Positions.POSITION_1, false));
            if (!string.IsNullOrEmpty(responseCarryVsSupp4.error))
                return (null, responseCarryVsSupp4.error);
            laningStatisticsResponse.CarryVsSupp4 = responseCarryVsSupp4.data;

            var responseSupp5VsSupp4 = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos5HeroId, Positions.POSITION_5, false));
            if (!string.IsNullOrEmpty(responseSupp5VsSupp4.error))
                return (null, responseSupp5VsSupp4.error);
            laningStatisticsResponse.Supp5VsSupp4 = responseSupp5VsSupp4.data;

            var responseSupp5VsOfflane = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos5HeroId, Positions.POSITION_5, false));
            if (!string.IsNullOrEmpty(responseSupp5VsOfflane.error))
                return (null, responseSupp5VsOfflane.error);
            laningStatisticsResponse.Supp5VsOfflane = responseSupp5VsOfflane.data;

            var responseOfflaneVsCarry = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos3HeroId, Positions.POSITION_3, false));
            if (!string.IsNullOrEmpty(responseOfflaneVsCarry.error))
                return (null, responseOfflaneVsCarry.error);
            laningStatisticsResponse.OfflaneVsCarry = responseOfflaneVsCarry.data;

            var responseOfflaneVsSupp5 = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos3HeroId, Positions.POSITION_3, false));
            if (!string.IsNullOrEmpty(responseOfflaneVsSupp5.error))
                return (null, responseOfflaneVsSupp5.error);
            laningStatisticsResponse.OfflaneVsSupp5 = responseOfflaneVsSupp5.data;

            var responseSupp4VsSupp5 = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos3HeroId, Positions.POSITION_4, false));
            if (!string.IsNullOrEmpty(responseSupp4VsSupp5.error))
                return (null, responseSupp4VsSupp5.error);
            laningStatisticsResponse.Supp4VsSupp5 = responseSupp4VsSupp5.data;

            var responseSupp4VsCarry = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos3HeroId, Positions.POSITION_4, false));
            if (!string.IsNullOrEmpty(responseSupp4VsCarry.error))
                return (null, responseSupp4VsCarry.error);
            laningStatisticsResponse.Supp4VsCarry = responseSupp4VsCarry.data;

            var stats = ToLaningStatisticsConverter.Covert(laningStatisticsResponse, match);

            if (!string.IsNullOrEmpty(stats.error))
                return (null, stats.error);
            return (stats.stats, "");
        }

        private string GetLaningQuery(int heroId, Positions position, bool isWith)
        {
            string query = $@"
                {{
                  heroStats {{
                   laneOutcome(heroId: { heroId } positionIds: { position } bracketBasicIds: DIVINE_IMMORTAL isWith: { isWith.ToString().ToLower() }) {{
                    winCount
                    drawCount
                    lossCount
                    stompWinCount
                    stompLossCount 
                    heroId2
  	                }}
                  }} 
                }}
            ";

            return query;
        }
    }
}
