using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.Threading;
using LigaStavok.Threading.Processors;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Translations
{
	public class CreateTranslationsRequestProcessor : AsyncQueueProcessor<MessageContext<TranslationsRequest>>, ICreateTranslationsRequestProcessor
	{
		private readonly ILogger<CreateTranslationsRequestProcessor> logger;
		private readonly IHttpRequestMessageFactory httpRequestMessageFactory;
		private readonly IExecuteTranslationsRequestProcessor asyncProcessor;

		public CreateTranslationsRequestProcessor(
			ILogger<CreateTranslationsRequestProcessor> logger,
			IHttpRequestMessageFactory httpRequestMessageFactory,
			IExecuteTranslationsRequestProcessor asyncProcessor
		)
		{
			this.logger = logger;
			this.httpRequestMessageFactory = httpRequestMessageFactory;
			this.asyncProcessor = asyncProcessor;
		}

		protected override Task OnNext(MessageContext<TranslationsRequest> message)
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
