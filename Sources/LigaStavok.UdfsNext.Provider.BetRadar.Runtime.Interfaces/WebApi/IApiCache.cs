using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Messages;

namespace Udfs.BetradarUnifiedFeed.Plugin.Abstractions
{
    public interface IApiCache
    {
        void Add(ApiCommand message);

        bool Contains(ApiCommand message);
    }
}