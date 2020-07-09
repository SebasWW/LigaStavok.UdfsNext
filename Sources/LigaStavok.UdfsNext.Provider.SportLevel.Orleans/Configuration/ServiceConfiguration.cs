using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Configuration;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans.Configuration
{
	public class ServiceConfiguration
	{
		public ClusterConfiguration Cluster { get; set; }

		public ProviderConfiguration Provider { get; set; }
	}
}
