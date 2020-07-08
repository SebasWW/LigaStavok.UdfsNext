using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.Threading.Processors;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Subscription
{

	public class CreateTranslationRequestProcessor : AsyncQueueProcessor<MessageContext<TranslationRequest>>, ICreateTranslationRequestProcessor
	{
		private readonly ILogger<CreateTranslationRequestProcessor> logger;
		private readonly IHttpRequestMessageFactory httpRequestMessageFactory;
		private readonly IExecuteTranslationRequestProcessor asyncProcessor;

		public CreateTranslationRequestProcessor(
			ILogger<CreateTranslationRequestProcessor> logger,
			IHttpRequestMessageFactory httpRequestMessageFactory,
			IExecuteTranslationRequestProcessor asyncProcessor
		)
		{
			this.logger = logger;
			this.httpRequestMessageFactory = httpRequestMessageFactory;
			this.asyncProcessor = asyncProcessor;
		}

		protected override Task OnNext(MessageContext<TranslationRequest> message)
		{
			HttpRequestMessage request;

			try
			{
				request = httpRequestMessageFactory.Create(message.Message);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "HttpRequestMessage building error.");
				return Task.CompletedTask;
			}

			try
			{
				asyncProcessor.Enqueue(message.Next(request));
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Sending to next processor error.");
				return Task.CompletedTask;
			}

			return Task.CompletedTask;
		}
	}
}
