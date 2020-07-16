using LigaStavok.UdfsNext.Hosting;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans.Configuration;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans.StartupTasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;

namespace Microsoft.Extensions.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseSportLevelProviderOrleans(this IHostBuilder hostBuilder)
		{
			ServiceConfiguration configuration = null;
			hostBuilder.ConfigureServices((context, services) =>
				{
					configuration = context.Configuration.Get<ServiceConfiguration>();
				}
			);

			// Cluster
			hostBuilder.UseOrleans(
				siloBuilder =>
				{
					//ServiceConfiguration configuration = null;
					//siloBuilder.ConfigureServices((context, services) =>
					//	{
					//		configuration = context.Configuration.Get<ServiceConfiguration>();
					//	}
					//);

					siloBuilder.Configure(configuration.Cluster);

					// Logging
					//siloBuilder.ConfigureLogging();

					// Dependencies
					siloBuilder.ConfigureServices(
						services =>
						{
							services.AddSportLevelProviderOrleans(configuration);
						}
					);

					// Grains 
					//siloBuilder.ConfigureApplicationParts(parts => parts.AddFromAppDomain());
					//siloBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences());
					siloBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ProviderManagerGrain).Assembly).WithReferences());
					//siloBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IProviderManagerGrain).Assembly));

					// Tasks
					siloBuilder.AddStartupTask<ProviderManagerStartupTask>();
				}
			);

			return hostBuilder;
		}
	}
}
