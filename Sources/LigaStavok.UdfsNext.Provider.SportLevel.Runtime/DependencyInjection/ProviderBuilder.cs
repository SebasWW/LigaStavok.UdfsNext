﻿using System;
using LigaStavok.UdfsNext.Provider.SportLevel.DataFlow;
using LigaStavok.UdfsNext.Provider.SportLevel.State;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;
using LigaStavok.UdfsNext.Providers;
using LigaStavok.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DependencyInjection
{
	public class ProviderBuilder : IProviderBuilder
	{
		public IServiceCollection ServiceCollection { get; }

		public ProviderBuilder(IServiceCollection services)
		{
			ServiceCollection = services;


			// WebSocket
			services.AddSingleton<IWebSocketMessageParser, WebSocketMessageParser>();

			// WebApi
			services.AddSingleton<IHttpResponseMessageParser, HttpResponseMessageParser>();
			services.AddSingleton<IHttpRequestMessageFactory, HttpRequestMessageFactory>();

			//services.AddSingleton<HttpClientManager>();
			services.AddHttpClient<HttpClientManager>(
				(provider, httpClient) =>
				{
					// For sample purposes, assume TodoClient is used in the context of an incoming request.
					//var httpRequest = provider.GetRequiredService<IHttpContextAccessor>().HttpContext.Request;

					//httpClient.BaseAddress = new Uri(UriHelper.BuildAbsolute(httpRequest.Scheme, httpRequest.Host, httpRequest.PathBase));
					//httpClient.Timeout = TimeSpan.FromSeconds(5);
				}
				).AddPolicyHandler(
					HttpPolicyExtensions
						.HandleTransientHttpError()
						.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i * 2))
				);

			// Primitives
			services.AddSingleton<TranslationSubscriptionCollection>();

			// Flow
			services.AddSingleton<FeedListenerFlow>();
			services.AddSingleton<FeedSubscriberFlow>();
			services.AddSingleton<ProviderManagerFlow>();

			// Runtime
			services.AddSingleton<IProviderManager, ProviderManager>();
			services.AddSingleton<IFeedManager, FeedManager>();
			services.AddSingleton<IFeedListener, FeedListener>();
			services.AddSingleton<IFeedSubscriber<TranslationState>, FeedSubscriber>();

			// Services
			services.AddHostedService<FeedListenerService>();

		}
	}
}
