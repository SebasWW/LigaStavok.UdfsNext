using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests
{
	public class TranslationsRequest : WebApiRequest
    {
        public const string SourceValue = "Translations";

        public DateTimeOffset? FromISO8601 { get; set; }
        public DateTimeOffset? ToISO8601 { get; set; }
        public int? SportId { get; set; }
        public int? TournamentId { get; set; }
        public string Booking { get; set; }
        public int? Length { get; set; }
        public int? Start { get; set; }
        public string Order_dir { get; set; }
        public int[] StateIds { get; set; }
    }
}
