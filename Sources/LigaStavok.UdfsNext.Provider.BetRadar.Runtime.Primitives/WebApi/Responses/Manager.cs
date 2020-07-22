namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
	public partial class Manager
    {
        public string CountryCode { get; set; }

        public string Id { get; set; }
        
        public string Name { get; set; }

        public string Nationality { get; set; }

        public static Manager Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Manager
            {
                CountryCode = dynamicXml.CountryCode,
                Id          = dynamicXml.Id,
                Name        = dynamicXml.Name,
                Nationality = dynamicXml.Nationality
            };

            return builder;
        }
    }
}