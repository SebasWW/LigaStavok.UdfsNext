using System.ComponentModel;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class Alive : IStatelessMessage
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
        ///     If set to false this means the product is up again after downtime, and the receiving client
        ///     will have to issue recovery messages against the API to start receiving any additional
        ///     messages and get the current state.
        /// </summary>
        public bool Subscribed { get; set; }

        /// <summary>
        ///     Timestamp in milliseconds since epoch when this message was generated according
        ///     to generating system's clock.
        /// </summary>
        public long Timestamp { get; set; }


        public static Alive Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Alive
            {
                Product    = dynamicXml.Product<ProductType>(),
                Subscribed = dynamicXml.Subscribed<int>() == 1,
                Timestamp  = dynamicXml.Timestamp<long>()
            };

            return builder;
        }
    }
}