using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public class LoginRequest : IWebSocketRequest
	{
		[JsonProperty("msg_id")]
		public string MsgId { get; } = Guid.NewGuid().ToString();

		[JsonProperty("msg_type")]
		public string MsgType { get => "login_request"; }

		[JsonProperty("username")]
		public string UserName { get; set; }

		[JsonProperty("auth_key")]
		public string AuthKey { get; set; }
	}
}
