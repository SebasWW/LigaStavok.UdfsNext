using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions
{
    public static class OutcomeExtensions
    {
        public static string GetUniqueId(this IOutcome outcome, string marketUniqueId)
        {
            return $"{marketUniqueId}:{outcome.Id}";
        }
    }
}