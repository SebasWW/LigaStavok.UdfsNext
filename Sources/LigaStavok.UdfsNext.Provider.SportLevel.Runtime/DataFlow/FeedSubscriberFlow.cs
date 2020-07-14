﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow
{
	public class FeedSubscriberFlow
	{
		private readonly ILogger<FeedManager> logger;

		private readonly TransformManyBlock<MessageContext<TranslationRequest, TranslationSubscription>,
			MessageContext<HttpRequestMessage, TranslationSubscription>> translationCreateRequestBlock;
		private readonly TransformManyBlock<MessageContext<HttpRequestMessage, TranslationSubscription>,
			MessageContext<HttpResponseMessage, TranslationSubscription>> translationExecRequestBlock;
		private readonly TransformManyBlock<MessageContext<HttpResponseMessage, TranslationSubscription>,
			MessageContext<string, TranslationSubscription>> translationCheckHashBlock;
		private readonly TransformManyBlock<MessageContext<string, TranslationSubscription>,
			MessageContext<Translation, TranslationSubscription>> translationParseResponseBlock;

		private readonly BroadcastBlock<MessageContext<Translation>> translationRouterBlock;
		private readonly ActionBlock<MessageContext<Translation>> translationToAdapterBlock;
		private readonly ActionBlock<MessageContext<Translation>> translationSubscriptionBlock;

		private readonly TranslationSubscriptionCollection subscriptions;
		private readonly HttpClientManager httpClientManager;
		private readonly IHttpRequestMessageFactory httpRequestMessageFactory;
		private readonly IProviderAdapter providerAdapter;
		private readonly IFeedManager feedManager;

		public FeedSubscriberFlow(
			ILogger<FeedManager> logger,
			TranslationSubscriptionCollection subscriptions,

			HttpClientManager httpClientManager,
			IHttpRequestMessageFactory httpRequestMessageFactory,
			IProviderAdapter providerAdapter,
			IFeedManager feedManager
		)
		{
			this.logger = logger;
			this.subscriptions = subscriptions;
			this.httpClientManager = httpClientManager;
			this.httpRequestMessageFactory = httpRequestMessageFactory;
			this.providerAdapter = providerAdapter;
			this.feedManager = feedManager;

			// Flow 1
			translationCreateRequestBlock
				= new TransformManyBlock<MessageContext<TranslationRequest, TranslationSubscription>,
				MessageContext<HttpRequestMessage, TranslationSubscription>>(TranslationCreateRequestHandler);

			// Flow 2
			translationExecRequestBlock
				= new TransformManyBlock<MessageContext<HttpRequestMessage, TranslationSubscription>,
				MessageContext<HttpResponseMessage, TranslationSubscription>>(TranslationExecRequestHandler);

			// Flow 3
			translationCheckHashBlock
				= new TransformManyBlock<MessageContext<HttpResponseMessage, TranslationSubscription>,
				MessageContext<string, TranslationSubscription>>(TranslationCheckHashHandler);

			// Flow 4
			translationParseResponseBlock
				= new TransformManyBlock<MessageContext<string, TranslationSubscription>,
				MessageContext<Translation, TranslationSubscription>>(TranslationMessageParseHandler);

			// Flow 5
			translationRouterBlock
				= new BroadcastBlock<MessageContext<Translation>>(t => t);

			// Flow 6-1
			translationToAdapterBlock
				= new ActionBlock<MessageContext<Translation>>(TranslationToAdapterHandler);

			// Flow 6-2
			translationSubscriptionBlock
				= new ActionBlock<MessageContext<Translation>>(TranslationSubscriptionHandler);

			translationCreateRequestBlock.LinkTo(translationExecRequestBlock);
			translationExecRequestBlock.LinkTo(translationCheckHashBlock);
			translationCheckHashBlock.LinkTo(translationParseResponseBlock);
			translationParseResponseBlock.LinkTo(translationRouterBlock);
			translationRouterBlock.LinkTo(translationToAdapterBlock);
			translationRouterBlock.LinkTo(translationSubscriptionBlock);
		}

		public  void Post(MessageContext<TranslationRequest, TranslationSubscription> messageContext)
		{
			translationCreateRequestBlock.Post(messageContext);
		}

		private IEnumerable<MessageContext<HttpRequestMessage, TranslationSubscription>>
			TranslationCreateRequestHandler(MessageContext<TranslationRequest, TranslationSubscription> messageContext)
		{

			try
			{
				return Enumerable.Repeat(messageContext.NextWithState(httpRequestMessageFactory.Create(messageContext.Message)), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "HttpRequestMessage building error.");
				return Array.Empty<MessageContext<HttpRequestMessage, TranslationSubscription>>();
			}
		}

		private async Task<IEnumerable<MessageContext<HttpResponseMessage, TranslationSubscription>>>
			TranslationExecRequestHandler(MessageContext<HttpRequestMessage, TranslationSubscription> messageContext)
		{
			try
			{
				return Enumerable.Repeat(messageContext.NextWithState(await httpClientManager.SendAsync(messageContext.Message)), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "HttpRequestMessage execution error.");
				return Array.Empty<MessageContext<HttpResponseMessage, TranslationSubscription>>();
			}
		}

		private async Task<IEnumerable<MessageContext<string, TranslationSubscription>>>
			TranslationCheckHashHandler(MessageContext<HttpResponseMessage, TranslationSubscription> messageContext)
		{
			try
			{
				var message = await messageContext.Message.Content.ReadAsStringAsync();
				var newHash = message.GetHashCode();

				// it has not changes.
				if (newHash == messageContext.State.State.MetaHash) return Array.Empty<MessageContext<string, TranslationSubscription>>();
				messageContext.State.State.MetaHash = newHash;

				return Enumerable.Repeat(messageContext.NextWithState(message), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Translation httpResponseMessage parsing error.");
				return Array.Empty<MessageContext<string, TranslationSubscription>>();
			}
		}

		private IEnumerable<MessageContext<Translation, TranslationSubscription>>
			TranslationMessageParseHandler(MessageContext<string, TranslationSubscription> messageContext)
		{
			try
			{
				var translations = JsonConvert.DeserializeObject<TranslationsResponse>(messageContext.Message);
				if (translations == null) throw new Exception("Empty response.");

				return translations
					.Where(t => t.State != "finished" && t.State != "cancelled")
					.Select(t => messageContext.NextWithState(t));
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "TranslationMessage parsing error.");
				return Array.Empty<MessageContext<Translation, TranslationSubscription>>();
			}
		}

		private void TranslationSubscriptionHandler(MessageContext<Translation> messageContext)
		{
			try
			{
				if (subscriptions.TryGetValue(messageContext.Message.Id, out var state))
				{
					var translation = messageContext.Message;

					state.Booking.BookedData = translation.BookedData.HasValue && translation.BookedData.Value;
					state.Booking.BookedMarket = translation.BookedMarket.HasValue && translation.BookedMarket.Value;

					if (state.Booking.BookedData)
						feedManager.SendDataSubscribeRequestAsync(
							messageContext.Next(new TranslationSubscriptionRequest() { Id = translation.Id, State = state.State }),
							CancellationToken.None
						);

					if (state.Booking.BookedMarket)
						feedManager.SendMarketSubscribeRequestAsync(
							messageContext.Next(new TranslationSubscriptionRequest() { Id = translation.Id, State = state.State }),
							CancellationToken.None
						);
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Translation subscription error.");
			}
		}

		private Task TranslationToAdapterHandler(MessageContext<Translation> messageContext)
		{
			try
			{
				return providerAdapter.SendTranslationAsync(messageContext);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Translation to adapter sending error.");
				return Task.CompletedTask;
			}
		}

	}
}
