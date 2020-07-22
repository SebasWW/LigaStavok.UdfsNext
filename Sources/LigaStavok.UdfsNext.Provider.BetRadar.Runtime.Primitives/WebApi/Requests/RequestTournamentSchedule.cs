namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestTournamentSchedule : ApiCommandRequest
    {
        public Language Language { get; set; }

        public string TournamentId { get; set; }
    }
}