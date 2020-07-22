using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Common.Messages;

namespace Udfs.BetradarUnifiedFeed.Plugin.Abstractions
{
    public interface IFeedMessageParser
    {
        IFeedMessageParsingResult ParseFeedMessageText(IHaveHeader sourceMessage,string decodedText);
    }
}