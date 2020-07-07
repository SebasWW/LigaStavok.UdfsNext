using LigaStavok.UdfsNext.Provider.SportLevel;
using LigaStavok.WebSocket;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportLevelProvider(
            this IServiceCollection services, 
            Action<SportLevelProviderOptions> configureDelegate
        ) 
        {
            services.Configure<SportLevelProviderOptions>(
                options =>
                {
                    configureDelegate?.Invoke(options);

                    services.Configure<ProviderManagerOptions>(
                        providerManagerOptions =>
                        {

                        }
                    );

                    services.AddWebSocketClient(
                        options =>
                        {

                        }
                    );
                }
            );

            // Runtime
            services.AddSingleton<IProviderManager, ProviderManager>();

            return services;
        }
    }
}
