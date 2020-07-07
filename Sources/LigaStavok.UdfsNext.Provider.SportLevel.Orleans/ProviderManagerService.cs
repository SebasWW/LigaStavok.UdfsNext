using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public class ProviderManagerService : BackgroundService
	{
		private readonly ILogger<ProviderManagerService> _logger;

		public ProviderManagerService(

			ILogger<ProviderManagerService> logger
		)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}
