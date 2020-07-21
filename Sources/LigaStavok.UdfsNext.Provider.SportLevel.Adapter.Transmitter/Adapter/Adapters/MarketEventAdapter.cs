using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using LigaStavok.UdfsNext.Provider.SportLevel.Configuration;
using LigaStavok.UdfsNext.Provider.SportLevel.State;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Microsoft.Extensions.Logging;
using Udfs.Transmitter.DSL.SpecifierDescription;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public class MarketEventAdapter : IMarketEventAdapter
	{
		private readonly AdapterConfiguration adapterConfiguration;
		private readonly ILogger<MarketEventAdapter> logger;
		private readonly Dictionary<int, int> marketGroups;

		public MarketEventAdapter(
			AdapterConfiguration adapterConfiguration,
			ILogger<MarketEventAdapter> logger
		)
		{
			this.adapterConfiguration = adapterConfiguration;
			this.logger = logger;

			var arr = adapterConfiguration.OutcomeMapping
				.SelectMany(t => t.Values.Select(y => new { key = y, value = t.GroupId })).ToArray();

			// Checking
			var badItem = arr.GroupBy(t => t.key).FirstOrDefault(t => t.Count() > 1)?.ToArray();
			if (badItem != null)
			{
				throw new Exception($"Invalid configuration is detected. OutcomeMapping parameter has dublicated id {badItem[0].key} in groups {badItem[0].value} and {badItem[1].value}.");
			}

			marketGroups = arr.ToDictionary(k => k.key, v => v.value);
		}

		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<EventData, TranslationSubscription> context)
		{
			var msg = context.Message;
			var translationState = context.State.PersistableState;
			var translationId = long.Parse(msg.TranslationId);
			var lineService = LineService.SportLevel;

			var marketsBuilder = new List<CreateUpdateMarketsCommandSelection>();
			var list = new List<ITransmitterCommand>();

			var currentMarkets = new Dictionary<string, TranslationMarket>();

			foreach (var market in msg.Extra.Markets)
			{
				var fakeMarketId = $"{lineService}:{msg.TranslationId}:{market.Id}";

				var specifierKey = adapterConfiguration.Specifiers.Total.Contains(market.Id) ? SpecifierKey.Total
					: adapterConfiguration.Specifiers.Hcp.Contains(market.Id) ? SpecifierKey.Handicap
					: adapterConfiguration.Specifiers.Pointnr.Contains(market.Id) ? SpecifierKey.PointNumber
					: null;

				// Outcomes
				if (market.Selections.Any())
				{
					var arr = market.Selections.ToArray();

					// Not categorized market with specifiers
					if (arr.Any(t => t[2].HasValue) && specifierKey == null)
					{
						logger.LogWarning($"Uncategorized market with specifiers was detected. MessageId={context.IncomingId}, marketId={market.Id}");

						continue; // to next market
					}

					Specifier hcpOldSpecifier = null;

					for (int i = 0; i < arr.Count(); i++)
					{
						Specifier specifier = null;
						var item = arr[i];

						if (specifierKey != null)
						{
							if (i % 2 == 1 && specifierKey == SpecifierKey.Handicap)
								specifier = hcpOldSpecifier;
							else
								specifier = item[2].HasValue ? GetSpecifier(specifierKey, item[2].Value) : null;
						}

						// Creating markets
						AddFakeMarkets(
							marketsBuilder,
							specifier,
							item,
							currentMarkets,
							fakeMarketId,
							market.Id
						);

						hcpOldSpecifier = specifier;
					}
				}
			}

			// Markets to state
			foreach (var item in currentMarkets)
			{
				translationState.Markets.GetOrAdd(item.Key, item.Value);
			}

			lock (translationState.Markets)
			{
				// Susupensing markets which are left
				foreach (var fakeId in translationState.Markets.Keys.Except(currentMarkets.Select(t => t.Key)))
				{
					var entry = translationState.Markets[fakeId];

					marketsBuilder.Add(
						new CreateUpdateMarketsCommandSelection
						(
							marketId: entry.FakeId,
							marketTypeId: entry.Id.ToString(),
							selectionId: fakeId,
							specifiers: entry.SpecifierValue == null
								? null
								: new SpecifierCollection(GetSpecifier(entry.Id, translationState.Markets[fakeId].SpecifierValue, adapterConfiguration)),
							selectionTypeId: entry.SelectionId.ToString(),
							tradingStatus: TradingStatus.Suspended,
							selectionTradingStatus: TradingStatus.Suspended,
							value: null,
							probability: null
						)
					);
				}
			}

			list.Add(
				new CreateUpdateMarketsCommand(
					lineService: lineService,
					gameEventId: msg.TranslationId.ToString(),
					receivedOn: context.ReceivedOn,
					incomingId: context.IncomingId,
					selections: marketsBuilder.ToImmutableArray(),
					extraAttributes: null
				)
			);

			context.State.SaveState();
			return list;
		}

		private void AddFakeMarkets(
			List<CreateUpdateMarketsCommandSelection> marketsBuilder,
			Specifier specifier,
			IList<decimal?> selection,
			Dictionary<string, TranslationMarket> currentMarkets,
			string fakeMarketId,
			long originalMarketId
		)
		{
			if (!marketGroups.TryGetValue(Convert.ToInt32(selection[0]), out var selectionTypeId)) // index 0 - outcome_id
				selectionTypeId = Convert.ToInt32(selection[0]);

			var specifiers = specifier == null ? null : new SpecifierCollection(specifier);

			var currentMarketId = fakeMarketId + (specifier == null ? "" : ":" + specifier.Key + "=" + specifier.Value);
			var currentSelectionId = fakeMarketId + ":" + selectionTypeId + (specifier == null ? "" : ":" + specifier.Key + "=" + specifier.Value);


			// adding market to cache
			currentMarkets.TryAdd(
				currentSelectionId,
				new TranslationMarket()
				{
					Id = originalMarketId,
					FakeId = currentMarketId,
					SelectionId = selectionTypeId,
					SpecifierValue = specifier?.Value
				}
			);

			marketsBuilder.Add
			(
				new CreateUpdateMarketsCommandSelection
				(
					marketId: currentMarketId,
					marketTypeId: originalMarketId.ToString(),
					selectionId: currentSelectionId,

					specifiers: specifiers,
					selectionTypeId: selectionTypeId.ToString(),
					tradingStatus: TradingStatus.Open,
					selectionTradingStatus: TradingStatus.Open,
					value: Round2(selection[1]),
					probability: null
				)
			);
		}

		private static Specifier GetSpecifier(SpecifierKey specifierKey, decimal index2Value)
		{
			return new Specifier(
				specifierKey,
				(index2Value / 100).ToString(CultureInfo.InvariantCulture)
			);
		}

		private static Specifier GetSpecifier(long marketId, string stringValue, AdapterConfiguration adapterConfiguration)
		{
			return new Specifier(
				adapterConfiguration.Specifiers.Total.Contains(marketId) ? SpecifierKey.Total
				: adapterConfiguration.Specifiers.Pointnr.Contains(marketId) ? SpecifierKey.PointNumber : SpecifierKey.Handicap,
				stringValue
			);
		}

		private static decimal? Round2(decimal? d)
		{
			if (d.HasValue) return Math.Round(d.Value, 2);
			return d;
		}
	}
}