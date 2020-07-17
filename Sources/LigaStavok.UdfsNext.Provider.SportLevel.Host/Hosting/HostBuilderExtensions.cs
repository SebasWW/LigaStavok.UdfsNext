using System;
using System.Diagnostics;
using System.IO;
using LigaStavok.UdfsNext.Dumps.DependencyInjection;
using LigaStavok.UdfsNext.Dumps.FileSystem;
using LigaStavok.UdfsNext.Dumps.SqlServer;
using LigaStavok.UdfsNext.Hosting;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

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

			// конфигурация сервиса
			ServiceConfiguration configuration = null;

			hostBuilder
				.ConfigureAppConfiguration(
					builder =>
					{
						builder.AddJsonFile("appsettings.json");
					}
				)
				.ConfigureServices((context, services) =>
					{
						configuration = context.Configuration.Get<ServiceConfiguration>();

						services

						// Adapter
						.AddSportLevelTransmitterAdapter()
						.AddSingleton(configuration.Adapter) // Перепилить на Options

						// Runtime
						.AddSportLevelRunTime(
							builder =>
							{
								builder.ConfigureProviderManager(
									options =>
									{
										options.MetaRefreshInterval = configuration.Provider.MetaRefreshInterval;
									}
								);

								builder.ConfigureFeedListener(
									options =>
									{
										options.UserName = configuration.Provider.UserName;
										options.Password = configuration.Provider.Password;
									}
								);

								builder.ConfigureHttpClientManager(
									options =>
									{
										options.UserName = configuration.Provider.UserName;
										options.Password = configuration.Provider.Password;
										options.Uri = new Uri(configuration.Provider.WebApiUrl);
									}
								);

								builder.AddWebSocketClient(
									options =>
									{
										options.UseDefaultCredentials = true;
										options.Uri = new Uri(configuration.Provider.WebSocketUrl);
									}
								);
							}
						)
						// Dumps
						.AddMessageDumping(
							builder =>
							{
								if (configuration.Dump.SqlServer.Enabled)
									builder.ConfigureSqlServerDumping(
										options =>
										{
											options.Configure(configuration.Dump.SqlServer);
										}
								   );

								if (configuration.Dump.FileSystem.Enabled)
									builder.ConfigureFileSystemDumping(
										options =>
										{
											options.Configure(configuration.Dump.FileSystem);
										}
								   );
							}
						)

						// Kestrel Http
						//.Configure<KestrelServerOptions>(context.Configuration.GetSection("Kestrel"))


						// Logging
						.AddLogging(
							configure =>
							{
								configure.AddProvider(new NLogLoggerProvider());
							}
						)
						;
					}
				)
				.UseSportLevelProviderOrleans(
					siloBuilder =>
					{
						siloBuilder.Configure(configuration.Cluster);
					}
				);
			//.ConfigureWebHostDefaults
			//.UseContentRoot(pathToContentRoot)
			//.UseKestrel()
			//.UseStartup<Startup>();

			return hostBuilder;
		}
	}
}
