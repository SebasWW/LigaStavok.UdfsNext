using System;
using LigaStavok.UdfsNext.Provider.SportLevel.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportLevelRunTime(
            this IServiceCollection services, 
            Action<IProviderBuilder> configureDelegate
        ) 
        {
            IProviderBuilder builder = new ProviderBuilder(services);
            configureDelegate.Invoke(builder);

            //// WebSocket
            //services.AddSingleton<IWebSocketMessageParser, WebSocketMessageParser>();

            //// WebApi
            //services.AddSingleton<IHttpResponseMessageParser, HttpResponseMessageParser>();
            //services.AddSingleton<IHttpRequestMessageFactory, HttpRequestMessageFactory>();

            ////services.AddSingleton<HttpClientManager>();
            //services.AddHttpClient<HttpClientManager>(
            //    (provider, httpClient) =>
            //        {
            //            // For sample purposes, assume TodoClient is used in the context of an incoming request.
            //            //var httpRequest = provider.GetRequiredService<IHttpContextAccessor>().HttpContext.Request;

            //            //httpClient.BaseAddress = new Uri(UriHelper.BuildAbsolute(httpRequest.Scheme, httpRequest.Host, httpRequest.PathBase));
            //            //httpClient.Timeout = TimeSpan.FromSeconds(5);
            //        }
            //    ).AddPolicyHandler(
            //        HttpPolicyExtensions
            //            .HandleTransientHttpError()
            //            .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i*2))
            //    );

            //// Primitives
            //services.AddSingleton<TranslationSubscriptionCollection>();

            //// Flow
            //services.AddSingleton<FeedListenerFlow>();
            //services.AddSingleton<FeedSubscriberFlow>();
            //services.AddSingleton<ProviderManagerFlow>();

            //// Runtime
            //services.AddSingleton<IProviderManager, ProviderManager>();
            //services.AddSingleton<IFeedManager, FeedManager>();
            //services.AddSingleton<IFeedListener, FeedListener>();
            //services.AddSingleton<IFeedSubscriber, FeedSubscriber>();

            //// Services
            //services.AddHostedService<FeedListenerService>();

            return services;
        }
    }
}
