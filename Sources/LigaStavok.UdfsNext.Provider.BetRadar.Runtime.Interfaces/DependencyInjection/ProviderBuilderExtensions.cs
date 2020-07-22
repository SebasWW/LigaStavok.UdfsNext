﻿using System;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi;
using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.UdfsNext.Provider.BetRadar.DependencyInjection
{
	public static class ProviderBuilderExtensions
	{
		public static IProviderBuilder ConfigureProviderManager(this IProviderBuilder providerBuilder, Action<ProviderManagerOptions> action)
		{
			providerBuilder.ServiceCollection.Configure(action);
			return providerBuilder;
		}

		public static IProviderBuilder ConfigureFeedListener(this IProviderBuilder providerBuilder, Action<FeedListenerOptions> action)
		{
			providerBuilder.ServiceCollection.Configure(action);
			return providerBuilder;
		}

		public static IProviderBuilder ConfigureHttpClientManager(this IProviderBuilder providerBuilder, Action<HttpClientManagerOptions> action)
		{
			providerBuilder.ServiceCollection.Configure(action);
			return providerBuilder;
		}

		public static IProviderBuilder ConfigureFeedSubscriber(this IProviderBuilder providerBuilder, Action<FeedSubscriberOptions> action)
		{
			providerBuilder.ServiceCollection.Configure(action);
			return providerBuilder;
		}

		//public static IProviderBuilder ConfigureRabbitMQ(this IProviderBuilder providerBuilder, Action<WebSocketClientOptions> action)
		//{
		//	providerBuilder.ServiceCollection.Configure(action);
		//	return providerBuilder;
		//}
	}
}
