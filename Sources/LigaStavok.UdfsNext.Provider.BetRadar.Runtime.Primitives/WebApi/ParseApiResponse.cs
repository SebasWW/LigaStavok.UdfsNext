using System;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	public sealed class ParseApiResponse 
	{

		public string EventId { get;}

        public string Data { get; set; }

        public Lang Language { get; set; }

        public Guid RequestId { get; set; }

	}
}