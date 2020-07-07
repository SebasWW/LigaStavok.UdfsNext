using LigaStavok.Threading;
using LigaStavok.UdfsNext.Provider.SportLevel.State;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using LigaStavok.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class TranslationManager : SingleThreadProcessor<MessageContext<string>>, IHostedService, ITranslationManager
	{
		private readonly Logger<TranslationManager> logger;
		private readonly IWebsocketClient webSocketClient;

		private readonly IProviderAdapter providerAdapter;
		private readonly IWebSocketMessageParser webSocketMessageParser;
		private readonly IStateManager stateManager;
		private readonly TranslationManagerOptions options;
		private CancellationTokenSource websocketCancellationTokenSource = null;
		private readonly ConcurrentDictionary<long, TranslationSubscriptionRequest> subscriptions = new ConcurrentDictionary<long, TranslationSubscriptionRequest>();

		public TranslationManager(
			IOptions<TranslationManagerOptions> options,
			Logger<TranslationManager> logger,
			IWebsocketClient webSocketClient,
			IProviderAdapter providerAdapter,
			IWebSocketMessageParser webSocketMessageParser,
			IStateManager stateManager
		)
		{
			this.logger = logger;
			this.webSocketClient = webSocketClient;
			this.providerAdapter = providerAdapter;
			this.webSocketMessageParser = webSocketMessageParser;
			this.stateManager = stateManager;
			this.options = options.Value;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			webSocketClient.OnConnected += WebSocketClient_OnConnected;
			webSocketClient.OnMessage += WebSocketClient_OnMessage;

			websocketCancellationTokenSource = new CancellationTokenSource();
			webSocketClient.Start(websocketCancellationTokenSource.Token);

			return base.StartAsync(cancellationToken);
		}

		public override Task StopAsync(CancellationToken cancellationToken)
		{
			websocketCancellationTokenSource.Cancel();

			webSocketClient.OnConnected -= WebSocketClient_OnConnected;
			webSocketClient.OnMessage -= WebSocketClient_OnMessage;

			return base.StartAsync(cancellationToken);
		}

		public Task SendAsync(MessageContext<string> context, CancellationToken cancellationToken)
		{
			return webSocketClient.SendAsync(context.Message, cancellationToken);
		}

		private void WebSocketClient_OnMessage(object sender, TextMessageReceivedEventArgs e)
		{
			Enqueue(new MessageContext<string>(e.MessageText));
		}

		private void WebSocketClient_OnConnected(object sender, EventArgs e)
		{
			Task.Run(() => this.LoginAsync(options.UserName, options.Password));
		}

		public Task SubscribeAsync(MessageContext<TranslationSubscriptionRequest> messageContext, CancellationToken cancellationToken)
		{
			var request = subscriptions.AddOrUpdate(messageContext.Message.Id, id => messageContext.Message, (id, old) => messageContext.Message);
			
			return this.SendSubscribeRequestAsync(messageContext, cancellationToken);
		}

		public Task UnsubscribeAsync(MessageContext<TranslationUnsubscriptionRequest> messageContext, CancellationToken cancellationToken)
		{
			if (subscriptions.TryRemove(messageContext.Message.Id, out var request))
				return this.SendUnsubscribeRequestAsync(messageContext, cancellationToken);
			else
				return Task.CompletedTask;
		}

		public override async Task OnProcess(MessageContext<string> messageContext)
		{
			var message = webSocketMessageParser.Parse(messageContext.Message);

			switch (message)
			{
				case LoginResponseMessage msg:

					await Task.WhenAll(subscriptions.Select(t => this.SendSubscribeRequestAsync(messageContext.Next(t.Value), CancellationToken.None)).ToArray());
					
					break;
				case PingMessage msg:

					// Response to feed
					await this.SendPongRequestAsync(messageContext.Next(msg));

					// Alive to db
					await providerAdapter.PingAsync(messageContext.Next(msg));
					break;

				case EventsMessage msg:

					// Sending market events only
					foreach (var item in msg.Events)
					{
						providerAdapter.EventAsync(messageContext.Next(item));
					}
					break;

				case SubscribeResponseMessage msg:

					// Todo
					break;

				case SubscribeHistorySentMessage msg:

					// Todo
					break;

				case Translation msg:

					_adapterActor.Tell(messageContext.Next(msg));
					break;

				default:
					throw new ArgumentException("Unexpected message received.");
			}
		}
	}
}
