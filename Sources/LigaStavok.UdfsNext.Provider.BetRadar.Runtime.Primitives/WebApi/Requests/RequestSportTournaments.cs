namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestSportTournaments : ApiCommandRequest
    {
        public Language Language { get; set; }

        public string SportId { get; set; }
    }
}