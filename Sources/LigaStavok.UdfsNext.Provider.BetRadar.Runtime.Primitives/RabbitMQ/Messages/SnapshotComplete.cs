using System.ComponentModel;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class SnapshotComplete : IStatelessMessage
    {
        /// <summary>
        ///    Gets the product that sent this message.
        /// </summary>
        public ProductType Product { get; set; }

        public string GetEventId()
        {
            return null;
        }

        /// <summary>
        ///    Gets the identifier of related request.
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        ///    Gets timestamp in milliseconds since epoch when this message was generated according to generating system's clock.
        /// </summary>
        public long Timestamp { get; set; }

        public static SnapshotComplete Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new SnapshotComplete
            {
                Product    = dynamicXml.Product<ProductType>(),
                RequestId  = dynamicXml.RequestId<int>(),
                Timestamp  = dynamicXml.Timestamp<long>()
            };

            return builder;
        }
    }
}