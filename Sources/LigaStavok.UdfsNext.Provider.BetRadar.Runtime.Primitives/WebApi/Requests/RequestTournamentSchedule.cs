namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestTournamentSchedule : ApiCommandRequest
    {
        public Lang Language { get; set; }

        public string TournamentId { get; set; }
    }
}