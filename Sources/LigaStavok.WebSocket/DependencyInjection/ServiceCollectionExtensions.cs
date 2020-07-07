using LigaStavok.WebSocket;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebSocketClient(
            this IServiceCollection services, 
            Action<WebSocketClientOptions> configureDelegate
        ) 
        {
            services.Configure<WebSocketClientOptions>(
                options => 
                    configureDelegate?.Invoke(options)
            );

            services.AddSingleton<IWebsocketClient, WebSocketClient>();
            services.AddHostedService<WebSocketClientService>();

            return services;
        }
    }
}
