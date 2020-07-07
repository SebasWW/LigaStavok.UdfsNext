using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages
{
	public class SubscribeResponseMessage
	{
		[JsonProperty("msg_id")]
		public Guid MsgId { get; set; }

		[JsonProperty("msg_type")]
		public string MsgType { get; set; }

		[JsonProperty("translation_id")]
		public string TranslationId { get; set; }

		[JsonProperty("scout_id")]
		public long ScoutId { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("response_to")]
		public string ResponseTo { get; set; }
	}
}
