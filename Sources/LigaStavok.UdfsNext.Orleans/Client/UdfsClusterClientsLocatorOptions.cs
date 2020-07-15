using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Orleans.Client
{
	public class UdfsClusterClientsLocatorOptions
	{
		public Dictionary<string, UdfsClusterClientOptions> Clusters { get; }
			= new Dictionary<string, UdfsClusterClientOptions>();
	}
}
