using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Line.Clustering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LigaStavok.UdfsNext.Line.Providers.BetRadar
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
							.AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true,reloadOnChange: true);
					}
				)
				.UseUdfsLineClusterClient()
				//.UseUdfsLineCluster()
				.ConfigureServices((hostContext, services) =>
					{
						services.AddHostedService<Worker>();
					}
				);
	}
}
