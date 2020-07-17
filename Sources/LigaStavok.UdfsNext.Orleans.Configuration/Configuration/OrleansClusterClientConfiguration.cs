using LigaStavok.UdfsNext.Orleans;

namespace LigaStavok.UdfsNext.Configuration
{
	public class OrleansClusterClientConfiguration
	{
		public string ClusterId { get; set; }

		public string ServiceId { get; set; }

		public DbConnectionOptions Membership { get; set; }

		public int ConnectionRetryCount { get; set; }
	}
}
