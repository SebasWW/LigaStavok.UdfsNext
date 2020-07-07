using System.Net.Http;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Responses;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi
{
	public interface IHttpResponseMessageParser
	{
		Task<IWebApiResponse> ParseAsync(HttpResponseMessage httpResponseMessage);
	}
}