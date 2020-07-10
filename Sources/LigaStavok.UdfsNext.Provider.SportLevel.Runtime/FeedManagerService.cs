using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using LigaStavok.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class FeedManagerService : IHostedService
	{
		private readonly IFeedManager translationManager;
		private readonly ILogger<FeedManagerService> logger;
		private readonly IWebSocketClient webSocketClient;
		private readonly IWebSocketMessageParser webSocketMessageParser;
		private readonly IProviderAdapter providerAdapter;
		private readonly IFeedManager feedManager;
		private readonly TransformManyBlock<MessageContext<string>, MessageContext<object>> parseFeedMessageBlock;
		private readonly ActionBlock<MessageContext<object>> routeMessageBlock;
		private CancellationTokenSource stoppingTokenSource;

		public FeedManagerService(
			ILogger<FeedManagerService> logger,
			IWebSocketClient webSocketClient,

			IWebSocketMessageParser webSocketMessageParser,
			IProviderAdapter providerAdapter,
			IFeedManager feedManager
		)
		{
			this.logger = logger;
			this.webSocketClient = webSocketClient;
			this.webSocketMessageParser = webSocketMessageParser;
			this.providerAdapter = providerAdapter;
			this.feedManager = feedManager;
			parseFeedMessageBlock
				= new TransformManyBlock<MessageContext<string>, MessageContext<object>>(ParseFeedMessageBlock);

			routeMessageBlock
				= new ActionBlock<MessageContext<object>>(RouteMessageBlock);

			parseFeedMessageBlock.LinkTo(routeMessageBlock);
		}

		private void RouteMessageBlock(MessageContext<object> messageContext)
		{
			try
			{
				switch (messageContext.Message)
				{
					case LoginResponseMessage msg:
						if (msg.Status != "Ok") throw new AuthenticationException();

						stoppingTokenSource?.Cancel();
						stoppingTokenSource = new CancellationTokenSource();

						feedManager.ExecuteAsync(stoppingTokenSource.Token);

						break;

					case PingMessage msg:
						providerAdapter.SendPingAsync(messageContext.Next(msg));
						break;

					case EventsMessage msg:
						providerAdapter.SendEventsAsync(messageContext.Next(msg));
						break;

					case Translation msg:
						providerAdapter.SendTranslationAsync(messageContext.Next(msg));
						break;

					default:
						throw new Exception($"Unknown message. {messageContext.Message.GetType().FullName}");
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Routing message error.");
			}
		}

		private IEnumerable<MessageContext<object>> ParseFeedMessageBlock(MessageContext<string> messageContext)
		{
			try
			{
				return Enumerable.Repeat(messageContext.Next(webSocketMessageParser.Parse(messageContext.Message)), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Parsing message error.");
				return Array.Empty<MessageContext<object>>();
			}
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			webSocketClient.OnDisconnected += WebSocketClient_OnDisconnected;
			webSocketClient.OnMessage += WebSocketClient_OnMessage;
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			webSocketClient.OnMessage -= WebSocketClient_OnMessage;
			stoppingTokenSource.Cancel();

			return Task.CompletedTask;
		}

		private void WebSocketClient_OnDisconnected(object sender, EventArgs e)
		{
			stoppingTokenSource?.Cancel();
		}

		private void WebSocketClient_OnMessage(object sender, TextMessageReceivedEventArgs e)
		{
			parseFeedMessageBlock.Post(new MessageContext<string>(e.MessageText));
		}

	}
}
