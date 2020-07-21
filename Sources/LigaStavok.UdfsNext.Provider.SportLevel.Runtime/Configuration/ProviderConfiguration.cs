using System;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Configuration
{
	public class ProviderConfiguration
	{
		public string UserName { get; set; }

		public string Password { get; set; }


		public string WebSocketUrl { get; set; }

		public string WebApiUrl { get; set; }

		public TimeSpan MetaRefreshInterval { get;  set; }

		public decimal Margin { get; set; }
	}
}