using LigaStavok.UdfsNext.Configuration;
using LigaStavok.UdfsNext.Dumps.Configuration;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Configuration;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans.Configuration
{
	public class ServiceConfiguration
	{
		public ClusterConfiguration Cluster { get; set; }

		public ProviderConfiguration Provider { get; set; }

		public AdapterConfiguration Adapter { get; set; }

		public DumpConfiguration Dump { get; set; }
	}
}
