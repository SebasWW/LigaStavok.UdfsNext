using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public abstract class UnsubscribeRequest : ITranslationWebSocketRequest
	{
		[JsonProperty("msg_id")]
		public string MsgId { get; set; }

		[JsonProperty("msg_type")]
		public string MsgType { get => "unsubscribe_request"; }

		[JsonProperty("translation_id")]
		public long TranslationId { get; set; }

		[JsonProperty("feed")]
		public string Feed { get;  set; }
	}
}
