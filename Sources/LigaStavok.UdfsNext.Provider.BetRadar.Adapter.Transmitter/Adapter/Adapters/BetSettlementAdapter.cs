using System.Collections.Generic;
using System.Collections.Immutable;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Converters;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public class BetSettlementAdapter : IBetSettlementAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<BetSettlement> messageContext)
		{
			var message = messageContext.Message;
			var lineService = message.Product.ToLineService();

			yield return new CreateResultsCommand
			(
				lineService: lineService,
				gameEventId: message.EventId.ToTransmitterEventId(),
				receivedOn: messageContext.ReceivedOn,
				incomingId: messageContext.IncomingId,
				selections: CreateResultsCommandsForMarkets(message, lineService).ToImmutableArray(),
				extraAttributes: null
			);
		}

		private static IEnumerable<CreateResultsCommandSelection> CreateResultsCommandsForMarkets(
			BetSettlement message,
			LineService lineService
		)
		{
			foreach (var market in message.Markets)
			{
				var marketUniqueId = market.GetUniqueId(lineService, message.EventId);

				foreach (var outcome in market.Outcomes)
				{
					yield return CreateResultsCommandForOutcome(outcome, market, marketUniqueId);
				}
			}
		}

		private static CreateResultsCommandSelection CreateResultsCommandForOutcome(BetSettlementOutcome outcome,
			BetSettlementMarket market,
			string marketUniqueId)
		{
			var voidFactor = outcome.VoidFactor.HasValue
				? System.Convert.ToDecimal(outcome.VoidFactor.Value)
				: default(decimal?);

			var deadHeatFactor = outcome.DeadHeatFactor.HasValue
				? System.Convert.ToDecimal(outcome.DeadHeatFactor.Value)
				: default(decimal?);

			ResultType resultType;

			if (voidFactor.HasValue && voidFactor.Value != 0)
			{
				resultType = voidFactor == 1
					? ResultType.Refund
					: ResultType.PartialRefund;
			}
			else
			{
				resultType = outcome.Result == 1
					? ResultType.Win
					: ResultType.Loss;
			}

			var specifiers = market.ExtractSpecifiers();

			return new CreateResultsCommandSelection
			(
				marketId: marketUniqueId,
				marketTypeId: market.Id.ToString(),
				specifiers: specifiers,
				selectionId: outcome.GetUniqueId(marketUniqueId),
				selectionTypeId: outcome.Id,
				resultType: resultType,
				voidFactor: voidFactor,
				deadHeatFactor: deadHeatFactor,
				voidReason: CodesConverter.GetVoidReasonByReasonId(market.VoidReason)
			);
		}
	}
}