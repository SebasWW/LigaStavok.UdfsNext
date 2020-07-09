using LigaStavok.Threading;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Streaming
{
	public  interface IMessageParserProcessor : IAsyncProcessor<MessageContext<string>>
	{
	}
}