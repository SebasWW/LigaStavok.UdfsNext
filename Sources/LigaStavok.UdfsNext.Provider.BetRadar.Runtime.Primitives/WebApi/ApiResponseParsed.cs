using System;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
	public sealed class ApiResponseParsed : IApiResponseParsingResult
    {
		public string EventId { get; set; }

        public Lang Language { get; set; }

        public object Response { get; set; }

        public Guid RequestId { get; set; }

        public ProductType ProductType { get; set; }

        //public bool CanBeAdapted()
        //{
        //    return !(Response is Api.Responses.Response);
        //}
    }
}