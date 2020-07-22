using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class BetStop : IStatelessMessage
    {
        /// <summary>
        /// Gets the id of the event this bet stop is for.
        /// </summary>
        public string EventId { get; set; }
        
        /// <summary>
        /// Gets the product that sent this message.
        /// </summary>
        public ProductType Product { get; set; }

        public string GetEventId()
        {
            return EventId;
        }

        /// <summary>
        /// Gets the Identifier of related request.
        /// </summary>
        public int? RequestId { get; set; }

        /// <summary>
        /// Gets the timestamp in milliseconds since epoch when this message was generated according
        /// to generating system's clock.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Gets the description of which set of markets should be suspended – the value should be
        /// a group-name as can be seen in the market-descriptions ('all' is a special keyword
        /// that means all markets for this event).
        /// </summary>
        public string Groups { get; set; }

        public static BetStop Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new BetStop
            {
                EventId    = dynamicXml.EventId,
                Product    = dynamicXml.Product<ProductType>(),
                RequestId  = dynamicXml.RequestId<int?>(),
                Timestamp  = dynamicXml.Timestamp<long>(),
                Groups     = dynamicXml.Groups<string>()
            };

            return builder;
        }
    }
}