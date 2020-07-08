using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class TranslationManager : ITranslationManager
	{
		private readonly Logger<TranslationManager> logger;
		private readonly IWebSocketClient webSocketClient;

		private readonly TranslationManagerOptions options;
		private readonly TranslationSubscriptionCollection subscriptions;

		public TranslationManager(
			IOptions<TranslationManagerOptions> options,
			Logger<TranslationManager> logger,
			IWebSocketClient webSocketClient,
			TranslationSubscriptionCollection subscriptions
		)
		{
			this.logger = logger;
			this.webSocketClient = webSocketClient;
			this.subscriptions = subscriptions;
			this.options = options.Value;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			webSocketClient.OnConnected += WebSocketClient_OnConnected;
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			webSocketClient.OnConnected -= WebSocketClient_OnConnected;
			return Task.CompletedTask;
		}

		public Task ExecuteAsync(CancellationToken stoppingToken)
		{
			return Task.Run(() => SubscribeAllAsync(stoppingToken));
		}

		private void SubscribeAllAsync(CancellationToken stoppingToken)
		{
			throw new NotImplementedException();
		}

		public Task SendAsync(MessageContext<string> context, CancellationToken cancellationToken)
		{
			return webSocketClient.SendAsync(context.Message, cancellationToken);
		}

		private void WebSocketClient_OnConnected(object sender, EventArgs e)
		{
			Task.Run(() => this.LoginAsync(options.UserName, options.Password));
		}

		public Task SubscribeAsync(MessageContext<TranslationSubscriptionRequest> messageContext, CancellationToken cancellationToken)
		{
			subscriptions.TryAdd(messageContext.Message.Id, new TranslationSubscription() { State = messageContext.Message.State });
			return Task.CompletedTask;
		}

		public Task UnsubscribeAsync(MessageContext<TranslationUnsubscriptionRequest> messageContext, CancellationToken cancellationToken)
		{
			Task marketTask, dataTask;

			if (subscriptions.TryRemove(messageContext.Message.Id, out var subscription))
			{
				if (subscription.Booking.BookedMarket)
					marketTask = this.SendMarketUnsubscribeRequestAsync(messageContext, cancellationToken);
				else
					marketTask = Task.CompletedTask;
			
				if (subscription.Booking.BookedData)
					dataTask = this.SendDataUnsubscribeRequestAsync(messageContext, cancellationToken);
				else
					dataTask = Task.CompletedTask;

				return Task.WhenAll(dataTask, marketTask);
			}

			return Task.CompletedTask;
		}
	}
}
