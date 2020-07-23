using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;
using Udfs.Common.Primitives;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public class MarketDescriptionListAdapter : IMarketDescriptionListAdapter
	{
		public IEnumerable<ITransmitterCommand> Adapt(
			MessageContext<MarketDescriptionList> messageContext,
			Language language,
			LineService lineService
		)
		{
			var message = messageContext.Message;
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

		private static ImmutableArray<AddTranslationsCommandTranslationItem> GetTranslations(
			MarketDescriptionList message, Language language)
		{
			var marketDescriptions = message.Markets.Where(x => !string.IsNullOrEmpty(x.Variant)).ToList();
			var outcomeDescriptions = marketDescriptions.SelectMany(x => x.Outcomes);

			var translations = new[]
			{
				GetTranslationsForMarkets(marketDescriptions, language),
				GetTranslationsForOutcomes(outcomeDescriptions, language)
			};

			return translations.SelectMany(x => x).ToImmutableArray();
		}

		private static IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationsForMarkets(
			IEnumerable<MarketDescription> descriptions, Language language)
		{
			foreach (var description in descriptions.Where(x => !x.Variant.StartsWith("sr:")))
			{
				yield return new AddTranslationsCommandTranslationItem
				(
					language: language,
					objectType: TranslationObjectType.MarketName,
					objectId: description.Variant,
					value: description.Name
				);
			}
		}

		private static IEnumerable<AddTranslationsCommandTranslationItem> GetTranslationsForOutcomes(
			IEnumerable<MarketDescriptionOutcome> descriptions, Language language)
		{
			foreach (var description in descriptions)
			{
				yield return new AddTranslationsCommandTranslationItem
				(
					language: language,
					objectType: TranslationObjectType.OutcomeName,
					objectId: description.Id,
					value: description.Name
				);
			}
		}
	}
}