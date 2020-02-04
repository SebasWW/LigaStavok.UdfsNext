using System;
using LigaStavok.UdfsNext.Clustering.Client;
using LigaStavok.UdfsNext.Configuration;
using LigaStavok.UdfsNext.Line.Clustering;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;

namespace LigaStavok.UdfsNext.Line
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddUdfsLineClusterClient<TCluster>(
			this IServiceCollection services,
			ClusterClientConfiguration configuration
		)
		{
			services.AddUdfsClusterClient<TCluster>(
				options =>
				{
					options.Configure(configuration);
				}
			);

			return services;
		}
	}
}
