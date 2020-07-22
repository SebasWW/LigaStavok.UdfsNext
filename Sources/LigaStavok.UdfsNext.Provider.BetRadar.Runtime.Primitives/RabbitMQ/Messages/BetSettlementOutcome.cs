using System;
using System.Collections.Generic;
using System.Linq;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
	public sealed class BetSettlementOutcome : IOutcome
    {
        public double? DeadHeatFactor { get; set; }

        public string Id { get; set; }

        public int Result { get; set; }

        public double? VoidFactor { get; set; }

        public static BetSettlementOutcome Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new BetSettlementOutcome()
            {
                DeadHeatFactor = dynamicXml.DeadHeatFactor<double?>(),
                Id             = OutcomeIdParser.ParseId(dynamicXml.Id),
                Result         = dynamicXml.Result<int>(),
                VoidFactor     = dynamicXml.VoidFactor<double?>()
            };

            return builder;
        }

        public static IEnumerable<BetSettlementOutcome> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select<dynamic, BetSettlementOutcome>(Parse).ToArray()
                   ?? Array.Empty<BetSettlementOutcome>();
        }
    }
}