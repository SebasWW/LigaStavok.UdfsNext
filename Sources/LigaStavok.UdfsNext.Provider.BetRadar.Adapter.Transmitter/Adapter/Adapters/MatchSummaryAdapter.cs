using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Converters;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;
using Udfs.Common.Primitives;
using Udfs.Transmitter.DSL.GameEventStateDescription;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public class MatchSummaryAdapter : IMatchSummaryAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(
			MessageContext<MatchSummary> messageContext,
			Language language,
			LineService lineService
		)
		{
			var message = messageContext.Message;

			var status = message.SportEventStatus;
			if (status != null)
			{
				yield return new UpdateGameEventStateCommand
				(
					lineService: lineService,
					gameEventId: message.SportEvent.Id.ToTransmitterEventId(),
					receivedOn: messageContext.ReceivedOn,
					incomingId: messageContext.IncomingId,
					description: GetDescription()
				);

				UpdateGameEventStateCommandDescription GetDescription()
				{
					var builder = new UpdateGameEventStateCommandDescription.Builder();

					// Status
					if (status.MatchStatusCode.HasValue && lineService != LineService.BetradarUnifiedFeedLCoO)
						builder.WithStatus(MatchStatusConverter.Convert(status.MatchStatusCode.Value));

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

					if (status.PeriodScores.Any())
					{
						var scoreLine = CalcScoreLine(status);
						builder.WithScoresByParts(scoreLine);
					}

					if (status.Clock != null)
					{
						int ParseTime(string s)
						{
							var fractions = s.Split(':');
							var seconds = 0;
							var multiplier = 1;

							for (var i = fractions.Length - 1; i >= 0; i--)
							{
								seconds = int.Parse(fractions[i]) * multiplier;

								multiplier = multiplier * 60;
							}

							return seconds;
						}

						if (!string.IsNullOrEmpty(status.Clock.MatchTime))
						{
							builder.WithElapsedTime(
								ParseTime(status.Clock.MatchTime));
						}
					}

					return builder.ToImmutable();
				}
			}
		}

		private static GameEventStateScoreLine CalcScoreLine(SportEventStatus status)
		{
			var scores = status.PeriodScores
				.OrderBy(GetPeriodScorePosition)
				.Select(x =>
					new GameEventStateScore
					(
						x.HomeScore.ToString(CultureInfo.InvariantCulture),
						x.AwayScore.ToString(CultureInfo.InvariantCulture)
					)
				);

			return new GameEventStateScoreLine(scores);
		}

		private static int GetPeriodScorePosition(PeriodScore periodScore)
		{
			switch (periodScore.Type)
			{
				case PeriodScoreType.RegularPeriod: return periodScore.Number.GetValueOrDefault(0);
				case PeriodScoreType.Overtime: return 1000 + periodScore.Number.GetValueOrDefault(0);
				case PeriodScoreType.Penalties: return 2000 + periodScore.Number.GetValueOrDefault(0);
				default: return 3000 + periodScore.Number.GetValueOrDefault(0);
			}
		}
	}
}