using Microsoft.Extensions.Hosting;
using Orleans;
using System;

namespace LigaStavok.UdfsNext.Orleans.Client
{
	public interface IUdfsClusterClient 
	{
		IClusterClient ClusterClient { get; }

		int ConnectionRetryCount { get; }

		TimeSpan ConnectionRetryDelay { get; }
	}
} 