using Microsoft.Extensions.Hosting;
using Orleans;

namespace LigaStavok.UdfsNext.Clustering.Client
{
	public interface IUdfsClusterClient<TCluster> : IHostedService
	{
		IClusterClient ClusterClient { get; }
	}
} 