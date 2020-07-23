using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public interface IBetCancelAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<BetCancel> messageContext);
	}
}