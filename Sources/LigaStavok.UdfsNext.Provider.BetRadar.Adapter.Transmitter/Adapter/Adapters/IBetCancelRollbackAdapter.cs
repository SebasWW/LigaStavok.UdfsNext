using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public interface IBetCancelRollbackAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<BetCancelRollback> messageContext);
	}
}