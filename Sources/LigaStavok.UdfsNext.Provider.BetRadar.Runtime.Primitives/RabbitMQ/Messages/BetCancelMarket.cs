using System;
using System.Collections.Generic;
using System.Linq;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class BetCancelMarket : IMarket
    {
        /// <summary>
        ///    Unique bet type identifier
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        ///    Market line unique identifier
        /// </summary>
        public string Specifiers { get; set; }
        
        /// <summary>
        /// Reason of void
        /// </summary>
        public string VoidReason { get; set; }

        public static BetCancelMarket Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new BetCancelMarket
            {
                Id         = dynamicXml.Id<int>(),
                Specifiers = dynamicXml.Specifiers,
                VoidReason = dynamicXml.void_reason
            };

            return builder;
        }
        
        public static IEnumerable<BetCancelMarket> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select<dynamic, BetCancelMarket>(Parse).ToArray()
                   ?? Array.Empty<BetCancelMarket>();
        }
    }
}