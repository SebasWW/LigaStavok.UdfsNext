using LigaStavok.UdfsNext.Clustering;

namespace LigaStavok.UdfsNext.Configuration
{
	public class ClusterClientConfiguration
	{
		public string ClusterId { get; set; }

		public string ServiceId { get; set; }

		public DbConnectionOptions Membership { get; set; }

		public int ConnectionRetryCount { get; set; }
	}
}
