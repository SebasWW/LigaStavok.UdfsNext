using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.WebSocket;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Streaming
{
	public class MessageBuffer
	{
		private readonly IWebSocketClient webSocketClient;

		public MessageBuffer(
			IWebSocketClient webSocketClient
		)
		{
			this.webSocketClient = webSocketClient;
		}

		private void WebSocketClient_OnMessage(object sender, TextMessageReceivedEventArgs e)
		{
			Enqueue(new MessageContext<string>(e.MessageText));
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			webSocketClient.OnMessage += WebSocketClient_OnMessage;


			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{

			webSocketClient.OnMessage -= WebSocketClient_OnMessage;

			return Task.CompletedTask;
		}
	}
}
