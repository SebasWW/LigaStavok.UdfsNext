using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public interface IDataEventAdapter
	{
		System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<ITransmitterCommand>> AdaptAsync(MessageContext<EventData> context);
	}
}