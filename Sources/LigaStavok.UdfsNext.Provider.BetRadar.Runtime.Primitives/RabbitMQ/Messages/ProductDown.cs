using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
	public sealed class ProductDown : IStatelessMessage
    {
        /// <summary>
        ///     The product that sent this message
        /// </summary>
        public ProductType Product { get; set; }

        public string GetEventId()
        {
            return null;
        }

        /// <summary>
        ///     Timestamp in milliseconds since epoch when this message was generated according to generating system's clock.
        /// </summary>
        public long Timestamp { get; set; }

        public static ProductDown Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new ProductDown
            {
                Product    = dynamicXml.Product<ProductType>(),
                Timestamp  = dynamicXml.Timestamp<long>()
            };

            return builder;
        }
    }
}