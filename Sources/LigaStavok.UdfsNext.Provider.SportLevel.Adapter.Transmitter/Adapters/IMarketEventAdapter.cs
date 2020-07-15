using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.SportLevel.State;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public interface IMarketEventAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<EventData, TranslationSubscription> context);
	}
}