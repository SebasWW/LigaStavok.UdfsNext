using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel;
//using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter
{
	public class TransmitterAdapter : IProviderAdapter
	{
		private readonly Logger<TransmitterAdapter> logger;

		public TransmitterAdapter(
			Logger<TransmitterAdapter> logger

		)
		{
			this.logger = logger;
		}

		public Task SendTranslationAsync(MessageContext<Translation> messageContext)
		{
		//	var (Ok, Reason) = ClientActor.CheckTranslation(ctx.Message, _adapterConfiguration);
		//	if (!Ok)
		//	{
		//		logger.LogWarning($"Translation validation is failed. Id: {messageContext.Message.Id}, Reason: {Reason}");
		//		return Array.Empty<ITransmitterCommand>();
		//	}

		//	var commands = translationAdapter.Adapt(messageContext);

			return Task.CompletedTask;
		}

		public Task SendEventsAsync(MessageContext<EventsMessage> messageContext)
		{
			//var commands = betStartEventAdapter.Adapt(messageContext);

			return Task.CompletedTask;
		}

		public Task SendPingAsync(MessageContext<PingMessage> msg)
		{
			throw new NotImplementedException();
		}
	}
}
