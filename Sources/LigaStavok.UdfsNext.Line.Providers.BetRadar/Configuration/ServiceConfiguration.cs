using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Configuration;

namespace LigaStavok.UdfsNext.Line.Providers.BetRadar.Configuration
{
	public class ServiceConfiguration
	{
		public ClusterConfiguration Cluster { get; set; }
		public ClusterClientConfiguration LineCluster { get; set; }
	}
}
