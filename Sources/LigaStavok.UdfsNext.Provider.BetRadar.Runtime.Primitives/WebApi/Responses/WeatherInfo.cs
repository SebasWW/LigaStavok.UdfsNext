using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class WeatherInfo
    {
        public string Pitch { get; set; }

        public int? TemperatureCelsius { get; set; }

        public string WeatherConditions { get; set; }

        public string Wind { get; set; }

        public string WindAdvantage { get; set; }

        public static WeatherInfo Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new WeatherInfo
            {
                Pitch              = dynamicXml.Pitch,
                TemperatureCelsius = dynamicXml.TemperatureCelsius<int?>(),
                WeatherConditions  = dynamicXml.WeatherConditions,
                Wind               = dynamicXml.Wind,
                WindAdvantage      = dynamicXml.WindAdvantage
            };

            return builder;
        }
    }
}