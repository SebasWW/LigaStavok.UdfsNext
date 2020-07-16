using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseSportLevelProviderHost(this IHostBuilder hostBuilder)
		{
			// получаем путь к файлу 
			var pathToExe = Process.GetCurrentProcess().MainModule.FileName;

			// путь к каталогу проекта
			var pathToContentRoot = Path.GetDirectoryName(pathToExe);

			hostBuilder
				.ConfigureAppConfiguration(
					builder =>
					{
						builder.AddJsonFile("appsettings.json");
					}
				)
				.UseSportLevelProviderOrleans();
			//.ConfigureWebHostDefaults
			//.UseContentRoot(pathToContentRoot)
			//.UseKestrel()
			//.UseStartup<Startup>();

			return hostBuilder;
		}
	}
}
