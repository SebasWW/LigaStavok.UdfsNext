using System.Collections.Generic;
using System.ComponentModel;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    [ImmutableObject(true)]
    public sealed class BetCancel : IStatefulMessage
    {
     
        /// <summary>
        ///     If start and end time are specified they designate a range in time for which bets made
        ///     should be cancelled.
        /// </summary>
        public long? EndTime { get; set; }

        /// <summary>
        ///     The id of the event this bet cancel message is for.
        /// </summary>
        public string EventId { get; set; }
        
        /// <summary>
        ///     Markets affected by this bet cancel message.
        /// </summary>
        public IEnumerable<BetCancelMarket> Markets { get; set; }

        /// <summary>
        ///     The product that sent this message.
        /// </summary>
        public ProductType Product { get; set; }

        public string GetEventId()
        {
            return EventId;
        }

        /// <summary>
        ///     Identifier of related request.
        /// </summary>
        public int? RequestId { get; set; }

        /// <summary>
        ///     If start and end time are specified they designate a range in time for which bets made
        ///     should be cancelled.
        /// </summary>
        public long? StartTime { get; set; }

        /// <summary>
        ///    If the market was cancelled because of a migration from a different sport event, 
        ///    it gets a id specifying the sport event from which the market has migrated.
        /// </summary>
        public string SupercededBy { get; set; }

        /// <summary>
        ///     Timestamp in milliseconds since epoch when this message was generated according
        ///     to generating system's clock.
        /// </summary>
        public long Timestamp { get; set; }

        public static BetCancel Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new BetCancel
            {
                EndTime      = dynamicXml.EndTime<long?>(),
                EventId      = dynamicXml.EventId,
                Markets      = BetCancelMarket.ParseList(dynamicXml.GetMarketList()),
                Product      = dynamicXml.Product<ProductType>(),
                RequestId    = dynamicXml.RequestId<int?>(),
                StartTime    = dynamicXml.StartTime<long?>(),
                SupercededBy = dynamicXml.SupercededBy,
                Timestamp    = dynamicXml.Timestamp<long>()
            };

            return builder;
        }
    }
}