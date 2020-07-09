using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Console
{
	class Program
	{
		static Task Main(string[] args)
		{
			return CreateHostBuilder(args).Build().RunAsync();
		}


		public static IHostBuilder CreateHostBuilder(string[] args)
		{

			// получаем путь к файлу 
			var pathToExe = Process.GetCurrentProcess().MainModule.FileName;

			// путь к каталогу проекта
			var pathToContentRoot = Path.GetDirectoryName(pathToExe);

			return Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(
					builder =>
					{
						builder.AddJsonFile("appsettings.json");
					}
				)
				.UseSportLevelProviderOrleans()
				.ConfigureServices((context, services) =>
				{
					services
						//.Configure<KestrelServerOptions>(context.Configuration.GetSection("Kestrel"))
						.AddLogging(
							configure =>
							{
								configure.AddProvider(new NLogLoggerProvider());
							}
						);
				});
				//.ConfigureWebHostDefaults
				//.UseContentRoot(pathToContentRoot)
				//.UseKestrel()
				//.UseStartup<Startup>();

		}
	}
}
