using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class OddsChangeStatistics
    {
        public int AwayCorners { get; set; }

        public int AwayRedCards { get; set; }

        public int AwayYellowCards { get; set; }

        public int AwayYellowRedCards { get; set; }

        public int HomeCorners { get; set; }

        public int HomeRedCards { get; set; }

        public int HomeYellowCards { get; set; }

        public int HomeYellowRedCards { get; set; }

        public static OddsChangeStatistics Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }
            
            var statisticsBuilder = new OddsChangeStatistics
            {
                AwayYellowCards    = dynamicXml.yellowCards.Away<int>(),
                HomeYellowCards    = dynamicXml.yellowCards.Home<int>(),
                AwayYellowRedCards = dynamicXml.yellowRedCards.Away<int>(),
                HomeYellowRedCards = dynamicXml.yellowRedCards.Home<int>(),
                AwayRedCards       = dynamicXml.redCards.Away<int>(),
                HomeRedCards       = dynamicXml.redCards.Home<int>(),
                AwayCorners        = dynamicXml.corners.Away<int>(),
                HomeCorners        = dynamicXml.corners.Home<int>()
            };

            return statisticsBuilder;
        }
    }
}