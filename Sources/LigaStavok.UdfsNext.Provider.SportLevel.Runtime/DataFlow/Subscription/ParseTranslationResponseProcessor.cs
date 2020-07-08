using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.Threading.Processors;
using LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Translations;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Responses;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Subscription
{
	public class ParseTranslationResponseProcessor : AsyncQueueProcessor<MessageContext<HttpResponseMessage>>, IParseTranslationResponseProcessor
	{
		private readonly ILogger<ParseTranslationResponseProcessor> logger;
		private readonly IHttpResponseMessageParser httpResponseMessageParser;
		private readonly TranslationSubscriptionBookingAsyncQueue translationAsyncQueue;
		private readonly ITranslationAdapterProcessor translationAdapterProcessor;
		private readonly TranslationSubscriptionCollection translationSubscriptionCollection;

		public ParseTranslationResponseProcessor(

			ILogger<ParseTranslationResponseProcessor> logger,
			IHttpResponseMessageParser httpResponseMessageParser,
			TranslationSubscriptionBookingAsyncQueue translationAsyncQueue,
			ITranslationAdapterProcessor translationAdapterProcessor,
			TranslationSubscriptionCollection translationSubscriptionCollection
		)
		{
			this.logger = logger;
			this.httpResponseMessageParser = httpResponseMessageParser;
			this.translationAsyncQueue = translationAsyncQueue;
			this.translationAdapterProcessor = translationAdapterProcessor;
			this.translationSubscriptionCollection = translationSubscriptionCollection;
		}

		protected override async Task OnNext(MessageContext<HttpResponseMessage> message)
		{
			Translation translation;

			try
			{
				var response = (TranslationsResponse)await httpResponseMessageParser.ParseAsync(message.Message);

				if (response == null) throw new Exception("Null response.");
				if (response.Count == 0) throw new Exception("Empty response.");
				if (response.Count > 1) throw new Exception("Invalid response.");

				translation = response.First();

				if (translation.State == "finished" || translation.State == "cancelled") return;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "HttpResponseMessage parsing error.");
				return;
			}

			try
			{
				translationAsyncQueue.Enqueue(
					message.Next(
						new TranslationSubscriptionBooking()
						{
							BookedData = translation.BookedData.HasValue && translation.BookedData.Value,
							BookedMarket = translation.BookedMarket.HasValue && translation.BookedMarket.Value
						}
					)
				);	
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Sending to next processor error.");
			}

			// Adapter
			try
			{
				translationAdapterProcessor.Enqueue(message.Next(translation));
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Sending to adapter error.");
			}

			// Booking
			try
			{
				if (translationSubscriptionCollection.TryGetValue(translation.Id, out var subscription))
				{
					subscription.Booking = new TranslationSubscriptionBooking()
					{
						BookedData = translation.BookedData.HasValue && translation.BookedData.Value,
						BookedMarket = translation.BookedMarket.HasValue && translation.BookedMarket.Value
					};
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Setting booking error.");
			}

			return;
		}
	}
}
