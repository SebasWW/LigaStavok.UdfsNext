using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public class BetStopAdapter : IBetStopAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<BetStop> messageContext)
		{
			var message = messageContext.Message;

			yield return new StopGameEventBetsCommand
			(
				lineService: message.Product.ToLineService(),
				gameEventId: message.EventId.ToTransmitterEventId(),
				receivedOn: messageContext.ReceivedOn,
				incomingId: messageContext.IncomingId,
				reason: BetStopReason.FeedCommand,
				extraAttributes: null
			);
		}
	}
}