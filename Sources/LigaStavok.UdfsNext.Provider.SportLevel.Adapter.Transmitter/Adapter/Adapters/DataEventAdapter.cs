using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Converters;
using LigaStavok.UdfsNext.Provider.SportLevel.State;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Udfs.Transmitter.DSL.GameEventStateDescription;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public class DataEventAdapter : IDataEventAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<EventData, TranslationSubscription> context)
		{
			var list = new List<ITransmitterCommand>();
			var translationState = context.State.PersistableState;
			var msg = context.Message;

			list.Add(
				new UpdateGameEventStateCommand(
					lineService: LineService.SportLevel,
					gameEventId: msg.TranslationId,
					receivedOn: context.ReceivedOn,
					incomingId: context.IncomingId,
					description: GetDescription()
				)
			);

			context.State.SaveState();
			return list;

			UpdateGameEventStateCommandDescription GetDescription()
			{
				var builder = new UpdateGameEventStateCommandDescription.Builder();

				// Status
				var status = MatchStatusConverter.Convert(msg.EventCode);

				if (status != null)
					builder.WithStatus(status);

				else if (msg.GamePeriod.HasValue && msg.GamePeriod.Value > 0) //msg.EventCode == EventCode.START_PERIOD) 
					builder.WithStatus(new SportLevelGameEventStatus(msg.GamePeriod.Value + "set"));

				// Time
				if (msg.Playtime > 0)
					builder.WithSubject("matchtime", Convert.ToInt32(TimeSpan.FromMilliseconds(msg.Playtime).TotalMinutes) + 1);

				// Score
				if (msg.ScoreHome.HasValue || msg.ScoreAway.HasValue)
				{
					var home = msg.ScoreHome ?? 0;
					var away = msg.ScoreAway ?? 0;

					builder.WithScoresTotal
					(
						home.ToString(CultureInfo.InvariantCulture),
						away.ToString(CultureInfo.InvariantCulture)
					);
				}

				// Score part
				if (msg.Extra != null && msg.GamePeriod.HasValue && msg.GamePeriod.Value > 0)
					if (msg.Extra.Points1.HasValue || msg.Extra.Points2.HasValue)
					{
						// fix previos states
						for (var i = 1; i < msg.GamePeriod; i++)
						{
							translationState.MatchScore.GetOrAdd(i, y => new Score());
						}

						// save current score to state
						translationState.MatchScore.AddOrUpdate(
							msg.GamePeriod.Value,
							id => new Score() { Home = msg.Extra.Points1 ?? 0, Away = msg.Extra.Points2 ?? 0 },
							(id, score) =>
							{
								score.Home = msg.Extra.Points1 ?? 0;
								score.Away = msg.Extra.Points2 ?? 0;
								return score;
							}
						);

						var scoreLine = new GameEventStateScoreLine(
							translationState.MatchScore
								.OrderBy(entry => entry.Key)
								.Select(
									entry =>
									new GameEventStateScore(entry.Value.Home, entry.Value.Away)
								)
						);
						builder.WithScoresByParts(scoreLine);



					}

				switch (msg.EventCode)
				{
					case EventCode.POSESSION_1:
						builder.WithSubject(GameEventStateDescriptionSubject.Server, "1");
						break;

					case EventCode.POSESSION_2:
						builder.WithSubject(GameEventStateDescriptionSubject.Server, "2");
						break;
				}
				return builder.ToImmutable();
			}
		}
	}
}
