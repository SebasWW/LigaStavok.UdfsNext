using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public interface IAliveAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<Alive> messageContext);
	}
}