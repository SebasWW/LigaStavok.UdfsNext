using System;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.DataFlow;
using LigaStavok.WebSocket;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class FeedListener : IFeedListener
	{
		private readonly FeedListenerFlow feedListenerFlow;
		private readonly IWebSocketClient webSocketClient;
		private readonly IFeedManager feedManager;
		private readonly IFeedSubscriber feedSubscriber;
		private readonly FeedListenerOptions options;

		public FeedListener(
			FeedListenerFlow feedListenerFlow,
			IWebSocketClient webSocketClient,
			IFeedManager feedManager,
			IFeedSubscriber feedSubscriber,
			IOptions<FeedListenerOptions> options
		)
		{
			this.feedListenerFlow = feedListenerFlow;
			this.webSocketClient = webSocketClient;
			this.feedManager = feedManager;
			this.feedSubscriber = feedSubscriber;
			this.options = options.Value;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			webSocketClient.OnConnected += WebSocketClient_OnConnected;
			webSocketClient.OnDisconnected += WebSocketClient_OnDisconnected;
			webSocketClient.OnMessage += WebSocketClient_OnMessage;

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			webSocketClient.OnMessage -= WebSocketClient_OnMessage;
			webSocketClient.OnDisconnected -= WebSocketClient_OnDisconnected;
			webSocketClient.OnConnected -= WebSocketClient_OnConnected;

			feedSubscriber.StopAsync(CancellationToken.None);

			return Task.CompletedTask;
		}

		private void WebSocketClient_OnConnected(object sender, EventArgs e)
		{
			Task.Run(() => feedManager.LoginAsync(options.UserName, options.Password));
		}

		private void WebSocketClient_OnDisconnected(object sender, EventArgs e)
		{
			feedSubscriber.StopAsync(CancellationToken.None);
		}

		private void WebSocketClient_OnMessage(object sender, TextMessageReceivedEventArgs e)
		{
			feedListenerFlow.Post(new MessageContext<string>(e.MessageText));
		}
	}
}
