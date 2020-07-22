using System;
using LigaStavok.UdfsNext.Provider.SportLevel.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Hosting
{
	public interface IProviderHostBuilder
	{
		IHostBuilder HostBuilder { get; }

		IProviderHostBuilder ConfigureProvider(Action<IProviderBuilder> configureDelegate);
	}
}