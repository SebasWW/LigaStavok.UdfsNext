using System;
using System.Collections.Generic;
using System.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
	public sealed class OddsChangeResult
    {
        public decimal AwayScore { get; set; }

        public decimal HomeScore { get; set; }

        public int Status { get; set; }

        public static OddsChangeResult Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new OddsChangeResult
            {
                Status    = dynamicXml.MatchStatusCode<int>(),
                HomeScore = dynamicXml.HomeScore<decimal>(),
                AwayScore = dynamicXml.AwayScore<decimal>()
            };

            return builder;
        }

        
        public static IEnumerable<OddsChangeResult> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select<dynamic, OddsChangeResult>(Parse).ToArray()
                   ?? Array.Empty<OddsChangeResult>();
        }
    }
}