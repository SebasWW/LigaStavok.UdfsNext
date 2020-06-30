using System;
using LigaStavok.UdfsNext.Configuration;
using LigaStavok.UdfsNext.Orleans;
using Microsoft.Extensions.Configuration;
using Orleans.Hosting;
using LigaStavok.UdfsNext.Line.Configuration;
using LigaStavok.UdfsNext.Line;
using Orleans;

namespace Microsoft.Extensions.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseUdfsLineCluster(this IHostBuilder hostBuilder)
		{
			ServiceConfiguration configuration = null;
			hostBuilder.ConfigureServices((hostContext, servicesCollection) =>
				{
					configuration = hostContext.Configuration.Get<ServiceConfiguration>();
				}
			);

			// Cluster
			hostBuilder.UseUdfsLineCluster(
				() =>
				{
					return configuration.Cluster;
				},
				siloBuilder =>
				{
					// GrainServices
					siloBuilder.AddGrainService<LineGrainService>();

					// Grains 
					siloBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(LineGrainService).Assembly).WithReferences());
				}
			);

			return hostBuilder;
		}

		public static IHostBuilder UseUdfsLineCluster(this IHostBuilder hostBuilder, Func<ClusterConfiguration> clusterConfigurationFunc, Action<ISiloBuilder> builderDelegate)
		{
			hostBuilder
				.UseUdfsCluster(
					options =>
					{
						options.Configure(clusterConfigurationFunc.Invoke());
					},
					builderDelegate
				);

			return hostBuilder;
		}
	}
}
