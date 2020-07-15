using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public class PongRequest : IWebSocketRequest
	{
		[JsonProperty("msg_id")]
		public string MsgId { get; } = Guid.NewGuid().ToString();

		[JsonProperty("msg_type")]
		public string MsgType { get => "pong"; }

		[JsonProperty("timestamp")]
		public double Timestamp { get; set; }
	}
}
