using System.Collections.Generic;
using System.ComponentModel;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class BetSettlement : IStatefulMessage
    {
        /// <summary>
        ///    Gets the level of certainty.
        /// </summary>
        /// <remarks>
        ///    The client system will often receive two BetSettements for the same outcome – one immediately
        ///    after the match ends caused by the live scout, and a second confirming one when the results
        ///    have been official confirmed. The two messages have different certainty-levels to indicate the
        ///    difference. In almost all cases the outcome results will be the same. In extra-ordinary cases the
        ///    results may differ and the Client system will have to decide how to handle this. 
        /// </remarks>
        public int Certainty { get; set; }

        /// <summary>
        ///     The id of the event this bet settlement is for.
        /// </summary>
        public string EventId { get; set; }
        
        /// <summary>
        ///     Markets affected by this bet settlement message.
        /// </summary>
        public IEnumerable<BetSettlementMarket> Markets { get; set; }

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
        ///     Timestamp in milliseconds since epoch when this message was generated according
        ///     to generating system's clock.
        /// </summary>
        public long Timestamp { get; set; }



        public static BetSettlement Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new BetSettlement
            {
                Certainty  = dynamicXml.Certainty<int>(),
                EventId    = dynamicXml.EventId,
                Markets    = BetSettlementMarket.ParseList(dynamicXml.Outcomes?.GetMarketList()),
                RequestId  = dynamicXml.RequestId<int?>(),
                Product    = dynamicXml.Product<ProductType>(),
                Timestamp  = dynamicXml.Timestamp<long>()
            };

            return builder;
        }

        public BetSettlement CloneWithProduct(ProductType productType)
        {
            var builder = new BetSettlement
            {
                Certainty = Certainty,
                EventId = EventId,
                Markets = Markets,
                RequestId = RequestId,
                Product = productType,
                Timestamp = Timestamp
            };

            return builder;
        }
    }
}