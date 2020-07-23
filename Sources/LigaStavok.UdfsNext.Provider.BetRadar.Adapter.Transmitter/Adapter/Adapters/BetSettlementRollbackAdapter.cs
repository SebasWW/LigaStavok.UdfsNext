using System.Collections.Generic;
using System.Collections.Immutable;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public class BetSettlementRollbackAdapter : IBetSettlementRollbackAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<BetSettlementRollback> messageContext)
		{
			var message = messageContext.Message;
			var lineService = message.Product.ToLineService();

			yield return new RollbackResultsCommand
			(
				receivedOn: messageContext.ReceivedOn,
				incomingId: messageContext.IncomingId,
				gameEventId: message.EventId.ToTransmitterEventId(),
				lineService: lineService,
				markets: GetMarkets().ToImmutableArray(),
				extraAttributes: null
			);

			IEnumerable<RollbackResultsCommandMarket> GetMarkets()
			{
				foreach (var market in message.Markets)
				{
					var specifiers = market.ExtractSpecifiers();

					yield return new RollbackResultsCommandMarket
					(
						marketId: market.GetUniqueId(lineService, message.EventId),
						marketTypeId: market.Id.ToString(),
						specifiers: specifiers
					);
				}
			}
		}
	}
}