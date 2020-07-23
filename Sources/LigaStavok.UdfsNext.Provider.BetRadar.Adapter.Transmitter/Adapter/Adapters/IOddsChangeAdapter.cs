using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public interface IOddsChangeAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<OddsChange> messageContext, LineService lineService);
	}
}