using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Provider.SportLevel.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Hosting
{
	public class ProviderHostBuilder : IProviderHostBuilder
	{
		private IProviderBuilder providerBuilder;

		public ProviderHostBuilder(IHostBuilder hostBuilder)
		{
			HostBuilder = hostBuilder;
		}

		public IHostBuilder HostBuilder { get; }

		public IProviderHostBuilder ConfigureProvider(Action<IProviderBuilder> configureDelegate)
		{
			HostBuilder.ConfigureServices(
				(context, services) =>
				{
					providerBuilder ??= new ProviderBuilder(services);
					configureDelegate.Invoke(providerBuilder);
				}
			);

			return this;
		}
	}
}
