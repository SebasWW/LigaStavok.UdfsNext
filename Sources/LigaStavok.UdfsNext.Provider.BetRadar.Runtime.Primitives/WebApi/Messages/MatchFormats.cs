using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Messages
{
	public class MatchFormats
	{
		[JsonProperty("matchFormat")]
		public PartFormat MatchFormat { get; set; }

		[JsonProperty("overtimeFormat")]
		public PartFormat OvertimeFormat { get; set; }
	}
}
