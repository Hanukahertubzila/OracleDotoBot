using OracleDotoBot.Domain.Models;
using OracleDotoBot.StratzApi.OutputDataTypes;
using OracleDotoBot.StratzApi.ResponseObjectModels;
using OracleDotoBot.StratzApiParser.Client;
using OracleDotoBot.StratzApiParser.Converters;
using OracleDotoBot.StratzApiParser.Enums;
using OracleDotoBot.StratzApiParser.OutputDataTypes;
using OracleDotoBot.StratzApiParser.Parsers;
using OracleDotoBot.StratzApiParser.Response_Object_Models;
using OracleDotoBot.StratzApiParser.ResponseObjectModels;

namespace OracleDotoBot.StratzApiParser.Api
{
    public class StratzDotaApi
    {
        public StratzDotaApi(string baseUrl, string token)
        {
            _client = new ApiClient(baseUrl, token);
        }

        private readonly ApiClient _client;

        public async Task<(List<PlayerPerformance> match, string error)> GetPlayerHeroPerformance(Match match)
        {
            var playerPerformances = new List<PlayerPerformance>();
            var rpos1 = await _client.Request<HeroPerformanceResponse>
                (GetHeroPerformanceQuery(match.RadiantTeam.Pos1.Hero.Id, 
                match.RadiantTeam.Pos1.AccountId));
            if (!string.IsNullOrEmpty(rpos1.error))
                return (playerPerformances, rpos1.error);
            playerPerformances.Add(new PlayerPerformance()
            {
                HeroId = match.RadiantTeam.Pos1.Hero.Id,
                TotalMatchCount = rpos1.data.Data.Player.HeroPerformance.MatchCount,
                WinMatchCount = rpos1.data.Data.Player.HeroPerformance.WinCount
            });

            var rpos2 = await _client.Request<HeroPerformanceResponse>
                (GetHeroPerformanceQuery(match.RadiantTeam.Pos2.Hero.Id,
                match.RadiantTeam.Pos2.AccountId));
            if (!string.IsNullOrEmpty(rpos2.error))
                return (playerPerformances, rpos2.error);
            playerPerformances.Add(new PlayerPerformance()
            {
                HeroId = match.RadiantTeam.Pos2.Hero.Id,
                TotalMatchCount = rpos2.data.Data.Player.HeroPerformance.MatchCount,
                WinMatchCount = rpos2.data.Data.Player.HeroPerformance.WinCount
            });

            var rpos3 = await _client.Request<HeroPerformanceResponse>
                (GetHeroPerformanceQuery(match.RadiantTeam.Pos3.Hero.Id,
                match.RadiantTeam.Pos3.AccountId));
            if (!string.IsNullOrEmpty(rpos3.error))
                return (playerPerformances, rpos3.error);
            playerPerformances.Add(new PlayerPerformance()
            {
                HeroId = match.RadiantTeam.Pos3.Hero.Id,
                TotalMatchCount = rpos3.data.Data.Player.HeroPerformance.MatchCount,
                WinMatchCount = rpos3.data.Data.Player.HeroPerformance.WinCount
            });

            var rpos4 = await _client.Request<HeroPerformanceResponse>
                (GetHeroPerformanceQuery(match.RadiantTeam.Pos4.Hero.Id,
                match.RadiantTeam.Pos4.AccountId));
            if (!string.IsNullOrEmpty(rpos4.error))
                return (playerPerformances, rpos4.error);
            playerPerformances.Add(new PlayerPerformance()
            {
                HeroId = match.RadiantTeam.Pos4.Hero.Id,
                TotalMatchCount = rpos4.data.Data.Player.HeroPerformance.MatchCount,
                WinMatchCount = rpos4.data.Data.Player.HeroPerformance.WinCount
            });

            var rpos5 = await _client.Request<HeroPerformanceResponse>
                (GetHeroPerformanceQuery(match.RadiantTeam.Pos5.Hero.Id,
                match.RadiantTeam.Pos5.AccountId));
            if (!string.IsNullOrEmpty(rpos5.error))
                return (playerPerformances, rpos5.error);
            playerPerformances.Add(new PlayerPerformance()
            {
                HeroId = match.RadiantTeam.Pos5.Hero.Id,
                TotalMatchCount = rpos5.data.Data.Player.HeroPerformance.MatchCount,
                WinMatchCount = rpos5.data.Data.Player.HeroPerformance.WinCount
            });

            var dpos1 = await _client.Request<HeroPerformanceResponse>
                (GetHeroPerformanceQuery(match.DireTeam.Pos1.Hero.Id,
                match.DireTeam.Pos1.AccountId));
            if (!string.IsNullOrEmpty(dpos1.error))
                return (playerPerformances, dpos1.error);
            playerPerformances.Add(new PlayerPerformance()
            {
                HeroId = match.DireTeam.Pos1.Hero.Id,
                TotalMatchCount = dpos1.data.Data.Player.HeroPerformance.MatchCount,
                WinMatchCount = dpos1.data.Data.Player.HeroPerformance.WinCount
            });

            var dpos2 = await _client.Request<HeroPerformanceResponse>
                (GetHeroPerformanceQuery(match.DireTeam.Pos2.Hero.Id,
                match.DireTeam.Pos2.AccountId));
            if (!string.IsNullOrEmpty(dpos2.error))
                return (playerPerformances, dpos2.error);
            playerPerformances.Add(new PlayerPerformance()
            {
                HeroId = match.DireTeam.Pos2.Hero.Id,
                TotalMatchCount = dpos2.data.Data.Player.HeroPerformance.MatchCount,
                WinMatchCount = dpos2.data.Data.Player.HeroPerformance.WinCount
            });

            var dpos3 = await _client.Request<HeroPerformanceResponse>
                (GetHeroPerformanceQuery(match.DireTeam.Pos3.Hero.Id,
                match.DireTeam.Pos3.AccountId));
            if (!string.IsNullOrEmpty(dpos3.error))
                return (playerPerformances, dpos3.error);
            playerPerformances.Add(new PlayerPerformance()
            {
                HeroId = match.DireTeam.Pos3.Hero.Id,
                TotalMatchCount = dpos3.data.Data.Player.HeroPerformance.MatchCount,
                WinMatchCount = dpos3.data.Data.Player.HeroPerformance.WinCount
            });

            var dpos4 = await _client.Request<HeroPerformanceResponse>
                (GetHeroPerformanceQuery(match.DireTeam.Pos4.Hero.Id,
                match.DireTeam.Pos4.AccountId));
            if (!string.IsNullOrEmpty(dpos4.error))
                return (playerPerformances, dpos4.error);
            playerPerformances.Add(new PlayerPerformance()
            {
                HeroId = match.DireTeam.Pos4.Hero.Id,
                TotalMatchCount = dpos4.data.Data.Player.HeroPerformance.MatchCount,
                WinMatchCount = dpos4.data.Data.Player.HeroPerformance.WinCount
            });

            var dpos5 = await _client.Request<HeroPerformanceResponse>
                (GetHeroPerformanceQuery(match.DireTeam.Pos5.Hero.Id,
                match.DireTeam.Pos5.AccountId));
            if (!string.IsNullOrEmpty(dpos5.error))
                return (playerPerformances, dpos5.error);
            playerPerformances.Add(new PlayerPerformance()
            {
                HeroId = match.DireTeam.Pos5.Hero.Id,
                TotalMatchCount = dpos5.data.Data.Player.HeroPerformance.MatchCount,
                WinMatchCount = dpos5.data.Data.Player.HeroPerformance.WinCount
            });

            return (playerPerformances, "");
        }

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

        private string GetHeroPerformanceQuery(int heroId, long steamId)
        {
            string query = $@"
                {{
	                player (steamAccountId : { steamId }) {{
		                heroPerformance (heroId : { heroId }  request : {{ isLeague : true take : 100 }}) {{
                      winCount
                      matchCount
                    }}
                  }}
                }}
            ";

            return query;
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
