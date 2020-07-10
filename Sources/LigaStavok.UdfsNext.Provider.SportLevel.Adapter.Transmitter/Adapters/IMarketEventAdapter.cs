using System.Collections.Generic;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public interface IMarketEventAdapter
	{
		Task<IEnumerable<ITransmitterCommand>> AdaptAsync(MessageContext<EventData> context);
	}
}