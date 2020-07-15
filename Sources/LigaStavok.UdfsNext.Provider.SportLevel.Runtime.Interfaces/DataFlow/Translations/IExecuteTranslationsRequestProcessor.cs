using System.Net.Http;
using LigaStavok.Threading;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Translations
{
	public interface IExecuteTranslationsRequestProcessor : IAsyncProcessor<MessageContext<HttpRequestMessage>>
	{
	}
}
