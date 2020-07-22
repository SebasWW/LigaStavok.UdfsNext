using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class SportEventConditions
    {
        public string Attendance { get; set; }

        public string MatchMode { get; set; }

        public Referee Referee { get; set; }

        public Venue Venue { get; set; }

        public WeatherInfo WeatherInfo { get; set; }

        public static SportEventConditions Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new SportEventConditions
            {
                Attendance  = dynamicXml.Attendance,
                MatchMode   = dynamicXml.MatchMode,
                Referee     = Responses.Referee.Parse(dynamicXml.Referee),
                Venue       = Responses.Venue.Parse(dynamicXml.Venue),
                WeatherInfo = Responses.WeatherInfo.Parse(dynamicXml.WeatherInfo),
            };

            return builder;
        }
    }
}