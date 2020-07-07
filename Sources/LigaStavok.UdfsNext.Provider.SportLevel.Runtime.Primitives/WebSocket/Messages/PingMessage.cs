using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages
{
	public class PingMessage 
	{
		[JsonProperty("msg_id")]
		public Guid MsgId { get; set; }

		[JsonProperty("msg_type")]
		public string MsgType { get; set; }

		[JsonProperty("timestamp")]
		public double Timestamp { get; set; }
	}
}
