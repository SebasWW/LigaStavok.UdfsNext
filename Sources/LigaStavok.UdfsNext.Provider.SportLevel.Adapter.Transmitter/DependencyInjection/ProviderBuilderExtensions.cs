using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Provider.SportLevel.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DependencyInjection
{
	public static class ProviderBuilderExtensions
	{
		public static IProviderBuilder AddTransmitterAdapter(this IProviderBuilder providerBuilder, AdapterConfiguration adapterConfiguration)
		{
			providerBuilder.ServiceCollection
						// Adapter
						.AddSportLevelTransmitterAdapter()
						.AddSingleton(adapterConfiguration) // Перепилить на Options
						;

			return providerBuilder;
		}
	}
}
