using LigaStavok.UdfsNext.Provider.SportLevel;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter;
using System;
using System.Security;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportLevelTransmitterAdapter(
            this IServiceCollection services
        ) 
        {


            // Runtime
            //services.AddSingleton<SportLevelPluginInjector>();
            services.AddSingleton<IProviderAdapter, TransmitterAdapter>();

            // TransmitterHost
            services.AddHostedService<TransmitterHostedService>();

            return services;
        }
    }
}
