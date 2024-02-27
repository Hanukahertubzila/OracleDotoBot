using OracleDotoBot.Domain.Models;
using OracleDotoBot.Models;
using OracleDotoBot.StratzApiParser.Client;
using OracleDotoBot.StratzApiParser.Converters;
using OracleDotoBot.StratzApiParser.Enums;
using OracleDotoBot.StratzApiParser.OutputDataTypes;
using OracleDotoBot.StratzApiParser.Parsers;
using OracleDotoBot.StratzApiParser.Response_Object_Models;
using OracleDotoBot.StratzApiParser.ResponseObjectModels;
using System.Text;
using Match = OracleDotoBot.Domain.Models.Match;

namespace OracleDotoBot.StratzApiParser.Api
{
    public class StratzDotaApi
    {
        public StratzDotaApi(string baseUrl, string token, List<Hero> heroes)
        {
            _client = new ApiClient(baseUrl, token);
            _heroes = heroes;
        }

        private readonly ApiClient _client;
        private readonly List<Hero> _heroes;

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

        public async Task<(LaningStatistics? stats, string error)> GetLaningStatistics(Match match)
        {
            var laningStatisticsResponse = new LaningStatisticsResponse();

            var responseMidVsMid = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos2.Hero.Id, Positions.POSITION_2, false));
            if (!string.IsNullOrEmpty(responseMidVsMid.error))
                return (null, responseMidVsMid.error);
            laningStatisticsResponse.MidVsMid = responseMidVsMid.data;

            var responseCarryVsOfflane = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos1.Hero.Id, Positions.POSITION_1, false));
            if (!string.IsNullOrEmpty(responseCarryVsOfflane.error))
                return (null, responseCarryVsOfflane.error);
            laningStatisticsResponse.CarryVsOfflane = responseCarryVsOfflane.data;

            var responseCarryVsSupp4 = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos1.Hero.Id, Positions.POSITION_1, false));
            if (!string.IsNullOrEmpty(responseCarryVsSupp4.error))
                return (null, responseCarryVsSupp4.error);
            laningStatisticsResponse.CarryVsSupp4 = responseCarryVsSupp4.data;

            var responseSupp5VsSupp4 = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos5.Hero.Id, Positions.POSITION_5, false));
            if (!string.IsNullOrEmpty(responseSupp5VsSupp4.error))
                return (null, responseSupp5VsSupp4.error);
            laningStatisticsResponse.Supp5VsSupp4 = responseSupp5VsSupp4.data;

            var responseSupp5VsOfflane = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos5.Hero.Id, Positions.POSITION_5, false));
            if (!string.IsNullOrEmpty(responseSupp5VsOfflane.error))
                return (null, responseSupp5VsOfflane.error);
            laningStatisticsResponse.Supp5VsOfflane = responseSupp5VsOfflane.data;

            var responseOfflaneVsCarry = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos3.Hero.Id, Positions.POSITION_3, false));
            if (!string.IsNullOrEmpty(responseOfflaneVsCarry.error))
                return (null, responseOfflaneVsCarry.error);
            laningStatisticsResponse.OfflaneVsCarry = responseOfflaneVsCarry.data;

            var responseOfflaneVsSupp5 = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos3.Hero.Id, Positions.POSITION_3, false));
            if (!string.IsNullOrEmpty(responseOfflaneVsSupp5.error))
                return (null, responseOfflaneVsSupp5.error);
            laningStatisticsResponse.OfflaneVsSupp5 = responseOfflaneVsSupp5.data;

            var responseSupp4VsSupp5 = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos4.Hero.Id, Positions.POSITION_4, false));
            if (!string.IsNullOrEmpty(responseSupp4VsSupp5.error))
                return (null, responseSupp4VsSupp5.error);
            laningStatisticsResponse.Supp4VsSupp5 = responseSupp4VsSupp5.data;

            var responseSupp4VsCarry = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos4.Hero.Id, Positions.POSITION_4, false));
            if (!string.IsNullOrEmpty(responseSupp4VsCarry.error))
                return (null, responseSupp4VsCarry.error);
            laningStatisticsResponse.Supp4VsCarry = responseSupp4VsCarry.data;

            var responseRadiantCarrySupp5 = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos1.Hero.Id, Positions.POSITION_1, true));
            if (!string.IsNullOrEmpty(responseRadiantCarrySupp5.error))
                return (null, responseRadiantCarrySupp5.error);
            laningStatisticsResponse.RadiantCarrySupp5 = responseRadiantCarrySupp5.data;

            var responseDireCarrySupp5 = await _client.Request<LaningData>(
                GetLaningQuery(match.DireTeam.Pos1.Hero.Id, Positions.POSITION_1, true));
            if (!string.IsNullOrEmpty(responseDireCarrySupp5.error))
                return (null, responseDireCarrySupp5.error);
            laningStatisticsResponse.DireCarrySupp5 = responseDireCarrySupp5.data;

            var responseRadiantOfflaneSupp4 = await _client.Request<LaningData>(
                GetLaningQuery(match.RadiantTeam.Pos3.Hero.Id, Positions.POSITION_3, true));
            if (!string.IsNullOrEmpty(responseRadiantOfflaneSupp4.error))
                return (null, responseRadiantOfflaneSupp4.error);
            laningStatisticsResponse.RadiantOfflaneSupp4 = responseRadiantOfflaneSupp4.data;

            var responseDireOfflaneSupp4 = await _client.Request<LaningData>(
                GetLaningQuery(match.DireTeam.Pos3.Hero.Id, Positions.POSITION_3, true));
            if (!string.IsNullOrEmpty(responseDireOfflaneSupp4.error))
                return (null, responseDireOfflaneSupp4.error);
            laningStatisticsResponse.DireOfflaneSupp4 = responseDireOfflaneSupp4.data;

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
