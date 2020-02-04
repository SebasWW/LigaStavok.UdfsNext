using System;
using LigaStavok.UdfsNext.Clustering;
using LigaStavok.UdfsNext.Clustering.Client;
using LigaStavok.UdfsNext.Configuration;
using LigaStavok.UdfsNext.Line;
using LigaStavok.UdfsNext.Line.Clustering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseUdfsLineClusterClient<TCluster>(this IHostBuilder hostBuilder, Func<ClusterClientConfiguration> configurationFunc)
		{
			hostBuilder.UseUdfsClusterClient<TCluster>(
				options =>
				{
					var configuration = configurationFunc.Invoke();

					// Clustering information
					options.Configure(configuration);

					// Grains interfaces
					options.GrainAssemblies.Add(typeof(IUdfsLineEventGrain).Assembly);

				}
			);

			return hostBuilder;
		}

		public static IHostBuilder UseUdfsLineClusterClient<TCluster>(
			this IHostBuilder hostBuilder,
			Action<UdfsClusterClientOptions<TCluster>> configureDelegate
		)
		{
			hostBuilder.ConfigureServices((context, service) => service.AddUdfsClusterClient(configureDelegate));
			return hostBuilder;
		}
	}
}
