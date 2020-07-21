using System;
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

		public void ConfigureFeedListener(Action<FeedListenerOptions> action)
		{
			services.Configure<FeedListenerOptions>(action);
		}
		public void ConfigureHttpClientManager(Action<HttpClientManagerOptions> action)
		{
			services.Configure<HttpClientManagerOptions>(action);
		}

		public void ConfigureFeedSubscriber(Action<FeedSubscriberOptions> action)
		{
			services.Configure<FeedSubscriberOptions>(action);
		}

		public void AddWebSocketClient(Action<WebSocketClientOptions> action)
		{
			services.AddWebSocketClient(action);
		}
	}
}
