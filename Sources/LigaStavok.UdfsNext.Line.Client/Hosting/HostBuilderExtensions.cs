using System;
using LigaStavok.UdfsNext.Orleans.Client;
using LigaStavok.UdfsNext.Configuration;
using LigaStavok.UdfsNext.Line;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseUdfsLineClusterClient(
			this IHostBuilder hostBuilder, 
			Func<ClusterClientConfiguration> configurationFunc)
		{
			hostBuilder.UseUdfsLineClusterClient(
				options =>
				{
					var configuration = configurationFunc.Invoke();

					// Orleans information
					options.ConfigureWith(configuration);

				}
			);

			return hostBuilder;
		}

		public static IHostBuilder UseUdfsLineClusterClient(
			this IHostBuilder hostBuilder,
			Action<UdfsClusterClientOptions> configureDelegate
		)
		{
			hostBuilder.ConfigureServices(
				(context, service) => 
					service.AddUdfsLineClusterClient(configureDelegate)
			);

			return hostBuilder;
		}
	}
}
