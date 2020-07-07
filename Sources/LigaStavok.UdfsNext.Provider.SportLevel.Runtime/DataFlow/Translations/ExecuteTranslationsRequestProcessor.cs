using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.Threading;
using LigaStavok.Threading.Processors;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Translations
{
	public class ExecuteTranslationsRequestProcessor : AsyncQueueProcessor<MessageContext<HttpRequestMessage>>, IExecuteTranslationsRequestProcessor
	{
		private readonly ILogger<ExecuteTranslationsRequestProcessor> logger;
		private readonly HttpClientManager httpClientManager;
		private readonly IParseTranslationsResponseProcessor asyncProcessor;

		public ExecuteTranslationsRequestProcessor(
			ILogger<ExecuteTranslationsRequestProcessor> logger,
			HttpClientManager httpClientManager,
			IParseTranslationsResponseProcessor asyncProcessor
		)
		{
			this.logger = logger;
			this.httpClientManager = httpClientManager;
			this.asyncProcessor = asyncProcessor;
		}
		protected async override Task OnNext(MessageContext<HttpRequestMessage> message)
		{
			HttpResponseMessage request;

			try
			{
				request = await httpClientManager.SendAsync(message.Message);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "HttpRequestMessage execution error.");
				return;
			}

			try
			{
				asyncProcessor.Enqueue(message.Next(request));
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
