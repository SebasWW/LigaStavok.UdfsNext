using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Provider.BetRadar.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.UdfsNext.Provider.BetRadar.DependencyInjection
{
	public static class ProviderBuilderExtensions
	{
		public static IProviderBuilder AddTransmitterAdapter(this IProviderBuilder providerBuilder, AdapterConfiguration adapterConfiguration)
		{
			providerBuilder.ServiceCollection
						// Adapter
						.AddBetRadarTransmitterAdapter()
						.AddSingleton(adapterConfiguration) // Перепилить на Options
						;

			return providerBuilder;
		}
	}
}
