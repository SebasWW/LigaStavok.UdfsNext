using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Extensions;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public class BetStopEventAdapter : IBetStopEventAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<EventData> context)
		{
			yield return new StopGameEventBetsCommand
			(
				lineService: context.ProductType.ToLineService(),
				gameEventId: context.Message.TranslationId,
				receivedOn: context.ReceivedOn,
				incomingId: context.IncomingId,
				reason: BetStopReason.FeedCommand,
				extraAttributes: null
			);
		}
	}
}
