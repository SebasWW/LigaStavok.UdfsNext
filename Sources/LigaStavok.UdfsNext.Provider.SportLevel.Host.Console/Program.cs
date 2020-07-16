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
			return Host.CreateDefaultBuilder(args)
				.UseSportLevelProviderHost();
		}
	}
}
