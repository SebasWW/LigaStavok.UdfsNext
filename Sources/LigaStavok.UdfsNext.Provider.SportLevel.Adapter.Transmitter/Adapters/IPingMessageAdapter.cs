using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public interface IPingMessageAdapter
	{
		System.Collections.Generic.IEnumerable<ITransmitterCommand> Adapt(MessageContext<PingMessage> context);
	}
}