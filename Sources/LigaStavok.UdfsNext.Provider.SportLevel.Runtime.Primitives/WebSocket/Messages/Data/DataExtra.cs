using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data
{
	public class DataExtra
	{
		[JsonProperty("scout")]
		public string Scout { get; set; }

		[JsonProperty("markets")]
		public IEnumerable<Market> Markets { get; set; }

		[JsonProperty("points-1")]
		public int? Points1 { get; set; }

		[JsonProperty("points-2")]
		public int? Points2 { get; set; }

		[JsonProperty("serving-team")]
		public long? ServingTeam { get; set; }

	}
}
