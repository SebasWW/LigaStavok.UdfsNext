using System;
using System.Runtime.Serialization;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
	public class HttpClientManagerOptions
	{
		public Uri Uri { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }
	}
}