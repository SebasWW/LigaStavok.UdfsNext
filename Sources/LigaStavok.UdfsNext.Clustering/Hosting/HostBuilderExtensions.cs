using System;
using LigaStavok.UdfsNext.Orleans;
using LigaStavok.UdfsNext.Orleans.Client;
using LigaStavok.UdfsNext.Orleans.Hosting;
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
				(context, siloBuilder) =>
				{

					// Requesting options
					var udfsOptions = new UdfsClusterOptions();
					configureDelegate?.Invoke(udfsOptions);

					// Configure builder
					siloBuilder.ConfigureUdfs(udfsOptions);
					builderDelegate?.Invoke(siloBuilder);
				}
			);

			return hostBuilder;
		}

		public static IHostBuilder UseUdfsClusterClient<TCluster>(
			this IHostBuilder hostBuilder,
			Action<UdfsClusterClientOptions<TCluster>> configureDelegate
		)
		{
			hostBuilder.ConfigureServices(
				(context, services) => 
					services.AddUdfsClusterClient(configureDelegate)
			);
			return hostBuilder;
		}
	}
}
