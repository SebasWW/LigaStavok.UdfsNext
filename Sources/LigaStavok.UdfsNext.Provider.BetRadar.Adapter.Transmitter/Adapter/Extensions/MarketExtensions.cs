using System.Text.RegularExpressions;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;
using Udfs.Transmitter.DSL.SpecifierDescription;
using Udfs.Transmitter.Messages.Identifiers;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions
{
    public static class MarketExtensions
    {
        public static string GetUniqueId(this IMarket market, LineService lineService, string eventId)
        {
            return string.IsNullOrWhiteSpace(market.Specifiers)
                ? $"{(int)lineService}:{eventId}:{market.Id}"
                : $"{(int)lineService}:{eventId}:{market.Id}:{market.Specifiers}";
        }

        private static readonly Regex CompetitorIdRegex = new Regex(@"variant=pre:playerprops:\d+:(?<cmpId>\d+)",RegexOptions.Compiled);

        public static SpecifierCollection ExtractSpecifiers(this IMarket market)
        {
            var marketSpecifiers = market.Specifiers ?? string.Empty;

            var competitorIdMatch = CompetitorIdRegex.Match(marketSpecifiers);
            if (competitorIdMatch.Success)
            {
                var competitorId = competitorIdMatch.Groups["cmpId"].Value;
                marketSpecifiers = $"{marketSpecifiers}|{SpecifierKey.CompetitorId.Value}={competitorId}";
            }

            SpecifierCollection.TryParse(marketSpecifiers, out SpecifierCollection specifiers);

            return specifiers;
        }
    }
}