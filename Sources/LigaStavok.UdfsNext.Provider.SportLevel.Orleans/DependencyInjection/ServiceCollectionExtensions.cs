using LigaStavok.UdfsNext.Provider.SportLevel;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans.Configuration;
using LigaStavok.WebSocket;
using System;
using System.Security;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportLevelProviderOrleans(
            this IServiceCollection services, 
            ServiceConfiguration configuration
        ) 
        {

			services.Configure<ProviderManagerGrainOptions>(
				options =>
				{
				}
			)

            // Adapter
            .AddSportLevelTransmitterAdapter()

            // Runtime
            .AddSportLevel(
                builder =>
				{
                    builder.ConfigureProviderManager(
                        options =>
                        {
                            options.MetaRefreshInterval = configuration.Provider.MetaRefreshInterval;
                        }
                    );

                    builder.ConfigureFeedListener(
                        options =>
                        {
                            options.UserName = configuration.Provider.UserName;
                            options.Password = configuration.Provider.Password;
                        }
                    );

                    builder.ConfigureHttpClientManager(
                        options =>
                        {
                            options.UserName = configuration.Provider.UserName;
                            options.Password = configuration.Provider.Password;
                            options.Uri = new Uri(configuration.Provider.WebApiUrl);
                        }
                    );

                    builder.AddWebSocketClient(
                        options =>
                        {
                            options.UseDefaultCredentials = true;
                            options.Uri = new Uri(configuration.Provider.WebSocketUrl);
                        }
                    );
                }
            );

            return services;
        }
    }
}
