using System;
using LigaStavok.UdfsNext;
using LigaStavok.UdfsNext.Orleans;
using LigaStavok.UdfsNext.Orleans.Client;
using LigaStavok.UdfsNext.Line;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUdfsLineClusterClient(
            this IServiceCollection services, 
            Action<UdfsClusterClientOptions> configureDelegate
        ) 
        {
            services.AddUdfsClusterClientsLocator(
                options =>
                {
                    services.Configure<UdfsClusterClientOptions>(
                        clusterClientOptions =>
                        {
                            options.Clusters.Add(Services.Line, clusterClientOptions);

                            // Confire
                            configureDelegate?.Invoke(clusterClientOptions);

                            // Grains interfaces
                            options.Clusters[Services.Line].GrainAssemblies.Add(typeof(IUdfsLineEventGrain).Assembly);
                        }
                    );
                }
            );

            return services;
        }
    }
}
