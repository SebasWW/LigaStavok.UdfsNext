using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Configuration
{
	public class WebApiConfiguration
	{
		public string UserName { get; set; }

		public string Password { get; set; }

		public string Url { get; set; }

		public TimeSpan MetaRefreshInterval { get; set; }

	}
}
