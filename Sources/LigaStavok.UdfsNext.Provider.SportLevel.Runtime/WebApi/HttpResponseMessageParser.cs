using System;
using System.Net.Http;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Responses;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi
{
	public class HttpResponseMessageParser : IHttpResponseMessageParser
	{
		public async Task<IWebApiResponse> ParseAsync(HttpResponseMessage httpResponseMessage)
		{
			IWebApiResponse response;

			switch (httpResponseMessage.RequestMessage.Properties[WebApiRequest.SourceKey])
			{
				case SportsRequest.SourceValue:
					response = JsonConvert.DeserializeObject<SportsResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
					break;

				case TranslationsRequest.SourceValue:
					response = JsonConvert.DeserializeObject<TranslationsResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
					break;

				case TournamentsRequest.SourceValue:
					response = JsonConvert.DeserializeObject<TournamentsResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
					break;

				default:
					throw new NotSupportedException("Message can not be parsed.");
			}

			return response;
		}
	}
}
