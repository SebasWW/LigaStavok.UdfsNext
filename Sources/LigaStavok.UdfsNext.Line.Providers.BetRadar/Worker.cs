using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Clustering.Client;
using LigaStavok.UdfsNext.Line.Clustering;
using LigaStavok.UdfsNext.Remoting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Line.Provider.BetRadar
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IUdfsClusterClient<LineMskCluster> udfsClusterClient;

		public Worker(ILogger<Worker> logger, IUdfsClusterClient<LineMskCluster> udfsClusterClient)
		{
			_logger = logger;
			this.udfsClusterClient = udfsClusterClient;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				long i=0;
				var g = udfsClusterClient.ClusterClient;
					var x = g.GetGrain<IUdfsLineEventGrain>(i);

				var c = await x.GetAsync(UdfsRequest.Create("Betradar"));

				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}
