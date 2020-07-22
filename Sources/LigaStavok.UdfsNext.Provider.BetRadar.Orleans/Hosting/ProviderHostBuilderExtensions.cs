using System;
using Microsoft.Extensions.Hosting;
using Orleans.Hosting;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Hosting
{
	public static class ProviderHostBuilderExtensions
	{
		public static IProviderHostBuilder UseOrleans(this IProviderHostBuilder providerHostBuilder, Action<ISiloBuilder> configureHandler)
		{
			providerHostBuilder.HostBuilder.UseSportLevelProviderOrleans(configureHandler);

			return providerHostBuilder;
		}
	}
}
