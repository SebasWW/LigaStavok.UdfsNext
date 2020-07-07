using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages
{
	public class Sport
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }
	}
}
