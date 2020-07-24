using System;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestFixtureChanges : IApiCommandCreateRequest
    {
        public Lang Language { get; set; }

        public DateTime? AfterDateTime { get; set; }
    }
}