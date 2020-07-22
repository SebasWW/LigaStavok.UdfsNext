namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestTournamentInfo : ApiCommandRequest
    {
        public Language Language { get; set; }

        public string TournamentId { get; set; }
    }
}