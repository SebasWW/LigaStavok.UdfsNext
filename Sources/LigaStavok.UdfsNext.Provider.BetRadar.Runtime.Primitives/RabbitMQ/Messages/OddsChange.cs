using System;
using System.Collections.Generic;
using System.ComponentModel;

using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class OddsChange : IStatelessMessage
    {
 
        /// <summary>
        ///    Sent after a betstop while under betstop to signal the cause of the betstop.
        /// </summary>
        public OddsChangeBetStopReason? BetStopReason { get; set; }

        /// <summary>
        ///    When set signals that markets have been opened again after a betstop,
        ///    but it is still early after betstop.
        /// </summary>
        public OddsChangeBettingStatus? BettingStatus { get; set; }

        /// <summary>
        ///     The id of the event this odds change is for.
        /// </summary>
        public string EventId { get; set; }


        public OddsChangeEventStatus SportEventStatus { get; set; }

        /// <summary>
        ///    Can be set to "riskadjustment_update" if this message is
        ///    caused by a manual odds change.
        /// </summary>
        public OddsChangeReason? OddsChangeReason { get; set; }

        /// <summary>
        ///     Markets affected by this odds change message.
        /// </summary>
        public IEnumerable<OddsChangeMarket> Markets { get; set; }

        /// <summary>
        ///     The product that sent this message.
        /// </summary>
        public ProductType Product { get; set; }

        public string GetEventId()
        {
            return EventId;
        }

        public string GetPureEventId()
        {

            return EventId.StartsWith("sr:match:") ? EventId.Substring(9) : EventId;
        }

        /// <summary>
        ///     Identifier of related request.
        /// </summary>
        public long? RequestId { get; set; }

        /// <summary>
        ///     Timestamp in milliseconds since epoch when this message was generated according
        ///     to generating system's clock.
        /// </summary>
        public long Timestamp { get; set; }

        public static OddsChange Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new OddsChange
            {
                BetStopReason		= dynamicXml.Odds?.BetStopReason<OddsChangeBetStopReason?>(),
                BettingStatus		= dynamicXml.Odds?.BettingStatus<OddsChangeBettingStatus?>(),
                EventId				= dynamicXml.EventId,
                SportEventStatus	= OddsChangeEventStatus.Parse(dynamicXml.SportEventStatus),
                Markets				= OddsChangeMarket.ParseList(dynamicXml.Odds?.GetMarketList()),
                OddsChangeReason	= dynamicXml.OddsChangeReason<OddsChangeReason>(),
                Product				= dynamicXml.Product<ProductType>(),
                RequestId			= dynamicXml.RequestId<int?>(),
                Timestamp			= dynamicXml.Timestamp<long>()
            };

            return builder;
        }
    }
}