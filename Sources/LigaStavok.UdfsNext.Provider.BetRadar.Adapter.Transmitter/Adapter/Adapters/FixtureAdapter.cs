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
	public class FixtureAdapter : IFixtureAdapter
	{
		private readonly ILogger<FixtureAdapter> logger;

		public FixtureAdapter(ILogger<FixtureAdapter> logger)
		{
			this.logger = logger;
		}

		public IEnumerable<ITransmitterCommand> Adapt(
			MessageContext<Fixture> messageContext,
			Language language,
			LineService lineService
		)
		{
			var message = messageContext.Message;

			var sport = SportIdToSportConverter.Convert(message.Tournament.Sport.Id);
			if (sport == null)
			{
				logger.LogWarning($"Unknown sportId={message.Tournament.Sport.Id} in FixtureMessage Id={message.Id} Liveodds={message.Liveodds}");
				yield break;
			}

			if (language == Language.English)
			{
				ImmutableDictionary<string, string> extraAttributes = null;

				if (message.Liveodds != null)
				{
					var dic = new Dictionary<string, string>
					{
						{ "Booked", message.Liveodds == "booked" ? "1" : "0" }
					};

					var keys = new string[] { "period_length", "overtime_length", "best_of", "best_of_legs", "set_limit" };

					foreach (var key in keys)
						if (message.ExtraInfo.TryGetValue(key, out var value))
							dic.Add(key, value);

					extraAttributes = dic.ToImmutableDictionary();
				}

				yield return new CreateUpdateGameEventMetaCommand
				(
					lineService: lineService,
					gameEventId: message.Id.ToTransmitterEventId(),
					receivedOn: messageContext.ReceivedOn,
					incomingId: messageContext.IncomingId,
					gameEventType: GameEventType.Match,
					gameEventStartDateUtc: message.StartTime,
					sport: sport,
					categoryId: message.Tournament.Category.Id,
					topicId: message.Tournament.Id,
					competitors: GetCompetitors(message),
					extraAttributes: extraAttributes
				);
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

		private ImmutableArray<Competitor> GetCompetitors(Fixture message)
		{
			return message.Competitors.Select(x =>

				new Competitor
				(
					id: x.Id,
					type: CompetitorType.Team,
					status: CompetitorStatus.Active,
					specifier: new SpecifierCollection(new Specifier(SpecifierKey.Side, x.Qualifier))
				)

			).ToImmutableArray();
		}

		private ImmutableArray<AddTranslationsCommandTranslationItem> GetTranslations(Fixture message, Language language)
		{
			var translations = new[]
			{
				GetTranslationsForCompetitorsNames(message.Competitors, language),
				GetTranslationForTournamentName(message.Tournament, language),
				GetTranslationForCategoriesName(message.Tournament?.Category, language)
			};

			return translations.SelectMany(x => x)
				.GroupBy(x => new { x.Language, x.ObjectId, x.ObjectType })
				.Select(x => x.First())
				.ToImmutableArray();
		}

		private IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationsForCompetitorsNames(IEnumerable<Team> competitors, Language language)
		{
			foreach (var competitor in competitors)
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

		private IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationForTournamentName(Tournament tournament, Language language)
		{
			if (tournament == null)
			{
				yield break;
			}

			yield return new AddTranslationsCommandTranslationItem
			(
				language: language,
				objectType: TranslationObjectType.TopicName,
				objectId: tournament.Id,
				value: tournament.Name
			);
		}

		private IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationForCategoriesName(Category category, Language language)
		{
			if (category == null)
			{
				yield break;
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