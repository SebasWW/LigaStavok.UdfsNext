namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestTournamentInfo : ApiCommandRequest
    {
        public Lang Language { get; set; }

        public string TournamentId { get; set; }
    }
}