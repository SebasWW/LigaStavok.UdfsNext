using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Dumps;
using LigaStavok.UdfsNext.Provider.SportLevel.State;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Responses;
using LigaStavok.UdfsNext.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow
{
	public class FeedSubscriberFlow
	{
		int maxDegreeOfParallelism = 10;
		private TranslationsResponse oldTranslations;

		private readonly ILogger<FeedManager> logger;
		private readonly FeedSubscriberOptions options;
		private readonly IMessageDumper messageDumper;
		private readonly TransformManyBlock<MessageContext<TranslationRequest, TranslationSubscription>,
			MessageContext<HttpRequestMessage, TranslationSubscription>> translationCreateRequestBlock;
		private readonly TransformManyBlock<MessageContext<HttpRequestMessage, TranslationSubscription>,
			MessageContext<HttpResponseMessage, TranslationSubscription>> translationExecRequestBlock;
		private readonly TransformManyBlock<MessageContext<HttpResponseMessage, TranslationSubscription>,
			MessageContext<string, TranslationSubscription>> translationCheckHashBlock;
		private readonly TransformManyBlock<MessageContext<string, TranslationSubscription>,
			MessageContext<Translation, TranslationSubscription>> translationParseResponseBlock;
		private readonly TransformManyBlock<MessageContext<Translation, TranslationSubscription>, MessageContext<Translation, TranslationSubscription>> translationDumpBlock;
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
			IOptions<FeedSubscriberOptions> options,
			IMessageDumper messageDumper,
			TranslationSubscriptionCollection subscriptions,

			HttpClientManager httpClientManager,
			IHttpRequestMessageFactory httpRequestMessageFactory,
			IProviderAdapter providerAdapter,
			IFeedManager feedManager
		)
		{
			this.logger = logger;
			this.options = options.Value;
			this.messageDumper = messageDumper;
			this.subscriptions = subscriptions;
			this.httpClientManager = httpClientManager;
			this.httpRequestMessageFactory = httpRequestMessageFactory;
			this.providerAdapter = providerAdapter;
			this.feedManager = feedManager;

			// Flow 1
			translationCreateRequestBlock
				= new TransformManyBlock<MessageContext<TranslationRequest, TranslationSubscription>,
				MessageContext<HttpRequestMessage, TranslationSubscription>>(
					TranslationCreateRequestHandler, 
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 2
			translationExecRequestBlock
				= new TransformManyBlock<MessageContext<HttpRequestMessage, TranslationSubscription>,
				MessageContext<HttpResponseMessage, TranslationSubscription>>(
					TranslationExecRequestHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 3
			translationCheckHashBlock
				= new TransformManyBlock<MessageContext<HttpResponseMessage, TranslationSubscription>,
				MessageContext<string, TranslationSubscription>>(
					TranslationCheckHashHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 4
			translationParseResponseBlock
				= new TransformManyBlock<MessageContext<string, TranslationSubscription>,
				MessageContext<Translation, TranslationSubscription>>(
					TranslationMessageParseHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 5
			translationDumpBlock
				= new TransformManyBlock<MessageContext<Translation, TranslationSubscription>,
				MessageContext<Translation, TranslationSubscription>>(
					TranslationDumpHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 6
			translationRouterBlock
				= new BroadcastBlock<MessageContext<Translation>>(t => t);

			// Flow 7-1
			translationToAdapterBlock
				= new ActionBlock<MessageContext<Translation>>(
					TranslationToAdapterHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 7-2
			translationSubscriptionBlock
				= new ActionBlock<MessageContext<Translation>>(
					TranslationSubscriptionHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			translationCreateRequestBlock.LinkTo(translationExecRequestBlock);
			translationExecRequestBlock.LinkTo(translationCheckHashBlock);
			translationCheckHashBlock.LinkTo(translationParseResponseBlock);
			translationParseResponseBlock.LinkTo(translationDumpBlock);
			translationDumpBlock.LinkTo(translationRouterBlock);
			translationRouterBlock.LinkTo(translationToAdapterBlock);
			translationRouterBlock.LinkTo(translationSubscriptionBlock);
		}

		private IEnumerable<MessageContext<Translation, TranslationSubscription>> TranslationDumpHandler(MessageContext<Translation, TranslationSubscription> messageContext)
		{
			try
			{
				messageDumper.Write(
					messageContext.Next(
						new DumpMessage()
						{
							Source = DumpSource.FROM_API,
							MessageBody = JsonConvert.SerializeObject(messageContext.Message),
							MessageType = messageContext.Message.GetType().Name,
							EventId = messageContext.Message.Id.ToString()
						}
					)
				);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Dump sending error. ContextId: {messageContext.IncomingId}, TranslationId: {messageContext.Message.Id}");
			}

			return Enumerable.Repeat(messageContext, 1);
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
				logger.LogError(ex, $"HttpRequestMessage building error. ContextId: {messageContext.IncomingId}, TranslationId: {messageContext.Message.Id}");
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
				if (oldTranslations != null)
				{
					logger.LogWarning(ex, $"Translation list retrieving error. Using cache... ContextId: {messageContext.IncomingId}");

					// sending from cache
					foreach (var item in oldTranslations)
					{
						translationSubscriptionBlock.Post(messageContext.Next(item));
					}
				}
				else
					logger.LogError(ex, $"Translation list retrieving error. ContextId: {messageContext.IncomingId}");

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
				if (newHash == messageContext.State.MetaHash) return Array.Empty<MessageContext<string, TranslationSubscription>>();
				messageContext.State.MetaHash = newHash;

				return Enumerable.Repeat(messageContext.NextWithState(message), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Translation httpResponseMessage parsing error. ContextId: {messageContext.IncomingId}");
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

				// Cache for next api errors case
				oldTranslations = translations;

				return translations
					//.Where(t => t.State != "finished" && t.State != "cancelled")
					.Select(t => messageContext.NextWithState(t));
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"TranslationMessage parsing error. ContextId: {messageContext.IncomingId}");
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
							messageContext.Next(new TranslationSubscriptionRequest<TranslationState>() { Id = translation.Id, State = state.PersistableState }),
							CancellationToken.None
						);

					if (state.Booking.BookedMarket)
						feedManager.SendMarketSubscribeRequestAsync(
							messageContext.Next(new TranslationSubscriptionRequest<TranslationState>() { Id = translation.Id, State = state.PersistableState }),
							options,
							CancellationToken.None
						);
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Translation subscription error. ContextId: {messageContext.IncomingId}, TranslationId: {messageContext.Message.Id}");
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
				logger.LogError(ex, $"Translation to adapter sending error. ContextId: {messageContext.IncomingId}, TranslationId: {messageContext.Message.Id}");
				return Task.CompletedTask;
			}
		}
	}
}
