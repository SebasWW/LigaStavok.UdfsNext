using System;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DependencyInjection
{
	public interface IProviderBuilder
	{
		IServiceCollection ServiceCollection { get; }

		IProviderBuilder AddWebSocketClient(Action<WebSocketClientOptions> action);
		IProviderBuilder ConfigureFeedListener(Action<FeedListenerOptions> action);
		IProviderBuilder ConfigureFeedSubscriber(Action<FeedSubscriberOptions> action);
		IProviderBuilder ConfigureHttpClientManager(Action<HttpClientManagerOptions> action);
		IProviderBuilder ConfigureProviderManager(Action<ProviderManagerOptions> action);
	}
}