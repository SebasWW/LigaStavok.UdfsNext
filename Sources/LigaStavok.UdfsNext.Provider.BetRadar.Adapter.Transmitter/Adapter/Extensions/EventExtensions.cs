namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions
{
    public static class EventExtensions
    {
        public static string ToTransmitterEventId(this string eventId)
        {
            return string.IsNullOrEmpty(eventId) ? eventId : RemovePrefix(eventId, "sr:match:");
        }

        public static string RemovePrefix(string source, string remove)
        {
            var index = source.IndexOf(remove);
            return index < 0 ? source : source.Remove(index, remove.Length);
        }
    }
}
