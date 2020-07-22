namespace Udfs.BetradarUnifiedFeed.Plugin.Abstractions
{
    public interface IMarket
    {
        int Id { get; set; }

        string Specifiers { get; set; }
    }
}