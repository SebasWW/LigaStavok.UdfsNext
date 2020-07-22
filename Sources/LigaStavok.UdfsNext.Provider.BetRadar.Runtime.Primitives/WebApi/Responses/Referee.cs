using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Referee
    {       
        public string Id { get; set; }

        public string Name { get; set; }

        public string Nationality { get; set; }

        public static Referee Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Referee
            {
                Id          = dynamicXml.Id,
                Name        = dynamicXml.Name,
                Nationality = dynamicXml.Nationality
            };

            return builder;
        }
    }
}