using System;
using LigaStavok.UdfsNext.Hosting;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans.StartupTasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;

namespace Microsoft.Extensions.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseSportLevelProviderOrleans(this IHostBuilder hostBuilder, Action<ISiloBuilder> configureHandler)
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
							services.AddSportLevelProviderOrleans(
								options =>
								{

								}
							);
						}
					);

					// Grains 
					siloBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ProviderManagerGrain).Assembly).WithReferences());

					// Tasks
					siloBuilder.AddStartupTask<ProviderManagerStartupTask>();
				}
			);

			return hostBuilder;
		}
	}
}
