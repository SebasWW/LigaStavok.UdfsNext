using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public abstract class SubscribeRequest
	{

		[JsonProperty("msg_id")]
		public string MsgId { get; } = Guid.NewGuid().ToString();

		[JsonProperty("msg_type")]
		public string MsgType { get; } = "subscribe_request";

		[JsonProperty("translation_id")]
		public long TranslationId { get; set; }

		[JsonProperty("feed")]
		public string Feed { get; protected set; }

		[JsonProperty("last_event_number")]
		public long LastEventNumber { get; set; }

	}
}
