using System;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Configuration
{
	public class ProviderConfiguration
	{
		public string UserName { get; set; }

		public string Password { get; set; }

		public string WebApiUrl { get; set; }

		public TimeSpan MetaRefreshInterval { get;  set; }

	}
}