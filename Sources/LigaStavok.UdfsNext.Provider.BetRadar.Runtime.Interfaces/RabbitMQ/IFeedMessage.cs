namespace Udfs.BetradarUnifiedFeed.Plugin.Abstractions
{
    public interface IFeedMessage
    {
        ProductType Product { get; }
        string GetEventId();
    }
}