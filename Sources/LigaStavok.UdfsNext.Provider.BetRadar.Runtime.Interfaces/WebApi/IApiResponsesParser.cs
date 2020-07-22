using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests;

namespace Udfs.BetradarUnifiedFeed.Plugin.Abstractions
{
    public interface IApiResponsesParser
    {
        IApiResponseParsingResult ParseResponse(ParseApiResponse message);
    }
}