using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Converters;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;
using Microsoft.Extensions.Logging;
using Udfs.Common.Primitives;
using Udfs.Transmitter.DSL.Competitor;
using Udfs.Transmitter.DSL.SpecifierDescription;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public class ScheduleAdapter : IScheduleAdapter
	{
		private readonly ILogger<ScheduleAdapter> logger;

		public ScheduleAdapter(ILogger<ScheduleAdapter> logger)
		{
			this.logger = logger;
		}

		public IEnumerable<ITransmitterCommand> Adapt(
			MessageContext<Schedule> messageContext,
			Language language,
			LineService lineService
		)
		{
			var message = messageContext.Message;

			if (language == Language.English)
			{
				foreach (var evnt in message.SportEvents.Where(x => x.Tournament != null))
				{
					if (evnt.Competitors.Count == 2 &&
						evnt.Competitors[0].Qualifier == evnt.Competitors[1].Qualifier)
					{
						continue;
					}

					var sport = SportIdToSportConverter.Convert(evnt.Tournament.Sport.Id);

					if (sport == null)
					{
						logger.LogWarning(
							$"Unknown sportId={evnt.Tournament.Sport.Id} in ScheduleMessage. EventId={evnt.Id}");
						continue;
					}

					yield return new CreateUpdateGameEventMetaCommand(
						lineService: lineService,
						gameEventId: evnt.Id.ToTransmitterEventId(),
						receivedOn: messageContext.ReceivedOn,
						incomingId: messageContext.IncomingId,
						gameEventType: GameEventType.Match,
						gameEventStartDateUtc: evnt.Scheduled,
						sport: sport,
						categoryId: evnt.Tournament.Category.Id,
						topicId: evnt.Tournament.Id,
						competitors: GetCompetitors(evnt),
						extraAttributes: null
					);
				}
			}

			var translations = GetTranslations(message, language);

			if (translations.IsEmpty)
				yield break;

			yield return new AddTranslationsCommand
			(
				lineService: lineService,
				receivedOn: messageContext.ReceivedOn,
				incomingId: messageContext.IncomingId,
				translations: translations
			);
		}

		private ImmutableArray<Competitor> GetCompetitors(SportEvent sportEvent)
		{
			return sportEvent.Competitors.Select(x =>
				new Competitor
				(
					id: x.Id,
					type: CompetitorType.Team,
					status: CompetitorStatus.Active,
					specifier: new SpecifierCollection(new Specifier(SpecifierKey.Side, x.Qualifier))
				)
			).ToImmutableArray();
		}

		private ImmutableArray<AddTranslationsCommandTranslationItem> GetTranslations(
			Schedule message,
			Language language)
		{
			var translations = new[]
			{
				GetTranslationsForCompetitorsNames(message.SportEvents, language),
				GetTranslationsForTournamentsNames(message.SportEvents, language),
				GetTranslationsForCategoriesNames(message.SportEvents, language)
			};

			return translations.SelectMany(x => x)
				.GroupBy(x => new { x.Language, x.ObjectId, x.ObjectType })
				.Select(x => x.First())
				.ToImmutableArray();
		}

		private IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationsForCompetitorsNames(
			IEnumerable<SportEvent> events, Language language)
		{
			foreach (var competitor in events.SelectMany(x => x.Competitors))
			{
				yield return new AddTranslationsCommandTranslationItem
				(
					language: language,
					objectType: TranslationObjectType.CompetitorName,
					objectId: competitor.Id,
					value: competitor.Name
				);
			}
		}

		private IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationsForTournamentsNames(
			IEnumerable<SportEvent> events, Language language)
		{
			foreach (var tournament in events.Select(x => x.Tournament))
			{
				if (tournament == null)
				{
					continue;
				}

				yield return new AddTranslationsCommandTranslationItem
				(
					language: language,
					objectType: TranslationObjectType.TopicName,
					objectId: tournament.Id,
					value: tournament.Name
				);
			}
		}

		private IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationsForCategoriesNames(
			IEnumerable<SportEvent> events,
			Language language
		)
		{
			foreach (var category in events.Select(x => x.Tournament?.Category))
			{
				if (category == null)
				{
					continue;
				}

				yield return new AddTranslationsCommandTranslationItem
				(
					language: language,
					objectType: TranslationObjectType.CategoryName,
					objectId: category.Id,
					value: category.Name
				);
			}
		}
	}
}