using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public interface IPingMessageAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<PingMessage> context);
	}
}