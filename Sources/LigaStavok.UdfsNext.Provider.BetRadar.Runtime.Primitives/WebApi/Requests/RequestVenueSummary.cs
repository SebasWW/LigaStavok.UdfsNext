namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestVenueSummary : ApiCommandRequest
    {
        public Lang Language { get; set; }

        public string VenueId { get; set; }
    }
}