using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class FixtureChange : IStatelessMessage
    {
        /// <summary>
        ///     Gets a <see cref="FixtureChangeType" /> indicating how the fixture was changed
        /// </summary>
        /// <remarks>
        ///     If specified, declares what type of change it is (new, start time, coverage).
        ///     For a start time change or coverage change the details are in the message.
        ///     Otherwise, the new fixture has to be requested from the API.
        /// </remarks>
        public FixtureChangeType? ChangeType { get; set; }

        /// <summary>
        ///     The id of the event this fixture change is for.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        ///    Gets a value specifying the start time of the fixture in milliseconds since EPOCH UTC after the fixture was re-scheduled
        /// </summary>
        public long? NextLiveTime { get; set; }

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
        public long? RequestId { get; set; }

        /// <summary>
        ///     Gets a value specifying the start time of the fixture in milliseconds since EPOCH UTC
        /// </summary>
        public long StartTime { get; set; }
        
        /// <summary>
        ///     Timestamp in milliseconds since epoch when this message was generated according
        ///     to generating system's clock.
        /// </summary>
        public long Timestamp { get; set; }

        public static FixtureChange Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new FixtureChange
            {
                ChangeType   = dynamicXml.ChangeType<FixtureChangeType?>(),
                EventId      = dynamicXml.EventId,
                NextLiveTime = dynamicXml.NextLiveTime<long?>(),
                Product      = dynamicXml.Product<ProductType>(),
                RequestId    = dynamicXml.RequestId<int?>(),
                StartTime    = dynamicXml.StartTime<long>(),
                Timestamp    = dynamicXml.Timestamp<long>()
            };

            return builder;
        }
    }
}