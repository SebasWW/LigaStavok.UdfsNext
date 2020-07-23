using System.Collections.Generic;
using System.Collections.Immutable;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public class BetCancelRollbackAdapter : IBetCancelRollbackAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<BetCancelRollback> messageContext)
		{

			var lineService = messageContext.Message.Product.ToLineService();

			yield return new UndoCancelBetsCommand
			(
				receivedOn: messageContext.ReceivedOn,
				incomingId: messageContext.IncomingId,
				gameEventId: messageContext.Message.EventId.ToTransmitterEventId(),
				lineService: lineService,
				undoFrom: null,
				undoTo: null,
				markets: GetMarkets().ToImmutableArray(),
				extraAttributes: null
			);

			IEnumerable<UndoCancelBetsCommandMarket> GetMarkets()
			{
				foreach (var market in messageContext.Message.Markets)
				{
					var specifiers = market.ExtractSpecifiers();

					yield return new UndoCancelBetsCommandMarket
					(
						marketId: market.GetUniqueId(lineService, messageContext.Message.EventId),
						marketTypeId: market.Id.ToString(),
						specifiers: specifiers
					);
				}
			}
		}
	}
}