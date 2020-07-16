using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Orleans;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WindowsService
{
	public static class Program
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
				.UseSportLevelProviderHost()
				.UseWindowsService()
				.UseContentRoot(pathToContentRoot)
				.UseOrleans(
					builder =>
					{
						builder.UseDashboard(
							options =>
							{
								options.Port = 8080;
							}
						);
					}
				)
				;
		}
	}
}
