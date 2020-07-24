using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Console
{
	class Program
	{
		static Task Main(string[] args)
		{
			return CreateHostBuilder(args).Build().RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.UseBetRadarProviderHost();
		}
	}
}
