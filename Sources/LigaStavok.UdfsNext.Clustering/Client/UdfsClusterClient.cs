using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;

namespace LigaStavok.UdfsNext.Orleans.Client
{
	public class UdfsClusterClient : IUdfsClusterClient
	{
		private readonly UdfsClusterClientOptions options;

		internal UdfsClusterClient(
			UdfsClusterClientOptions options, 
			IClusterClient clusterClient
		)
		{
			this.options = options;
			ClusterClient = clusterClient;
		}

		public IClusterClient ClusterClient { get; }

		public int ConnectionRetryCount => options.ConnectionRetryCount;

		public TimeSpan ConnectionRetryDelay => options.ConnectionRetryDelay;

	}
}
