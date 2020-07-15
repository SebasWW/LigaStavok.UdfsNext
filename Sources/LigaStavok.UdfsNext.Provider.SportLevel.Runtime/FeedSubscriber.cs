using System;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.DataFlow;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class FeedSubscriber : IFeedSubscriber
	{
		private readonly ILogger<FeedSubscriber> logger;
		private readonly FeedSubscriberFlow feedSubscriptionFlow;
		private readonly TranslationSubscriptionCollection subscriptions;
		private readonly IFeedManager feedManager;

		CancellationTokenSource stoppingTokenSource;

		public FeedSubscriber(
			ILogger<FeedSubscriber> logger,
			FeedSubscriberFlow subscriptionFlow,
			TranslationSubscriptionCollection subscriptions,
			IFeedManager feedManager
		)
		{
			this.logger = logger;
			this.feedSubscriptionFlow = subscriptionFlow;
			this.subscriptions = subscriptions;
			this.feedManager = feedManager;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			stoppingTokenSource?.Cancel();
			stoppingTokenSource = new CancellationTokenSource();

			Task.Run(() => ExecuteAsync(stoppingTokenSource.Token));

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			stoppingTokenSource.Cancel();
			return Task.CompletedTask;
		}

		private async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			// Reseting hash for first call
			foreach (var subscription in subscriptions.ToArray())
				subscription.Value.MetaHash = 0;

			while (!stoppingToken.IsCancellationRequested)
			{
				foreach (var subscription in subscriptions.ToArray())
					try
					{
						feedSubscriptionFlow.Post(
							new MessageContext<TranslationRequest, TranslationSubscription>(
								new TranslationRequest()
								{
									Id = subscription.Key
								},
								subscription.Value
							)
						);
					}
					catch (Exception ex)
					{
						logger.LogCritical(ex, $"Translation meta request error. TranslationId: {subscription.Key}.");
					}
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
				}
				catch (Exception) { }
			}
		}

		public Task SubscribeAsync(MessageContext<TranslationSubscriptionRequest> messageContext, CancellationToken cancellationToken)
		{
			subscriptions.TryAdd(messageContext.Message.Id, new TranslationSubscription() { PersistableState = messageContext.Message.State });
			return Task.CompletedTask;
		}

		public Task UnsubscribeAsync(MessageContext<TranslationUnsubscriptionRequest> messageContext, CancellationToken cancellationToken)
		{
			Task marketTask, dataTask;

			if (subscriptions.TryRemove(messageContext.Message.Id, out var subscription))
			{
				if (subscription.Booking.BookedMarket)
					marketTask = feedManager.SendMarketUnsubscribeRequestAsync(messageContext, cancellationToken);
				else
					marketTask = Task.CompletedTask;

				if (subscription.Booking.BookedData)
					dataTask = feedManager.SendDataUnsubscribeRequestAsync(messageContext, cancellationToken);
				else
					dataTask = Task.CompletedTask;

				return Task.WhenAll(dataTask, marketTask);
			}

			return Task.CompletedTask;
		}
	}
}
