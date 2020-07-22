namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestCompetitorProfile : ApiCommandRequest
    {
        public string CompetitorId { get; set; }

        public Language Language { get; set; }
    }
}