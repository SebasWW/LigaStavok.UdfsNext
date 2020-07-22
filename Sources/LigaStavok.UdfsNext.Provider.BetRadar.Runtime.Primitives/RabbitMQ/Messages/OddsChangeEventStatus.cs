using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class OddsChangeEventStatus
    {
 
        public Dictionary<string, string> Claims { get; set; }

        public OddsChangeClock Clock { get; set; }

        public int MatchStatus { get; set; }

        public IEnumerable<OddsChangePeriodScore> PeriodScores { get; set; }

        public IEnumerable<OddsChangeResult> Results { get; set; }

        public OddsChangeStatistics Statistics { get; set; }

        public GameEventStatus Status { get; set; }

		public double? AwayScore { get; set; }

		public double? HomeScore { get; set; }

		public double? AwayGameScore { get; set; }

		public double? HomeGameScore { get; set; }

		public double? AwayPenaltyScore { get; set; }

		public double? HomePenaltyScore { get; set; }

		public byte? CurrentServer { get; set; }


		
        public static OddsChangeEventStatus Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new OddsChangeEventStatus
            {
                Claims       = ParseClaims(),
                Clock        = OddsChangeClock.Parse(dynamicXml.Clock),
                MatchStatus  = dynamicXml.MatchStatus<int>(),
                PeriodScores = OddsChangePeriodScore.ParseList(dynamicXml.PeriodScores?.GetPeriodScoreList()),
                Results      = OddsChangeResult.ParseList(dynamicXml.Results?.GetResultList()),
                Statistics   = OddsChangeStatistics.Parse(dynamicXml.Statistics),
                Status       = dynamicXml.Status<GameEventStatus>(),
				AwayScore	= dynamicXml.AwayScore<double?>(),
				HomeScore	= dynamicXml.HomeScore<double?>(),
				AwayGameScore = dynamicXml.AwayGamescore<double?>(),
				HomeGameScore = dynamicXml.HomeGamescore<double?>(),
				AwayPenaltyScore = dynamicXml.AwayPenaltyScore<double?>(),
				HomePenaltyScore = dynamicXml.HomePenaltyScore<double?>(),
				CurrentServer = dynamicXml.CurrentServer<byte?>(),
			};

            return builder;

            Dictionary<string, string> ParseClaims()
            {
                return ((IDictionary<string, string>)dynamicXml.GetAttributes())
                    .Where(x => x.Key != "status" && x.Key != "match_status")
                    .ToDictionary(k => k.Key, v => v.Value);
            }
        }
    }
}