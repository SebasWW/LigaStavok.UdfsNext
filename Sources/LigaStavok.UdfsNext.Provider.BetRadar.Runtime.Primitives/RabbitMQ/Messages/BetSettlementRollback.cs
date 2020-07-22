using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class BetSettlementRollback : IStatefulMessage
    {
        /// <summary>
        ///     The id of the event this bet settlement is for.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        ///     Markets affected by this bet settlement rollback message.
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

        public static BetSettlementRollback Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new BetSettlementRollback
            {
                EventId = dynamicXml.EventId,
                Markets = BetSettlementMarket.ParseList(dynamicXml.GetMarketList()),
                RequestId = dynamicXml.RequestId<int?>(),
                Product = dynamicXml.Product<ProductType>(),
                Timestamp = dynamicXml.Timestamp<long>()
            };

            return builder;
        }

        public BetSettlementRollback CloneWithProduct(ProductType productType)
        {
            var builder = new BetSettlementRollback
            {
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