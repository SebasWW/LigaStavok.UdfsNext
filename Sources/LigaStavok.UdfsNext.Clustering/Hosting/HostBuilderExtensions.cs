using System;
using LigaStavok.UdfsNext.Clustering;
using LigaStavok.UdfsNext.Clustering.Client;
using LigaStavok.UdfsNext.Clustering.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;

namespace Microsoft.Extensions.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseUdfsCluster(this IHostBuilder hostBuilder, Action<UdfsClusterOptions> configureDelegate)
		{
			// Orleans configuration
			return UseUdfsCluster(hostBuilder, configureDelegate, null);
		}

		public static IHostBuilder UseUdfsCluster(this IHostBuilder hostBuilder, Action<UdfsClusterOptions> configureDelegate, Action<ISiloBuilder> builderDelegate)
		{
			hostBuilder.UseOrleans(
				(context, builder) =>
				{

					// Requesting options
					var udfsOptions = new UdfsClusterOptions();
					configureDelegate?.Invoke(udfsOptions);

					// Configure builder
					builder.ConfigureUdfs(udfsOptions);
					builderDelegate?.Invoke(builder);
				}
			);

			return hostBuilder;
		}

		public static IHostBuilder UseUdfsClusterClient<TCluster>(
			this IHostBuilder hostBuilder,
			Action<UdfsClusterClientOptions<TCluster>> configureDelegate
		)
		{
			hostBuilder.ConfigureServices((context, service) => service.AddUdfsClusterClient(configureDelegate));
			return hostBuilder;
		}
	}
}
