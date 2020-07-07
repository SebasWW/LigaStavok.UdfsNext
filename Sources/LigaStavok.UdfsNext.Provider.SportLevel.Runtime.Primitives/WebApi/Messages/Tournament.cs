using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages
{
	public class Tournament
    {
        [JsonProperty("age_id")]
        public object AgeId { get; set; }

        [JsonProperty("gender_id")]
        public long GenderId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("sport_id")]
        public long SportId { get; set; }

        [JsonProperty("sport_title")]
        public string SportTitle { get; set; }

        [JsonProperty("sport_title_en")]
        public string SportTitleEn { get; set; }

        [JsonProperty("sport_title_ru")]
        public string SportTitleRu { get; set; }

        [JsonProperty("start_iso8601")]
        public object StartIso8601 { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_en")]
        public string TitleEn { get; set; }

        [JsonProperty("title_ru")]
        public string TitleRu { get; set; }
    }
}
