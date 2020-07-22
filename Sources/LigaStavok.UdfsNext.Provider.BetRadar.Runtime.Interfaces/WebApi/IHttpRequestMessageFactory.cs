using System.Net.Http;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
	public interface IHttpRequestMessageFactory
	{
		HttpRequestMessage Create(WebApiRequest requestCommand);
	}
}