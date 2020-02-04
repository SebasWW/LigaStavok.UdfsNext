using LigaStavok.UdfsNext.Line.Grains;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Microsoft.Extensions.Configuration;

namespace LigaStavok.UdfsNext.Service
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(
					(hostContext, configurationBuilder) =>
					{
						var env = hostContext.HostingEnvironment;
						configurationBuilder
							.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
							.AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
					}
				)
				.UseWindowsService()
                .UseUdfsLineCluster();
    }
}
