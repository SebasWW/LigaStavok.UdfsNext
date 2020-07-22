namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestOddsRecovery : IApiCommandCreateRequest
    {

        public string EventId { get; set; }

        public bool? FullRecovery { get; set; }

        public ProductType ProductType { get; set; }

        public long? RecoverAfter { get; set; }

        public int RequestId { get; set; }
    }
}