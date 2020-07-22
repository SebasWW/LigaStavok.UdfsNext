using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;

using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class OddsChangeMarket : IMarket
    {
        public bool Favourite { get; set; }

        public int Id { get; set; }
        
        public IEnumerable<OddsChangeOutcome> Outcomes { get; set; }

        public string Specifiers { get; set; }

        public OddsChangeMarketStatus? Status { get; set; }

        public static OddsChangeMarket Parse(dynamic dynamicXml)
        {
            var builder = new OddsChangeMarket
            {
                Id         = dynamicXml.Id<int>(),
                Favourite  = dynamicXml.Favourite<int?>() == 1,
                Outcomes   = OddsChangeOutcome.ParseList(dynamicXml.GetOutcomeList()),
                Specifiers = dynamicXml.Specifiers,
                Status     = dynamicXml.Status<OddsChangeMarketStatus?>()
            };

            return builder;
        }

        
        public static IEnumerable<OddsChangeMarket> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select<dynamic, OddsChangeMarket>(Parse).ToArray()
                   ?? Array.Empty<OddsChangeMarket>();
        }
    }
}