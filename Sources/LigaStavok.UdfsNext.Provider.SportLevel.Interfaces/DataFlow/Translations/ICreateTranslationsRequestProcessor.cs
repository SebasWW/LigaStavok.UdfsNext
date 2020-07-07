using LigaStavok.Threading;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Translations
{
	public interface ICreateTranslationsRequestProcessor : IAsyncProcessor<MessageContext<TranslationsRequest>>
	{
	}
}
