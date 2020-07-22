namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestMarketDescriptions : ApiCommandRequest
    {
        public bool IncludeMappins { get; set; }

        public Language Language { get; set; }
    }
}