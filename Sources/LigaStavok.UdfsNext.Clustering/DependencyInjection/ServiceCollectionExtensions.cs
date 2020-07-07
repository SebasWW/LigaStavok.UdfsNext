using System;
using System.Collections.Generic;
using System.Linq;
using LigaStavok.UdfsNext.Orleans;
using LigaStavok.UdfsNext.Orleans.Client;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUdfsClusterClientsLocator(
            this IServiceCollection services,
            Action<UdfsClusterClientsLocatorOptions> configureDelegate
        )
        {
            // Options
            services.Configure(configureDelegate);


            // Objects
            services.AddSingleton<UdfsClusterClientsLocator>();
            services.AddSingleton<IUdfsClusterClientBuilder, UdfsClusterClientBuilder>();


            // Service
            services.AddHostedService<UdfsClusterClientService>();

            return services;
        }
    }
}
