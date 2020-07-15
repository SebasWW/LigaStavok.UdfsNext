using LigaStavok.Threading;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Subscription
{
	public interface ICreateTranslationRequestProcessor : IAsyncProcessor<MessageContext<TranslationRequest>>
	{
	}
}