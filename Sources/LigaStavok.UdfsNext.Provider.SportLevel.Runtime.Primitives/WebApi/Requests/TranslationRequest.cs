using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests
{
	public class TranslationRequest : WebApiRequest
    {
        public const string SourceValue = "Translation";

        public long Id { get; set; }
    }
}
