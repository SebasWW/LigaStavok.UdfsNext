﻿using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests
{
	[ImmutableObject(true)]
    public sealed class RequestCreateSportCategories : ApiCommandRequest
    {

        public Lang Language { get; set; }

        public string SportId { get; set; }
    }
}