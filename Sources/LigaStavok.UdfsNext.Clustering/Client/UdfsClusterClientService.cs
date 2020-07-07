using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Orleans.Client
{
	public class UdfsClusterClientService : IHostedService
	{
		private readonly UdfsClusterClientsLocator clusterClientsLocator;
		private readonly IUdfsClusterClientBuilder clusterClientBuilder;
		private readonly UdfsClusterClientsLocatorOptions clusterClientsLocatorOptions;
		private readonly ILogger<UdfsClusterClientService> logger;

		public UdfsClusterClientService(
			UdfsClusterClientsLocator clusterClientsLocator,
			IOptions<UdfsClusterClientsLocatorOptions> options,
			IUdfsClusterClientBuilder udfsClusterClientBuilder,
			ILogger<UdfsClusterClientService> logger
		)
		{
			this.clusterClientsLocator = clusterClientsLocator;
			this.clusterClientBuilder = udfsClusterClientBuilder;
			this.clusterClientsLocatorOptions = options.Value;
			this.logger = logger;

			foreach (var clusterOptionsEntry in clusterClientsLocatorOptions.Clusters)
			{
				clusterClientsLocator.Add(
					clusterOptionsEntry.Key,
					clusterClientBuilder.Build(clusterOptionsEntry.Value)
				);
			}
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			var tasks = new Collection<Task>();

			foreach (var clientEntry in clusterClientsLocator)
			{
				var attempt = 0;
				var delay = TimeSpan.FromSeconds(1);

				tasks.Add(
					clientEntry.Value.ClusterClient.Connect(
						async exception => await ConnectErrorHandler(exception, clientEntry, cancellationToken, attempt)
					)
				);
			}

			return Task.WhenAll(tasks);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			var tasks = new Collection<Task>();

			foreach (var client in clusterClientsLocator.Values)
			{
				tasks.Add(
					client.ClusterClient.Close()
				);
			}

			return Task.WhenAll(tasks);
		}

		private async Task<bool> ConnectErrorHandler(
			Exception exception,
			KeyValuePair<string ,IUdfsClusterClient> clusterClientEntry,
			CancellationToken cancellationToken,
			int attempt
		)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return false;
			}

			// Retry or exception
			if (clusterClientEntry.Value.ConnectionRetryCount == 0 || ++attempt < clusterClientEntry.Value.ConnectionRetryCount)
			{
				logger.LogWarning(exception,
					$"Failed to connect to UDFS cluster {clusterClientEntry.Key} on attempt {attempt} of {clusterClientEntry.Value.ConnectionRetryCount}.");

				try
				{
					await Task.Delay(clusterClientEntry.Value.ConnectionRetryDelay, cancellationToken);
				}
				catch (OperationCanceledException)
				{
					return false;
				}

				return true;
			}
			else
			{
				logger.LogError(exception,
					$"Failed to connect to UDFS cluster {clusterClientEntry.Key} on attempt {attempt} of {clusterClientEntry.Value.ConnectionRetryCount}.");

				return false;
			}
		}
	}
}
