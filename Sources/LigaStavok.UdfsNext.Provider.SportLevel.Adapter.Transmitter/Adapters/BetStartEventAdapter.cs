using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Extensions;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public class BetStartEventAdapter : IBetStartEventAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<EventData> context)
		{
			yield return new StartGameEventBetsCommand
			(
				lineService: context.ProductType.ToLineService(),
				gameEventId: context.Message.TranslationId,
				receivedOn: context.ReceivedOn,
				incomingId: context.IncomingId
			);
		}
	}
}
