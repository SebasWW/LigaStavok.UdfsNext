﻿using System;
using LigaStavok.UdfsNext.Hosting;
using Microsoft.Extensions.Configuration;
using Orleans.Hosting;
using Orleans;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans.Configuration;

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
					siloBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences());

					// Tasks
					siloBuilder.AddStartupTask<ProviderManagerStartupTask>();
				}
			);

			return hostBuilder;
		}
	}
}
