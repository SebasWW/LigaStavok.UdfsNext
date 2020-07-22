using System;
using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class FeedMessageReady
    {
        public IFeedMessage Message { get; set; }
    }
}