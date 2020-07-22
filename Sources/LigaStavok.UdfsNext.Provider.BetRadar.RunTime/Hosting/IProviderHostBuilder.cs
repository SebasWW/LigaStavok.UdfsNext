using System;
using LigaStavok.UdfsNext.Provider.BetRadar.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Hosting
{
	public interface IProviderHostBuilder
	{
		IHostBuilder HostBuilder { get; }

		IProviderHostBuilder ConfigureProvider(Action<IProviderBuilder> configureDelegate);
	}
}