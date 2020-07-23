using System.Collections.Generic;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Messages
{
    public interface IApiRequestsFactory 
    {
        ApiCommand CreateRequestMessage(IApiCommandCreateRequest requestCommand);
    }
}