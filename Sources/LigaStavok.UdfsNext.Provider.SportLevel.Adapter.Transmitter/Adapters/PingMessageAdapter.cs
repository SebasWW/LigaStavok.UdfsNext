using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public class PingMessageAdapter : IPingMessageAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<PingMessage> context)
		{
			yield return new KeepAliveCommand
			(
				lineService: LineService.SportLevel,
				receivedOn: context.ReceivedOn,
				incomingId: context.IncomingId
			);
		}
	}
}