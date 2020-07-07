using System.Collections.Generic;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Api
{
	public interface IApiClient
	{
		Task<IEnumerable<Translation>> GetTranslationsAsync();
	}
}