using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages
{
	public class PartFormat
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("count")]
		public int? Count { get; set; }

		[JsonProperty("duration")]
		public int? Duration { get; set; }
	}
}
