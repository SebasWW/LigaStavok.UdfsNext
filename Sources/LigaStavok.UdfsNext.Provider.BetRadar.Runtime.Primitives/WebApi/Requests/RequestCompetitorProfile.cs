namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestCompetitorProfile : ApiCommandRequest
    {
        public string CompetitorId { get; set; }

        public Lang Language { get; set; }
    }
}