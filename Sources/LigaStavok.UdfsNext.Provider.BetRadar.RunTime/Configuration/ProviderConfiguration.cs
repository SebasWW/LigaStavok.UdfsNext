using System;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Configuration
{
	public class ProviderConfiguration
	{ 
		public WebApiConfiguration WebApi { get; set; }
		public TimeSpan MetaRefreshInterval { get; set; }
	}
}