using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.Threading.Processors;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Responses;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Translations
{
	public class ParseTranslationsResponseProcessor : AsyncQueueProcessor<MessageContext<HttpResponseMessage>>, IParseTranslationsResponseProcessor
	{
		private readonly ILogger<ParseTranslationsResponseProcessor> logger;
		private readonly IHttpResponseMessageParser httpResponseMessageParser;
		private readonly TranslationAsyncQueue translationAsyncQueue;

		public ParseTranslationsResponseProcessor(

			ILogger<ParseTranslationsResponseProcessor> logger,
			IHttpResponseMessageParser httpResponseMessageParser,
			TranslationAsyncQueue translationAsyncQueue
		)
		{
			this.logger = logger;
			this.httpResponseMessageParser = httpResponseMessageParser;
			this.translationAsyncQueue = translationAsyncQueue;
		}

		protected override async Task OnNext(MessageContext<HttpResponseMessage> message)
		{
			TranslationsResponse response;

			try
			{
				response = (TranslationsResponse) await httpResponseMessageParser.ParseAsync(message.Message);

				if (response == null) throw new Exception("Empty response.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "HttpResponseMessage parsing error.");
				return;
			}

			try
			{
				response
					.Where(t => t.State != "finished" && t.State != "cancelled")
					.AsParallel().ForAll(translation => translationAsyncQueue.Enqueue(message.Next(translation)));

				//foreach (var translation in response)
				//{
				//	translationAsyncQueue.Enqueue(message.Next(translation));
				//}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Sending to next processor error.");
				return;
			}

			return;
		}
	}
}
