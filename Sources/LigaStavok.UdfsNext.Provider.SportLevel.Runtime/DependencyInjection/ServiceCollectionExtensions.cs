using LigaStavok.UdfsNext.Provider.SportLevel;
using LigaStavok.UdfsNext.Provider.SportLevel.DataFlow;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;
using LigaStavok.WebSocket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Polly;
using Polly.Extensions.Http;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportLevel(
            this IServiceCollection services, 
            Action<ProviderBuilder> configureDelegate
        ) 
        {
            ProviderBuilder builder = new ProviderBuilder(services);
            configureDelegate.Invoke(builder);

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
                        .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i*2))
                );

            // Flow
            services.AddSingleton<FeedListenerFlow>();
            services.AddSingleton<FeedSubscriberFlow>();
            services.AddSingleton<ProviderManagerFlow>();


            // Runtime
            services.AddSingleton<IProviderManager, ProviderManager>();
            services.AddSingleton<IFeedManager, FeedManager>();
            services.AddSingleton<IFeedListener, FeedListener>();
            services.AddSingleton<IFeedSubscriber, FeedSubscriber>();

            // Services
            services.AddHostedService<FeedListenerService>();

            return services;
        }
    }
}
