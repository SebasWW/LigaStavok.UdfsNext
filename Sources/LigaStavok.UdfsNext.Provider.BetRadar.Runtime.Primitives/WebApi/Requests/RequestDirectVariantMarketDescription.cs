namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestDirectVariantMarketDescription : ApiCommandRequest
    {
        public Language Language { get; set; }

        public int MarketId { get; set; }

        public string Variant { get; set; }
    }
}