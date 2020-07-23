using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public class AliveAdapter : IAliveAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<Alive> messageContext)
		{
			yield return new KeepAliveCommand
			(
				lineService: messageContext.Message.Product.ToLineService(),
				receivedOn: messageContext.ReceivedOn,
				incomingId: messageContext.IncomingId
			);
		}
	}
}