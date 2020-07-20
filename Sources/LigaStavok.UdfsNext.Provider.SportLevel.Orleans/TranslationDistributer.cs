using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using Microsoft.Extensions.Logging;
using Orleans;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public class TranslationDistributer : ITranslationDistributer
	{
		private readonly IGrainFactory grainFactory;
		private readonly ILogger<TranslationDistributer> logger;

		public TranslationDistributer(IGrainFactory grainFactory, ILogger<TranslationDistributer> logger)
		{
			this.grainFactory = grainFactory;
			this.logger = logger;
		}

		public Task Distribute(MessageContext<Translation> messageContext)
		{
			try
			{
				return grainFactory.GetGrain<IFeedSubscriberGrain>(messageContext.Message.Id).InitializeAsync();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Calling IFeedSubscriberGrain({messageContext.Message.Id}).");
				return Task.CompletedTask;
			}
		}
	}
}
