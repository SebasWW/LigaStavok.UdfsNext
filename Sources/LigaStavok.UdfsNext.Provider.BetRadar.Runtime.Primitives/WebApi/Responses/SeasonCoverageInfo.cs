using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class SeasonCoverageInfo
    {
        public string MaxCoverageLevel { get; set; }

        public int? MaxCovered { get; set; }

        public string MinCoverageLevel { get; set; }
        
        public int Played { get; set; }

        public int Scheduled { get; set; }

        public string SeasonId { get; set; }

        public static SeasonCoverageInfo Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new SeasonCoverageInfo
            {
                MaxCoverageLevel = dynamicXml.MaxCoverageLevel,
                MaxCovered       = dynamicXml.MaxCovered<int?>(),
                MinCoverageLevel = dynamicXml.MinCoverageLevel,
                Played           = dynamicXml.Played<int>(),
                Scheduled        = dynamicXml.Scheduled<int>(),
                SeasonId         = dynamicXml.SeasonId
            };

            return builder;
        }
    }
}