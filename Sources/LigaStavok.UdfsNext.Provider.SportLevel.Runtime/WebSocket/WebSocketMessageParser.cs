using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using Newtonsoft.Json.Linq;
using System;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket
{
	public class WebSocketMessageParser : IWebSocketMessageParser
	{
		public object Parse(string text)
		{
			object feedMessage;

			var jtoken = JToken.Parse(text);

			var feedMessageType = jtoken.Value<string>("msg_type");

			switch (feedMessageType)
			{
				case WebSocketMessageType.Ping:
					feedMessage = jtoken.ToObject<PingMessage>();
					break;

				case WebSocketMessageType.LoginResponse:
					feedMessage = jtoken.ToObject<LoginResponseMessage>();
					break;

				case WebSocketMessageType.SubscribeResponse:
					feedMessage = jtoken.ToObject<SubscribeResponseMessage>();
					break;

				case WebSocketMessageType.SubscribeHistorySent:
					feedMessage = jtoken.ToObject<SubscribeHistorySentMessage>();
					break;

				case WebSocketMessageType.UnsubscribeResponse:
					feedMessage = jtoken.ToObject<UnsubscribeResponseMessage>();
					break;

				case WebSocketMessageType.Events:
					feedMessage = jtoken.ToObject<EventsMessage>();
					break;

				case null when jtoken.Value<string>("start_iso8601") != null:

					feedMessage = jtoken.ToObject<Translation>();
					break;

				default:
					throw new NotSupportedException($"Message of specified type ['{feedMessageType}'] can not be parsed.");
			}

			return feedMessage;
		}
	}
}
