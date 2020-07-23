namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class RequestPlayerProfileRequest : ApiCommandRequest
    {
        public Lang Language { get; set; }

        public string PlayerId { get; set; }
    }
}