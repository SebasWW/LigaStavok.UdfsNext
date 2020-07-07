using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages
{
	public class SubscribeHistorySentMessage
	{
		[JsonProperty("msg_id")]
		public Guid MsgId { get; set; }

		[JsonProperty("msg_type")]
		public string MsgType { get; set; }

		[JsonProperty("translation_id")]
		public string TranslationId { get; set; }

		[JsonProperty("scout_id")]
		public long ScoutId { get; set; }
	}
}
