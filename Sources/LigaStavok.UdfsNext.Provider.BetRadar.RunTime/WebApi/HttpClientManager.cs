using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
	public class HttpClientManager
	{
		private readonly HttpClientManagerOptions options;
		private readonly HttpClient httpClient;

		public HttpClientManager(
			IOptions<HttpClientManagerOptions> options,
			HttpClient httpClient
		)
		{
			this.options = options.Value;
			this.httpClient = httpClient;

			// Authentification
			var auth = new AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(Encoding.ASCII.GetBytes($"{options.Value.UserName}:{options.Value.Password}"))
			 );

			httpClient.DefaultRequestHeaders.Authorization = auth;
		}


		public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
		{
			// Requests are disabled
			//if (_config.DisableRemoteRequests) return;

			// Calls api
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
			//if (!httpResponseMessage.IsSuccessStatusCode) throw new BetRadarException($"HttpResponse status code {httpResponseMessage.StatusCode}.");

			return httpResponseMessage.EnsureSuccessStatusCode();
		}
	}
}
