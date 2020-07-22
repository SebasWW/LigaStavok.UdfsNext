using System;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class MatchSummary
    {
        public CoverageInfo CoverageInfo { get; set; }

        public DateTimeOffset? GeneratedOn { get; set; }

        public SportEvent SportEvent { get; set; }

        public SportEventConditions SportEventConditions { get; set; }

        public SportEventStatus SportEventStatus { get; set; }

        public Venue Venue { get; set; }

        public static MatchSummary Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new MatchSummary
            {
                CoverageInfo         = Responses.CoverageInfo.Parse(dynamicXml.CoverageInfo),
                GeneratedOn          = dynamicXml.GeneratedAt<DateTimeOffset?>(),
                SportEvent           = Responses.SportEvent.Parse(dynamicXml.SportEvent),
                SportEventConditions = Responses.SportEventConditions.Parse(dynamicXml.SportEventConditions),
                SportEventStatus     = Responses.SportEventStatus.Parse(dynamicXml.SportEventStatus),
                Venue                = Responses.Venue.Parse(dynamicXml.Venue),
            };

            return builder;
        }
    }
}