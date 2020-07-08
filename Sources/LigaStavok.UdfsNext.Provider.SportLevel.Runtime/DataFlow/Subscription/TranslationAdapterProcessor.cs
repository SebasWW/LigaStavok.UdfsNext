using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.Threading.Processors;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Subscription
{
	public class TranslationAdapterProcessor : AsyncQueueProcessor<MessageContext<Translation>>, ITranslationAdapterProcessor
	{
		private readonly ILogger<TranslationAdapterProcessor> logger;
		private readonly IProviderAdapter providerAdapter;

		public TranslationAdapterProcessor(

			ILogger<TranslationAdapterProcessor> logger,
			IProviderAdapter providerAdapter
		)
		{
			this.logger = logger;
			this.providerAdapter = providerAdapter;
		}

		protected override async Task OnNext(MessageContext<Translation> message)
		{

			try
			{
				await providerAdapter.SendTranslationAsync(message);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Sending to adapter error.");
				return;
			}
		}
	}
}
