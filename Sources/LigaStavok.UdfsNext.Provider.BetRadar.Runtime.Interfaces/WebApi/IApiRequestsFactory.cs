using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Messages;

namespace Udfs.BetradarUnifiedFeed.Plugin.Abstractions
{
    public interface IApiRequestsFactory 
    {
        ApiCommand CreateRequestMessage(IApiCommandCreateRequest requestCommand);
    }
}