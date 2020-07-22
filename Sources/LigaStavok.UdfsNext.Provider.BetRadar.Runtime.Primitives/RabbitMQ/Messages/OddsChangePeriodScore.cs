using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class OddsChangePeriodScore
    {
        public decimal AwayScore { get; set; }

        public decimal HomeScore { get; set; }

        public int Number { get; set; }

        public int Status { get; set; }

        public static OddsChangePeriodScore Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new OddsChangePeriodScore
            {
                Status    = dynamicXml.MatchStatusCode<int>(),
                Number    = dynamicXml.Number<int>(),
                HomeScore = dynamicXml.HomeScore<decimal>(),
                AwayScore = dynamicXml.AwayScore<decimal>()
            };

            return builder;
        }
        
        
        public static IEnumerable<OddsChangePeriodScore> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select<dynamic, OddsChangePeriodScore>(Parse).ToArray()
                   ?? Array.Empty<OddsChangePeriodScore>();
        }
    }
}