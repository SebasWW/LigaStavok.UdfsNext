using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public interface IBetStopAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<BetStop> messageContext);
	}
}