using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace LigaStavok.UdfsNext.Orleans.Client
{
    public class UdfsClusterClientBuilder : IUdfsClusterClientBuilder
    {
        public IUdfsClusterClient Build(
            UdfsClusterClientOptions clusterClientOptions
        )
		{
            var clientBuilder = new ClientBuilder();

            // Local or distribute cluster
            if (clusterClientOptions.Membership.Enabled)
                clientBuilder.UseAdoNetOrleans(
                    options =>
                    {
                        options.Invariant = clusterClientOptions.Membership.Provider;
                        options.ConnectionString = clusterClientOptions.Membership.ConnectionString;
                    }
                );
            else
                clientBuilder.UseLocalhostOrleans();

            //.ConfigureLogging(logging => logging.AddConsole())
            //.UsePerfCounterEnvironmentStatistics()
            //.Configure<ClientMessagingOptions>(options => options.ResponseTimeout = TimeSpan.FromSeconds(30))
            clientBuilder.Configure<ClusterOptions>(
                options =>
                {
                    options.ClusterId = clusterClientOptions.ClusterService.ClusterId;
                    options.ServiceId = clusterClientOptions.ClusterService.ServiceId;
                }
            );

            // Interfaces
            foreach (var item in clusterClientOptions.GrainAssemblies)
                clientBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(item).WithReferences());

            // Build client
            return new UdfsClusterClient(clusterClientOptions, clientBuilder.Build());
        }
	}
}
