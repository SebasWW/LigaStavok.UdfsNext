using LigaStavok.Threading;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Subscription
{
	public interface ITranslationAdapterProcessor : IAsyncProcessor<MessageContext<Translation>>
	{
	}
}