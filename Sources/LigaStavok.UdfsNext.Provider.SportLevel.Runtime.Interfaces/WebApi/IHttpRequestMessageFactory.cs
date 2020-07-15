using System.Net.Http;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi
{
	public interface IHttpRequestMessageFactory
	{
		HttpRequestMessage Create(WebApiRequest requestCommand);
	}
}