using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.DataFlow;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class FeedSubscriber : IFeedSubscriber
	{
		private readonly ILogger<FeedSubscriber> logger;
		private readonly FeedSubscriberFlow feedSubscriptionFlow;
		private readonly TranslationSubscriptionCollection subscriptions;
		private readonly IFeedManager feedManager;
		private readonly IProviderAdapter providerAdapter;

		CancellationTokenSource stoppingTokenSource;

		public FeedSubscriber(
			ILogger<FeedSubscriber> logger,
			FeedSubscriberFlow subscriptionFlow,
			TranslationSubscriptionCollection subscriptions,
			IFeedManager feedManager,
			IProviderAdapter providerAdapter
		)
		{
			this.logger = logger;
			this.feedSubscriptionFlow = subscriptionFlow;
			this.subscriptions = subscriptions;
			this.feedManager = feedManager;
			this.providerAdapter = providerAdapter;
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
			// Disabling bets
			//await providerAdapter.SendEventsAsync(
			//	new MessageContext<EventsMessage>(
			//		new EventsMessage()
			//		{
			//			Events = subscriptions
			//				.Where(t => t.Value.Booking.BookedMarket)
			//				.Select(t => new EventData() { EventCode = EventCode.BETSTOP, TranslationId = t.Key.ToString() })
			//		}
			//	)
			//);

			stoppingTokenSource.Cancel();
			return Task.CompletedTask;
		}

		private async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			// Reseting hash for first call ??? need we it?
			foreach (var subscription in subscriptions.ToArray())
				subscription.Value.MetaHash = 0;

			//// Enabling bets
			//await providerAdapter.SendEventsAsync(
			//	new MessageContext<EventsMessage>(
			//		new EventsMessage()
			//		{
			//			Events = subscriptions
			//				.Where(t => t.Value.Booking.BookedMarket)
			//				.Select(t => new EventData() { EventCode = EventCode.BETSTART, TranslationId = t.Key.ToString() })
			//		}
			//	)
			//);
			
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

		public Task SubscribeAsync(MessageContext<TranslationSubscriptionRequest> messageContext, Action saveStateAction, CancellationToken cancellationToken)
		{
			subscriptions.TryAdd(messageContext.Message.Id, new TranslationSubscription(saveStateAction) { PersistableState = messageContext.Message.State });
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
