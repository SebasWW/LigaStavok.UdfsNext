using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Car
    {
        public string Chassis { get; set; }

        public string EngineName { get; set; }

        public string Name { get; set; }

        public static Car Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Car
            {
                Chassis    = dynamicXml.Chassis,
                EngineName = dynamicXml.EngineName,
                Name       = dynamicXml.Name
            };

            return builder;
        }
    }
}