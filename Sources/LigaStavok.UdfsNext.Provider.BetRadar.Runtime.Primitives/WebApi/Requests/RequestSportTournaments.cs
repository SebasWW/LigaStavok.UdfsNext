namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestSportTournaments : ApiCommandRequest
    {
        public Lang Language { get; set; }

        public string SportId { get; set; }
    }
}