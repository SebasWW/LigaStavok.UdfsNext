using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public class MarketSubscribeRequestParameters
	{
		[JsonProperty("margin")]
		public Decimal Margin { get; set; }
	}
}
