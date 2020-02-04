using LigaStavok.UdfsNext.Configuration;

namespace LigaStavok.UdfsNext.Clustering.Client
{
	public static class UdfsClusterClientOptionsExtension
	{
		public static void Configure<TCluster>(this UdfsClusterClientOptions<TCluster> options, ClusterClientConfiguration configuration)
		{
			// Clustering information
			options.ClusterService.ClusterId = configuration.ClusterId;
			options.ClusterService.ServiceId = configuration.ServiceId;

			// Membership provider
			options.Membership.Enabled = configuration.Membership.Enabled;
			options.Membership.ConnectionString = configuration.Membership.ConnectionString;
			options.Membership.Provider = configuration.Membership.Provider;

			// Connection settings
			options.ConnectionRetryCount = configuration.ConnectionRetryCount;
		}
	}
}
