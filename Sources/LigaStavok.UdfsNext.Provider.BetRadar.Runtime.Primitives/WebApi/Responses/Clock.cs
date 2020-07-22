using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Clock
    {
        
        public string MatchTime { get; set; }

        public string StoppageTime { get; set; }

        public string StoppageTimeAnnounced { get; set; }

        public static Clock Parse(dynamic clock)
        {
            if (clock == null)
            {
                return null;
            }

            var builder = new Clock
            {
                MatchTime             = clock.MatchTime,
                StoppageTime          = clock.StoppageTime,
                StoppageTimeAnnounced = clock.StoppageTimeAnnounced
            };

            return builder;
        }
    }
}