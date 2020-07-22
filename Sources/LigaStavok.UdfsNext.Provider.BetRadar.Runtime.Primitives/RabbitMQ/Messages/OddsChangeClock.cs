using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public sealed class OddsChangeClock
    {
        public string MatchTime { get; set; }

        public string RemainingTime { get; set; }

        public string RemainingTimeInPeriod { get; set; }

        public string StoppageTime { get; set; }

        public string StoppageTimeAnnounced { get; set; }

        public bool? Stopped { get; set; }

        public static OddsChangeClock Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new OddsChangeClock
            {
                MatchTime             = dynamicXml.MatchTime,
                StoppageTime          = dynamicXml.StoppageTime,
                StoppageTimeAnnounced = dynamicXml.StoppageTimeAnnounced,
                RemainingTime         = dynamicXml.RemainingTime,
                RemainingTimeInPeriod = dynamicXml.RemainingTimeInPeriod,
                Stopped               = dynamicXml.Stopped<bool?>()
            };

            return builder;
        }
    }
}