using LigaStavok.UdfsNext.Configuration;

namespace LigaStavok.UdfsNext.Orleans.Client
{
	public static class UdfsClusterClientOptionsExtension
	{
		public static void ConfigureWith(this UdfsClusterClientOptions options, ClusterClientConfiguration configuration)
		{
			// Orleans information
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
