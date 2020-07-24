using LigaStavok.UdfsNext.Configuration;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Configuration
{
	public class ServiceConfiguration
	{
		public OrleansClusterConfiguration Cluster { get; set; }

		public ProviderConfiguration Provider { get; set; }

		public AdapterConfiguration Adapter { get; set; }

		public DumpConfiguration Dump { get; set; }
	}
}
