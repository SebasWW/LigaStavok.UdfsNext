using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;

using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class BetSettlementMarket : IMarket
    {
        public int Id { get; set; }

        public IEnumerable<BetSettlementOutcome> Outcomes { get; set; }

        public string Specifiers { get; set; }
        public string VoidReason { get; set; }

        public static BetSettlementMarket Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new BetSettlementMarket
            {
                Id         = dynamicXml.Id<int>(),
                Outcomes   = BetSettlementOutcome.ParseList(dynamicXml.GetOutcomeList()),
                Specifiers = dynamicXml.Specifiers,
                VoidReason = dynamicXml.void_reason
            };

            return builder;
        }

        
        public static IEnumerable<BetSettlementMarket> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select<dynamic, BetSettlementMarket>(Parse).ToArray()
                   ?? Array.Empty<BetSettlementMarket>();
        }
    }
}