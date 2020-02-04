using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;

namespace LigaStavok.UdfsNext.Clustering.Client
{
	public class UdfsClusterClient<TCluster> : IUdfsClusterClient<TCluster>
	{
		public IClusterClient ClusterClient { get; }
		private readonly UdfsClusterClientOptions<TCluster> options;
		private readonly ILogger<UdfsClusterClientOptions<TCluster>> logger;

		public UdfsClusterClient(UdfsClusterClientOptions<TCluster> options, ILogger<UdfsClusterClientOptions<TCluster>> logger)
		{
			this.options = options;
			this.logger = logger;
			ClusterClient = new ClusterClientBuilder<TCluster>(options).Build();
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var attempt = 0;
			var delay = TimeSpan.FromSeconds(1);

			await ClusterClient.Connect(async error =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return false;
				}

				if (++attempt < options.ConnectionRetryCount)
				{
					logger.LogWarning(error,
						"Failed to connect to UDFS cluster on attempt {@Attempt} of {@MaxAttempts}.",
						attempt, options.ConnectionRetryCount);

					try
					{
						await Task.Delay(delay, cancellationToken);
					}
					catch (OperationCanceledException)
					{
						return false;
					}

					return true;
				}
				else
				{
					logger.LogError(error,
						"Failed to connect to UDFS cluster on attempt {@Attempt} of {@MaxAttempts}.",
						attempt, options.ConnectionRetryCount);

					return false;
				}
			});
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await ClusterClient.AbortAsync();
		}
	}
}
