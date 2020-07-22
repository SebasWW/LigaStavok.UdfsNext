using System;
using LigaStavok.UdfsNext.Provider.BetRadar.Hosting;

namespace Microsoft.Extensions.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseBetRadar(this IHostBuilder hostBuilder, Action<IProviderHostBuilder> configureHandler)
		{

			var builder = new ProviderHostBuilder(hostBuilder);
			configureHandler.Invoke(builder);

			return hostBuilder;
		}
	}
}
