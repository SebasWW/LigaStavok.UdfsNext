using System;
using LigaStavok.UdfsNext.Clustering;
using LigaStavok.UdfsNext.Clustering.Client;
using LigaStavok.UdfsNext.Line;
using LigaStavok.UdfsNext.Line.Clustering;
using LigaStavok.UdfsNext.Line.Providers.BetRadar.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseUdfsLineClusterClient(this IHostBuilder hostBuilder)
		{
			ServiceConfiguration configuration = null;
			hostBuilder.ConfigureServices((hostContext, servicesCollection) =>
				 {
					 configuration = hostContext.Configuration.Get<ServiceConfiguration>();
				 }
			);

			hostBuilder.UseUdfsLineClusterClient<LineMskCluster>( () => configuration.LineCluster);

			return hostBuilder;
		}

	}
}
