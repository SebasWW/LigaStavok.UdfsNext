using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Converters;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public class TranslationAdapter : ITranslationAdapter
	{

		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<Translation> context)
		{
			yield return new CreateUpdateGameEventMetaCommand
			(
				lineService: LineService.SportLevel,
				gameEventId: context.Message.Id.ToString(),
				receivedOn: context.ReceivedOn,
				incomingId: context.IncomingId,
				gameEventType: GameEventType.Match,
				gameEventStartDateUtc: context.Message.StartIso8601,
				sport: SportConverter.Convert(context.Message.SportId),
				categoryId: context.Message.TournamentCountryTitle ?? context.Message.TournamentTitle,
				topicId: context.Message.TournamentId.ToString(),
				competitors: context.Message.GetCompetitors(),
				extraAttributes: null
			);

			var translations = GetTranslations(context, Language.English);

			if (!translations.IsEmpty)
				yield return new AddTranslationsCommand
				(
					lineService: LineService.SportLevel,
					receivedOn: context.ReceivedOn,
					incomingId: context.IncomingId,
					translations: translations
				);
		}

		private ImmutableArray<AddTranslationsCommandTranslationItem> GetTranslations(MessageContext<Translation> context, Language language)
		{
			var translations = new[]
			{
				GetTranslationsForCompetitorsNames(context.Message, language),
				GetTranslationForTournamentName(context.Message, language),
				GetTranslationForCategoriesName(context.Message, language)
			};

			return translations.SelectMany(x => x)
				.GroupBy(x => new { x.Language, x.ObjectId, x.ObjectType })
				.Select(x => x.First())
				.ToImmutableArray();
		}

		private IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationsForCompetitorsNames(Translation msg, Language language)
		{

			yield return new AddTranslationsCommandTranslationItem
			(
				language: language,
				objectType: TranslationObjectType.CompetitorName,
				objectId: msg.HomeTeamId.ToString(),
				value: msg.HomeTeamTitleEn ?? msg.HomeTeamShortTitleEn
			);

			yield return new AddTranslationsCommandTranslationItem
				(
					language: language,
					objectType: TranslationObjectType.CompetitorName,
					objectId: msg.AwayTeamId.ToString(),
					value: msg.AwayTeamTitleEn ?? msg.AwayTeamShortTitleEn
				);

		}

		private IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationForTournamentName(Translation msg, Language language)
		{
			if ((msg.TournamentTitleEn ?? msg.TournamentShortTitleEn) == null)
			{
				yield break;
			}

			yield return new AddTranslationsCommandTranslationItem
			(
				language: language,
				objectType: TranslationObjectType.TopicName,
				objectId: msg.TournamentId.ToString(),
				value: msg.TournamentTitleEn ?? msg.TournamentShortTitleEn
			);
		}

		private IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationForCategoriesName(Translation msg, Language language)
		{
			if (msg.TournamentCountryTitleEn == null)
			{
				yield break;
			}

			yield return new AddTranslationsCommandTranslationItem
			(
				language: language,
				objectType: TranslationObjectType.CategoryName,
				objectId: msg.TournamentCountryCode,
				value: msg.TournamentCountryTitleEn
			);
		}

		private ImmutableArray<Competitor> GetCompetitors(this Translation message)
		{
			return new Competitor[] {

				new Competitor
				(
					id: message.HomeTeamId.ToString(),
					type: CompetitorType.Team,
					status: CompetitorStatus.Active,
					specifier: new SpecifierCollection(new Specifier(SpecifierKey.Side, "home"))
				),
				new Competitor
				(
					id: message.AwayTeamId.ToString(),
					type: CompetitorType.Team,
					status: CompetitorStatus.Active,
					specifier: new SpecifierCollection(new Specifier(SpecifierKey.Side, "away"))
				)

			}.ToImmutableArray();
		}

	}
}