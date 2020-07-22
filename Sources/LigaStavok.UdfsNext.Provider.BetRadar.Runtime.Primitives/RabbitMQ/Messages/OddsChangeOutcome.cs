using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;

using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class OddsChangeOutcome : IOutcome
    {
        public bool Active { get; set; }

        public string Id { get; set; }

        public double? Odds { get; set; }

        public double? Probabilities { get; set; }

        public static OddsChangeOutcome Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var active  = (int)dynamicXml.Active<int>();
            var builder = new OddsChangeOutcome
            {
                Active        = active == 1,
                Id            = OutcomeIdParser.ParseId(dynamicXml.Id),
                Odds          = dynamicXml.Odds<double?>(),
                Probabilities = dynamicXml.Probabilities<double?>()
            };

           
            return builder;
        }

        
        public static IEnumerable<OddsChangeOutcome> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select<dynamic, OddsChangeOutcome>(Parse).Where(t => t.Odds.HasValue).ToArray()
                   ?? Array.Empty<OddsChangeOutcome>();
        }
    }
}