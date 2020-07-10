using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Subscription;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using LigaStavok.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class FeedManager : IFeedManager
	{
		private readonly Logger<FeedManager> logger;
		private readonly IWebSocketClient webSocketClient;
		private readonly FeedManagerOptions options;
		private readonly TranslationSubscriptionCollection subscriptions;
		private readonly ICreateTranslationRequestProcessor createTranslationRequestProcessor;

		private CancellationTokenSource forceCancellationTokenSource;

		public FeedManager(
			IOptions<FeedManagerOptions> options,
			Logger<FeedManager> logger,
			IWebSocketClient webSocketClient,
			TranslationSubscriptionCollection subscriptions,
			ICreateTranslationRequestProcessor createTranslationRequestProcessor
		)
		{
			this.logger = logger;
			this.webSocketClient = webSocketClient;
			this.subscriptions = subscriptions;
			this.createTranslationRequestProcessor = createTranslationRequestProcessor;
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

		public async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				foreach (var subscription in subscriptions.ToArray())
					try
					{
						createTranslationRequestProcessor.Enqueue(
							new MessageContext<TranslationRequest>(
								new TranslationRequest()
								{
									Id = subscription.Key
								}
							)
						);
					}
					catch (Exception ex)
					{
						logger.LogCritical(ex, $"Translation meta request error. TranslationId: {subscription.Key}.");
					}

				try
				{
					forceCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
					await Task.Delay(TimeSpan.FromSeconds(60), forceCancellationTokenSource.Token);
				}
				catch (Exception){}
			}
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
