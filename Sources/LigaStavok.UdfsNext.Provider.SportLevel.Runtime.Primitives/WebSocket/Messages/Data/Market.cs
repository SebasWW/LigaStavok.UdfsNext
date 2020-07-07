using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data
{
	public class Market
	{
		[JsonProperty("selections")]
		public decimal?[][] Selections { get; set; }

		[JsonProperty("id")]
		public long Id { get; set; }
	}
}
