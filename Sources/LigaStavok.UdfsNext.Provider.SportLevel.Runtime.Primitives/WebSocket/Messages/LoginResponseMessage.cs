using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages
{
	public class LoginResponseMessage
	{
		[JsonProperty("msg_id")]
		public Guid MsgId { get; set; }

		[JsonProperty("msg_type")]
		public string MsgType { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }
	}
}
