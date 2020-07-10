using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public interface IBetStartEventAdapter
	{
		System.Collections.Generic.IEnumerable<ITransmitterCommand> Adapt(MessageContext<EventData> context);
	}
}