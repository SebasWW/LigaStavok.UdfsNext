using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Venue
    {
        public int? Capacity { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public string Id { get; set; }

        public string MapCoordinates { get; set; }
        
        public string Name { get; set; }

        public static Venue Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Venue
            {
                Capacity       = dynamicXml.Capacity<int?>(),
                CityName       = dynamicXml.CityName,
                CountryName    = dynamicXml.CountryName,
                CountryCode    = dynamicXml.CountryCode,
                Id             = dynamicXml.Id,
                MapCoordinates = dynamicXml.MapCoordinates,
                Name           = dynamicXml.Name
            };

            return builder;
        }
    }
}