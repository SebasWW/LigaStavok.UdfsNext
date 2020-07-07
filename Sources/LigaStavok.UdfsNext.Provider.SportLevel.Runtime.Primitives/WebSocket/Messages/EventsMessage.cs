using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages
{
	public class EventsMessage
	{

		[JsonProperty("msg_id")]
		public Guid MsgId { get; set; }

		[JsonProperty("response-to")]
		public string ResponseTo { get; set; }

		[JsonProperty("msg_type")]
		public string MsgType { get; set; }

		[JsonProperty("events")]
		public IEnumerable<EventData> Events { get; set; }
	}
}
