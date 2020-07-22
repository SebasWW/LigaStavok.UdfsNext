using System;
using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
    public sealed class RequestSchedule : IApiCommandCreateRequest
    {
        public DateTimeOffset Date  { get; set; }

        public Language Language { get; set; }

        public ProductType ProductType { get; set; }
    }
}