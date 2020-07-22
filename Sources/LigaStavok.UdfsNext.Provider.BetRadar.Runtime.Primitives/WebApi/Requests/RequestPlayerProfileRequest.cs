namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestPlayerProfileRequest : ApiCommandRequest
    {
        public Language Language { get; set; }

        public string PlayerId { get; set; }
    }
}