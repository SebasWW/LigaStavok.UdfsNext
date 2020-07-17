using System;
using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Configuration;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter
{
	public sealed class TransmitterCommandsFactory : ITransmitterCommandsFactory
	{
		private readonly AdapterConfiguration adapterConfiguration;
		private readonly IDataEventAdapter dataEventAdapter;
		private readonly IMarketEventAdapter marketEventAdapter;
		private readonly ITranslationAdapter translationAdapter;
		private readonly IBetStartEventAdapter betStartEventAdapter;
		private readonly IBetStopEventAdapter betStopEventAdapter;
		private readonly IPingMessageAdapter pingMessageAdapter;

		public TransmitterCommandsFactory(
			AdapterConfiguration adapterConfiguration,

			IDataEventAdapter dataEventAdapter,
			IMarketEventAdapter marketEventAdapter,
			ITranslationAdapter translationAdapter,
			IBetStartEventAdapter betStartEventAdapter,
			IBetStopEventAdapter betStopEventAdapter,
			IPingMessageAdapter pingMessageAdapter
		)
		{
			this.adapterConfiguration = adapterConfiguration;
			this.dataEventAdapter = dataEventAdapter;
			this.marketEventAdapter = marketEventAdapter;
			this.translationAdapter = translationAdapter;
			this.betStartEventAdapter = betStartEventAdapter;
			this.betStopEventAdapter = betStopEventAdapter;
			this.pingMessageAdapter = pingMessageAdapter;
		}

		public IEnumerable<ITransmitterCommand> CreateFromPingMessage(MessageContext<PingMessage> messageContext)
		{
			return pingMessageAdapter.Adapt(messageContext);
		}

		public IEnumerable<ITransmitterCommand> CreateFromTranslation(MessageContext<Translation> messageContext)
		{
			return translationAdapter.Adapt(messageContext);
		}

		public IEnumerable<ITransmitterCommand> CreateFromEventData(MessageContext<EventData, TranslationSubscription> messageContext)
		{
			switch (messageContext.Message.EventCode)
			{
				case EventCode.HEARTBEAT:
					return Array.Empty<ITransmitterCommand>();

				case EventCode.MARKET:
					return marketEventAdapter.Adapt(messageContext);

				case EventCode.BETSTART:
					return betStartEventAdapter.Adapt(messageContext);

				case EventCode.BETSTOP:
					return betStopEventAdapter.Adapt(messageContext);

				//case EventCode.GAME_START_DELAYED:
				//case EventCode.WARM_UP:
				//case EventCode.PLAYERS_ON_PITCH:
				//case EventCode.MEDICAL_TIMEOUT:
				//case EventCode.TIMEOUT:
				//case EventCode.PLAYER_WITHDRAWAL:
				//case EventCode.GAME_INTERRUPTED:
				//case EventCode.FINISH_GAME:
				//case EventCode.START_PERIOD:
				//case EventCode.POSESSION_1:
				//case EventCode.POSESSION_2:

				default:
					return dataEventAdapter.Adapt(messageContext);
			}
		}
	}
}
