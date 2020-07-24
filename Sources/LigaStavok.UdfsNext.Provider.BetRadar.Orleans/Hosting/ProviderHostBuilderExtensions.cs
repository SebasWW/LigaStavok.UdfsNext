using System;
using Microsoft.Extensions.Hosting;
using Orleans.Hosting;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Hosting
{
	public static class ProviderHostBuilderExtensions
	{
		public static IProviderHostBuilder UseOrleans(this IProviderHostBuilder providerHostBuilder, Action<ISiloBuilder> configureHandler)
		{
			providerHostBuilder.HostBuilder.UseBetRadarProviderOrleans(configureHandler);

			return providerHostBuilder;
		}
	}
}
