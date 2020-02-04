using System;
using LigaStavok.UdfsNext.Configuration;
using LigaStavok.UdfsNext.Clustering;
using Microsoft.Extensions.Configuration;
using Orleans.Hosting;
using LigaStavok.UdfsNext.Line.Configuration;

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

			hostBuilder.UseUdfsLineCluster(
				() =>
				{
					return configuration.Cluster;
				}
			);

			return hostBuilder;
		}

		public static IHostBuilder UseUdfsLineCluster(this IHostBuilder hostBuilder, Func<ClusterConfiguration> clusterConfigurationFunc)
		{
			hostBuilder
				.UseUdfsCluster(
					options =>
					{
						options.Configure(clusterConfigurationFunc.Invoke());
					}
				);

			return hostBuilder;
		}
	}
}
