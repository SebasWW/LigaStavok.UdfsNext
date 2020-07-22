using System.Collections.Generic;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
	public sealed class RawFeedMessage 
    {
        public IEnumerable<byte> Data { get; set; }
    }
}