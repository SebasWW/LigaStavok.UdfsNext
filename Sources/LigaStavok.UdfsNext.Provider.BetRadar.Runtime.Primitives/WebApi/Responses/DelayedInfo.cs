using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class DelayedInfo
    {
        public string Description { get; set; }

        public int Id { get; set; }

        public static DelayedInfo Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new DelayedInfo
            {
                Description = dynamicXml.Description,
                Id          = dynamicXml.Id<int>()
            };

            return builder;
        }
    }
}