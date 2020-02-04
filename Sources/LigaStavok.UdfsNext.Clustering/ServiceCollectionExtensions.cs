using System;
using LigaStavok.UdfsNext.Clustering;
using LigaStavok.UdfsNext.Clustering.Client;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
//        public static IServiceCollection AddUdfsCluster<(
//    this IServiceCollection services
//    //Action<UdfsClusterBuilderOptions> configureDelegate,
//    //Action<IUdfsClusterBuilder> builderDelegate
//)

//        {
//            services.UseOrleans()

//            var options = new UdfsClusterClientBuilderOptions<TCluster>();
//            configureDelegate?.Invoke(options);

//            var build = new UdfsClusterClientBuilder<TCluster>(options);
//            builderDelegate?.Invoke(build);

//            var client = build.Build();
//            services.AddSingleton(client);

//            return services;
//        }
//    }

        public static IServiceCollection AddUdfsClusterClient<TCluster>(
            this IServiceCollection services, 
            Action<UdfsClusterClientOptions<TCluster>> configureDelegate
        ) 
        {
            var options = new UdfsClusterClientOptions<TCluster>();
            configureDelegate?.Invoke(options);

            services.AddSingleton(options);

            services.AddSingleton<IUdfsClusterClient<TCluster>, UdfsClusterClient<TCluster>>();
            services.AddHostedService<UdfsClusterClient<TCluster>>();

            return services;
        }
    }
}
