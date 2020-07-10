using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Extensions;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public class PingMessageAdapter : IPingMessageAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<PingMessage> context)
		{
			yield return new KeepAliveCommand
			(
				lineService: context.ProductType.ToLineService(),
				receivedOn: context.ReceivedOn,
				incomingId: context.IncomingId
			);
		}
	}
}