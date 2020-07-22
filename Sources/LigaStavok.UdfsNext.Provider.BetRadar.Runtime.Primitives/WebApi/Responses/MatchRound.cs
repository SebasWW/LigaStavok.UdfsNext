using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class MatchRound
    {
        public int? CupRoundMatches { get; set; }

        public int? CupRoundMatchNumber { get; set; }

        public string Group { get; set; }
        
        public string Name { get; set; }

        public int? Number { get; set; }

        public string OtherMatchId { get; set; }

        public string Type { get; set; }

        public static MatchRound Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new MatchRound
            {
                CupRoundMatches     = dynamicXml.CupRoundMatches<int?>(),
                CupRoundMatchNumber = dynamicXml.CupRoundMatchNumber<int?>(),
                Group               = dynamicXml.Group,
                Name                = dynamicXml.Name,
                Number              = dynamicXml.Number<int?>(),
                OtherMatchId        = dynamicXml.OtherMatchId,
                Type                = dynamicXml.Type
            };

            return builder;
        }
    }
}