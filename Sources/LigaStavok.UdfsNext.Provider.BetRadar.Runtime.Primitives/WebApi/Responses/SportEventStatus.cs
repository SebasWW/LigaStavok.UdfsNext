using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class SportEventStatus
    {
        public double? AggregateAwayScore { get; set; }

        public double? AggregateHomeScore { get; set; }

        public string AggregateWinnerId { get; set; }

        public double? AwayScore { get; set; }

        public Clock Clock { get; set; }

        public double? HomeScore { get; set; }

        public string MatchStatus { get; set; }

        public int? MatchStatusCode { get; set; }

        public int? Period { get; set; }

        public IEnumerable<PeriodScore> PeriodScores { get; set; }
 
        public string Status { get; set; }

        public string WinnerId { get; set; }

        public string WinningReason { get; set; }

        public static SportEventStatus Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new SportEventStatus
            {
                AggregateAwayScore = dynamicXml.AggregateAwayScore<double?>(),
                AggregateHomeScore = dynamicXml.AggregateHomeScore<double?>(),
                AggregateWinnerId  = dynamicXml.AggregateWinnerId,
                AwayScore          = dynamicXml.AwayScore<double?>(),
                Clock              = Responses.Clock.Parse(dynamicXml.Clock),
                HomeScore          = dynamicXml.HomeScore<double?>(),
                MatchStatus        = dynamicXml.MatchStatus,
                MatchStatusCode    = dynamicXml.MatchStatusCode<int?>(),
                Period             = dynamicXml.Period<int?>(),
                PeriodScores       = PeriodScore.ParseList(dynamicXml.PeriodScores?.GetPeriodScoreList()),
                Status             = dynamicXml.Status,
                WinnerId           = dynamicXml.WinnerId,
                WinningReason      = dynamicXml.WinningReason
            };

            return builder;
        }

        public static IEnumerable<SportEventStatus> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<SportEventStatus>();
        }
    }
}