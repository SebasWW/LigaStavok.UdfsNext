using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Configuration.Messages;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Converters;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Microsoft.Extensions.Logging;
using Udfs.Transmitter.DSL.GameEventStateDescription;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public class OddsChangeAdapter : IOddsChangeAdapter
	{
		private readonly ILogger<OddsChangeAdapter> logger;

		public OddsChangeAdapter(ILogger<OddsChangeAdapter> logger)
		{
			this.logger = logger;
		}

		public IEnumerable<ITransmitterCommand> Adapt(
			MessageContext<OddsChange> messageContext,
			LineService lineService,
			OddsChangeConfiguration config
		)
		{
			var message = messageContext.Message;

			// Ttl check
			if (config.TtlEnabled && config.Ttl < TimeSpan.FromMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - message.Timestamp))
			{
				logger.LogWarning($"OddsChangeMessageExpired EventId={message.EventId} Pooduct={message.Product} IncomingId={messageContext.IncomingId}. StopGameEventBetsCommand command is executed.");

				yield return new StopGameEventBetsCommand(
					lineService,
					message.GetPureEventId(),
					messageContext.ReceivedOn,
					messageContext.IncomingId,
					BetStopReason.UdfsFailure,
					ImmutableDictionary.Create<string, string>()
				);
				yield break;
			}

			// Adapt to CreateUpdateMarketsCommand
			foreach (var item in ConstructCreateUpdateMarketsCommand(messageContext, config))
			{
				yield return item;
			}

			// Adapt to UpdateGameEventStateCommand
			if (message.SportEventStatus != null)
				yield return ConstructUpdateGameEventStateCommand(messageContext, lineService);
		}

		private static IEnumerable<CreateUpdateMarketsCommand> ConstructCreateUpdateMarketsCommand(MessageContext<OddsChange> messageContext, OddsChangeConfiguration config)
		{
			var message = messageContext.Message;
			var lineService = message.Product.ToLineService();
			var marketsBuilder = new List<CreateUpdateMarketsCommandSelection>();

			foreach (var market in message.Markets.Where(x => x.Outcomes.Any() == false))
			{
				var specifiers = market.ExtractSpecifiers();

				marketsBuilder.Add
				(
					new CreateUpdateMarketsCommandSelection
					(
						marketId: market.GetUniqueId(lineService, message.EventId),
						selectionId: "*",
						marketTypeId: market.Id.ToString(),
						specifiers: specifiers,
						selectionTypeId: "*",
						tradingStatus: market.Status.ToMarketTradingStatus(),
						selectionTradingStatus: null,
						value: null,
						probability: null
					)
				);
			}

			foreach (var market in message.Markets.Where(x => !x.Outcomes.Any() == false))
			{
				var marketUniqueId = market.GetUniqueId(lineService, message.EventId);

				foreach (var outcome in market.Outcomes)
				{
					var specifiers = market.ExtractSpecifiers();

					var probability = outcome.Probabilities.HasValue
						? Convert.ToDecimal(outcome.Probabilities.Value)
						: default(decimal?);

					marketsBuilder.Add
					(
						new CreateUpdateMarketsCommandSelection
						(
							marketId: market.GetUniqueId(lineService, message.EventId),
							selectionId: outcome.GetUniqueId(marketUniqueId),
							marketTypeId: market.Id.ToString(),
							specifiers: specifiers,
							selectionTypeId: outcome.Id,
							tradingStatus: (outcome.Active && (market.Status == OddsChangeMarketStatus.Active)) ? TradingStatus.Open : TradingStatus.Suspended,
							selectionTradingStatus: outcome.Active ? TradingStatus.Open : TradingStatus.Closed,
							value: Convert.ToDecimal(outcome.Odds),
							probability: probability
						)
					);
				}

				// Max markets in command
				if (marketsBuilder.Count >= config.MarketCountMax)
				{
					yield return new CreateUpdateMarketsCommand(
						lineService: lineService,
						gameEventId: message.EventId.ToTransmitterEventId(),
						receivedOn: messageContext.ReceivedOn,
						incomingId: messageContext.IncomingId,
						selections: marketsBuilder.ToImmutableArray(),
						extraAttributes: null
					);

					marketsBuilder.Clear();
				}
			}

			// Markets are left or there are no markets at all
			if (marketsBuilder.Count > 0 || message.Markets.Any() == false)
			{
				yield return new CreateUpdateMarketsCommand(
					lineService: lineService,
					gameEventId: message.EventId.ToTransmitterEventId(),
					receivedOn: messageContext.ReceivedOn,
					incomingId: messageContext.IncomingId,
					selections: marketsBuilder.ToImmutableArray(),
					extraAttributes: null
				);
			}
		}

		private static UpdateGameEventStateCommand ConstructUpdateGameEventStateCommand(MessageContext<OddsChange> messageContext, LineService lineService)
		{
			var message = messageContext.Message;
			var status = message.SportEventStatus;

			return new UpdateGameEventStateCommand
			(
				lineService: lineService,
				gameEventId: message.EventId.ToTransmitterEventId(),
				receivedOn: messageContext.ReceivedOn,
				incomingId: messageContext.IncomingId,
				description: GetDescription()
			);

			UpdateGameEventStateCommandDescription GetDescription()
			{
				var builder = new UpdateGameEventStateCommandDescription.Builder();

				// Status
				if (lineService != LineService.BetradarUnifiedFeedLCoO)
					builder.WithStatus(MatchStatusConverter.Convert(status.MatchStatus));

				// ScoresTotal
				if (status.HomeScore.HasValue || status.AwayScore.HasValue)
				{
					var home = status.HomeScore ?? 0;
					var away = status.AwayScore ?? 0;

					builder.WithScoresTotal
					(
						home.ToString(CultureInfo.InvariantCulture),
						away.ToString(CultureInfo.InvariantCulture)
					);
				}

				//scoresGame
				if (status.HomeGameScore.HasValue || status.AwayGameScore.HasValue)
				{
					var home = status.HomeGameScore ?? 0;
					var away = status.AwayGameScore ?? 0;

					builder.WithSubject(TransmitterCommandDescriptionKeys.ScoresGame, new GameEventStateScore
					(
						home.ToString(CultureInfo.InvariantCulture),
						away.ToString(CultureInfo.InvariantCulture)
					));
				}

				// ScoresByParts
				if (status.PeriodScores.Any())
				{
					var scoreLine = CalcScoreLine(status);
					builder.WithScoresByParts(scoreLine);
				}

				// MatchTime
				if (status.Clock?.MatchTime != null)
				{
					var matchTime = status.Clock.MatchTime.ParseMinutesFrom(Rounding.AwayFromZero);
					builder.WithSubject(TransmitterCommandDescriptionKeys.MatchTime, $"{matchTime}");
				}


				// RemainingTime
				if (status.Clock?.RemainingTime != null)
				{
					var remainingTime = status.Clock.RemainingTime.ParseMinutesFrom(Rounding.ToZero);
					builder.WithSubject(TransmitterCommandDescriptionKeys.Timer_Remaining, $"{remainingTime}");
				}


				// Cards.Red
				if (status.Statistics != null)
				{
					var home = message.SportEventStatus.Statistics.HomeRedCards + message.SportEventStatus.Statistics.HomeYellowRedCards;
					var away = message.SportEventStatus.Statistics.AwayRedCards + message.SportEventStatus.Statistics.AwayYellowRedCards;

					if ((home | away) > 0)
						builder.WithCards(GameEventStateCardType.Red, new GameEventStateScore(home, away));
				}

				// Server
				if (status.CurrentServer.HasValue)
					builder.WithSubject(TransmitterCommandDescriptionKeys.Server, status.CurrentServer.Value);

				return builder.ToImmutable();
			}
		}

		private static GameEventStateScoreLine CalcScoreLine(OddsChangeEventStatus status)
		{
			var scores = status.PeriodScores
				.OrderBy(t => t.Number)
				.Select(x =>
					new GameEventStateScore
					(
						x.HomeScore.ToString(CultureInfo.InvariantCulture),
						x.AwayScore.ToString(CultureInfo.InvariantCulture)
					)
				);

			if (status.HomePenaltyScore.HasValue || status.AwayPenaltyScore.HasValue)
				scores = scores.Union(
					new GameEventStateScore[]
					{
						new GameEventStateScore(
							status.HomePenaltyScore.Value.ToString(CultureInfo.InvariantCulture),
							status.AwayPenaltyScore.Value.ToString(CultureInfo.InvariantCulture)
						)
					}
				);

			return new GameEventStateScoreLine(scores);
		}
	}
}