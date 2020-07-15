using System.Collections.Generic;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.State;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public interface IDataEventAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<EventData, TranslationSubscription> context);
	}
}