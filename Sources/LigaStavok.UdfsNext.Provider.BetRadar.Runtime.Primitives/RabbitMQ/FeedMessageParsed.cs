namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
	public sealed class FeedMessageParsed :  IFeedMessageParsingResult
    {
    
        public IFeedMessage Message { get; set; }

    }
}