using System;
using LigaStavok.UdfsNext.Hosting;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans;
using LigaStavok.UdfsNext.Providers.Orleans;
using LigaStavok.UdfsNext.Providers.Orleans.StartupTasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;

namespace Microsoft.Extensions.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseBetRadarProviderOrleans(this IHostBuilder hostBuilder, Action<ISiloBuilder> configureHandler)
		{
			// Cluster
			hostBuilder.UseOrleans(
				siloBuilder =>
				{
					configureHandler.Invoke(siloBuilder);

					// Dependencies
					siloBuilder.ConfigureServices(
						services =>
						{
							services.AddBetRadarProviderOrleans();
						}
					);

					// Grains 
					siloBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ProviderManagerGrain).Assembly).WithReferences());
					siloBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(FeedSubscriberGrain).Assembly).WithReferences());

					// Tasks
					siloBuilder.AddStartupTask<ProviderManagerStartupTask>();
				}
			);

			return hostBuilder;
		}
	}
}
