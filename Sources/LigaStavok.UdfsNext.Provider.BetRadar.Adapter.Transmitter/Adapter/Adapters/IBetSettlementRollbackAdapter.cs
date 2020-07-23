using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public interface IBetSettlementRollbackAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<BetSettlementRollback> messageContext);
	}
}