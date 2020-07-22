using System.Net.Http;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
	public interface IHttpResponseMessageParser
	{
		Task<IWebApiResponse> ParseAsync(HttpResponseMessage httpResponseMessage);
	}
}