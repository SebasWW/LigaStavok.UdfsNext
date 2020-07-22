using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;

using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    [ImmutableObject(true)]
    public sealed class BetSettlementMarket : IMarket
    {
        public BetSettlementMarket(Builder builder)
        {
            Id = builder.Id;
            Outcomes = builder.Outcomes;
            Specifiers = builder.Specifiers;
            VoidReason = builder.VoidReason;
        }
		
        public BetSettlementMarket(int id, IEnumerable<BetSettlementOutcome> outcomes, string specifiers, string voidReason)
        {
            Id = id;
            Outcomes = outcomes;
            Specifiers = specifiers;
            VoidReason = voidReason;
        }

        public class Builder
        {
            public int Id { get; set; }

            public IEnumerable<BetSettlementOutcome> Outcomes { get; set; }

            public string Specifiers { get; set; }
            public string VoidReason { get; set; }

            public BetSettlementMarket ToImmutable()
            {
                return new BetSettlementMarket(this);
            }
        }

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

            var builder = new Builder
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
                   ?? IEnumerable<BetSettlementMarket>.Empty;
        }
    }
}