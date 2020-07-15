using System.Net.Http;
using LigaStavok.Threading;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Subscription
{
	public interface IExecuteTranslationRequestProcessor : IAsyncProcessor<MessageContext<HttpRequestMessage>>
	{
	}
}