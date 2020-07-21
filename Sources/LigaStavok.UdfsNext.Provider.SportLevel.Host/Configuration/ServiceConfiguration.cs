using LigaStavok.UdfsNext.Configuration;
using LigaStavok.UdfsNext.Provider.SportLevel.Configuration;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Configuration
{
	public class ServiceConfiguration
	{
		public OrleansClusterConfiguration Cluster { get; set; }

		public ProviderConfiguration Provider { get; set; }

		public AdapterConfiguration Adapter { get; set; }

		public DumpConfiguration Dump { get; set; }
	}
}
