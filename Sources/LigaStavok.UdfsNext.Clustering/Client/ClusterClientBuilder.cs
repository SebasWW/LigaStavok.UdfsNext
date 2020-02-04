using System;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace LigaStavok.UdfsNext.Clustering.Client
{
    internal class ClusterClientBuilder<TCluster> : IClusterClientBuilder<TCluster>
    {
		private readonly UdfsClusterClientOptions<TCluster> options;

        public ClusterClientBuilder(UdfsClusterClientOptions<TCluster>  options)
        {
            this.options = options;
        }

		public IClusterClient Build()
		{
            var clientBuilder = new ClientBuilder();

            // Local or distribute cluster
            if (options.Membership.Enabled)
                clientBuilder.UseAdoNetClustering(
                    options =>
                    {
                        options.Invariant = this.options.Membership.Provider;
                        options.ConnectionString = this.options.Membership.ConnectionString;
                    }
                );
            else
                clientBuilder.UseLocalhostClustering();

            //.ConfigureLogging(logging => logging.AddConsole())
            //.UsePerfCounterEnvironmentStatistics()
            //.Configure<ClientMessagingOptions>(options => options.ResponseTimeout = TimeSpan.FromSeconds(30))
            clientBuilder.Configure<ClusterOptions>(
                options =>
                {
                    options.ClusterId = this.options.ClusterService.ClusterId;
                    options.ServiceId = this.options.ClusterService.ServiceId;
                }
            );

            // Interfaces
            foreach (var item in options.GrainAssemblies)
                clientBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(item).WithReferences());

            // Build client
            return clientBuilder.Build();
        }
	}
}
