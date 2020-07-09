using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class ProviderBuilder
	{
		private readonly IServiceCollection services;

		public ProviderBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		public void ConfigureProviderManager(Action<ProviderManagerOptions> action)
		{
			services.Configure<ProviderManagerOptions>(action);
		}

		public void ConfigureTranslationManager(Action<TranslationManagerOptions> action)
		{
			services.Configure<TranslationManagerOptions>(action);
		}
		public void ConfigureHttpClientManager(Action<HttpClientManagerOptions> action)
		{
			services.Configure<HttpClientManagerOptions>(action);
		}

		public void AddWebSocketClient(Action<WebSocketClientOptions> action)
		{
			services.AddWebSocketClient(action);
		}
	}
}
