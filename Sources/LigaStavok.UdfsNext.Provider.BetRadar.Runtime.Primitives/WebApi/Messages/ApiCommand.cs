using System;
using System.Collections.Generic;
using System.Net.Http;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Messages
{
	public sealed class ApiCommand
    {

		public string EventId { get; set; }

        public TimeSpan CachePeriod { get; set; }

        public Dictionary<string, string> CustomHeaders { get; set; }

        public Uri Endpoint { get; set; }

        public DateTimeOffset GeneratedOn { get; set; }

        public Guid RequestId { get; set; }

        public Lang Language { get; set; }

        public HttpMethod Method { get; set; }

        public bool IsRecovery { get; set; }

        public ProductType ProductType { get; set; }
    }
}