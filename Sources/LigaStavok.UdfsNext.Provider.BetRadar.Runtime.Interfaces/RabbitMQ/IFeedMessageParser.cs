using LigaStavok.UdfsNext;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ
{
	public interface IFeedMessageParser
    {
        IFeedMessageParsingResult ParseFeedMessageText(MessageContext messageContext,string decodedText);
    }
}