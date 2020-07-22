using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class MatchTimeline
    {
        public CoverageInfo CoverageInfo { get; set; }
        
        public DateTimeOffset? GeneratedOn { get; set; }

        public SportEvent SportEvent { get; set; }

        public SportEventConditions SportEventConditions { get; set; }

        public SportEventStatus SportEventStatus { get; set; }

        public IEnumerable<TimelineEvent> Timeline { get; set; }

        public static MatchTimeline Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new MatchTimeline
            {
                CoverageInfo         = Responses.CoverageInfo.Parse(dynamicXml.CoverageInfo),
                GeneratedOn          = dynamicXml.GeneratedAt<DateTimeOffset?>(),
                SportEvent           = Responses.SportEvent.Parse(dynamicXml.SportEvent),
                SportEventConditions = Responses.SportEventConditions.Parse(dynamicXml.SportEventConditions),
                SportEventStatus     = Responses.SportEventStatus.Parse(dynamicXml.SportEventStatus),
                Timeline             = TimelineEvent.ParseList(dynamicXml.Timeline?.GetEventList())
            };

            return builder;
        }
    }
}