using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public class BetStopEventAdapter : IBetStopEventAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<EventData> context)
		{
			yield return new StopGameEventBetsCommand
			(
				lineService:  LineService.SportLevel,
				gameEventId: context.Message.TranslationId,
				receivedOn: context.ReceivedOn,
				incomingId: context.IncomingId,
				reason: BetStopReason.FeedCommand,
				extraAttributes: null
			);
		}
	}
}
