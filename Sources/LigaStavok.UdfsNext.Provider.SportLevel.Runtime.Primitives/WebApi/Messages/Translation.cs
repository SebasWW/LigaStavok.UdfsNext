using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages
{
	public class Translation
	{
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("start_iso8601")]
        public DateTimeOffset StartIso8601 { get; set; }

        [JsonProperty("finish_iso8601")]
        public DateTimeOffset FinishIso8601 { get; set; }

        [JsonProperty("tournament_id")]
        public long TournamentId { get; set; }

        [JsonProperty("sport_id")]
        public long SportId { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("allowed_countries")]
        public string[] AllowedCountries { get; set; }

        [JsonProperty("match_format")]
        public MatchFormats MatchFormat { get; set; }

        [JsonProperty("data_source_type")]
        public string DataSourceType { get; set; }

        [JsonProperty("coverage_id")]
        public long? CoverageId { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

		[JsonProperty("title_ru")]
        public string TitleRu { get; set; }

        [JsonProperty("title_en")]
        public string TitleEn { get; set; }

        [JsonProperty("home_team_id")]
        public long HomeTeamId { get; set; }

        [JsonProperty("home_team_title_ru")]
        public string HomeTeamTitleRu { get; set; }

        [JsonProperty("home_team_title_en")]
        public string HomeTeamTitleEn { get; set; }

        [JsonProperty("home_team_region_ru")]
        public string HomeTeamRegionRu { get; set; }

        [JsonProperty("home_team_region_en")]
        public string HomeTeamRegionEn { get; set; }

        [JsonProperty("home_team_short_title_ru")]
        public string HomeTeamShortTitleRu { get; set; }

        [JsonProperty("home_team_short_title_en")]
        public string HomeTeamShortTitleEn { get; set; }

        [JsonProperty("home_team_title")]
        public string HomeTeamTitle { get; set; }

        [JsonProperty("home_team_region")]
        public string HomeTeamRegion { get; set; }

        [JsonProperty("home_team_short_title")]
        public string HomeTeamShortTitle { get; set; }

        [JsonProperty("home_team_age")]
        public string HomeTeamAge { get; set; }

        [JsonProperty("away_team_id")]
        public long AwayTeamId { get; set; }

        [JsonProperty("away_team_title_ru")]
        public string AwayTeamTitleRu { get; set; }

        [JsonProperty("away_team_title_en")]
        public string AwayTeamTitleEn { get; set; }

        [JsonProperty("away_team_region_ru")]
        public string AwayTeamRegionRu { get; set; }

        [JsonProperty("away_team_region_en")]
        public string AwayTeamRegionEn { get; set; }

        [JsonProperty("away_team_short_title_ru")]
        public string AwayTeamShortTitleRu { get; set; }

        [JsonProperty("away_team_short_title_en")]
        public string AwayTeamShortTitleEn { get; set; }

        [JsonProperty("away_team_title")]
        public string AwayTeamTitle { get; set; }

        [JsonProperty("away_team_region")]
        public string AwayTeamRegion { get; set; }

        [JsonProperty("away_team_short_title")]
        public string AwayTeamShortTitle { get; set; }

        [JsonProperty("away_team_age")]
        public string AwayTeamAge { get; set; }

        [JsonProperty("tournament_title_ru")]
        public string TournamentTitleRu { get; set; }

        [JsonProperty("tournament_title_en")]
        public string TournamentTitleEn { get; set; }

        [JsonProperty("tournament_short_title_ru")]
        public string TournamentShortTitleRu { get; set; }

        [JsonProperty("tournament_short_title_en")]
        public string TournamentShortTitleEn { get; set; }

        [JsonProperty("tournament_title")]
        public string TournamentTitle { get; set; }

        [JsonProperty("tournament_short_title")]
        public string TournamentShortTitle { get; set; }

        [JsonProperty("tournament_gender")]
        public string TournamentGender { get; set; }

        [JsonProperty("tournament_age_title")]
        public string TournamentAgeTitle { get; set; }

        [JsonProperty("tournament_country_title_ru")]
        public string TournamentCountryTitleRu { get; set; }

        [JsonProperty("tournament_country_title_en")]
        public string TournamentCountryTitleEn { get; set; }

        [JsonProperty("tournament_country_code")]
        public string TournamentCountryCode { get; set; }

        [JsonProperty("tournament_country_title")]
        public string TournamentCountryTitle { get; set; }

        [JsonProperty("location_city_title_ru")]
        public string LocationCityTitleRu { get; set; }

        [JsonProperty("location_city_title_en")]
        public string LocationCityTitleEn { get; set; }

        [JsonProperty("location_country_title_ru")]
        public object LocationCountryTitleRu { get; set; }

        [JsonProperty("location_country_title_en")]
        public string LocationCountryTitleEn { get; set; }

        [JsonProperty("location_city_title")]
        public string LocationCityTitle { get; set; }

        [JsonProperty("location_country_code")]
        public string LocationCountryCode { get; set; }

        [JsonProperty("location_country_title")]
        public string LocationCountryTitle { get; set; }

        [JsonProperty("booked_data")]
        public bool? BookedData { get; set; }

        [JsonProperty("booked_video")]
        public bool? BookedVideo { get; set; }

        [JsonProperty("booked_vt")]
        public bool? BookedVt { get; set; }

        [JsonProperty("booked_market")]
        public bool? BookedMarket { get; set; }

        [JsonProperty("non_invoice_data")]
        public bool NonInvoiceData { get; set; }

        [JsonProperty("non_invoice_video")]
        public bool NonInvoiceVideo { get; set; }

        [JsonProperty("non_invoice_vt")]
        public bool NonInvoiceVt { get; set; }

        [JsonProperty("non_invoice_market")]
        public bool NonInvoiceMarket { get; set; }

        [JsonProperty("planned_data")]
        public bool PlannedData { get; set; }

        [JsonProperty("planned_video")]
        public bool PlannedVideo { get; set; }

        [JsonProperty("planned_vt")]
        public bool PlannedVt { get; set; }

        [JsonProperty("planned_market")]
        public bool PlannedMarket { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("score_home")]
        public long? ScoreHome { get; set; }

        [JsonProperty("score_away")]
        public long? ScoreAway { get; set; }

        [JsonProperty("stage_type_id")]
        public long? StageTypeId { get; set; }

    }
}
