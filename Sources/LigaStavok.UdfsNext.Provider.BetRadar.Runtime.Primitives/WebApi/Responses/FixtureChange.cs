using System;
using System.Collections.Generic;
using System.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
	public partial class FixtureChange
    {       
        public string SportEventId { get; set; }

        public DateTimeOffset UpdateTime { get; set; }
   
        public static FixtureChange Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var fixtureChangeBuilder = new FixtureChange
            {
                SportEventId = dynamicXml.SportEventId,
                UpdateTime   = dynamicXml.UpdateTime<DateTimeOffset>()
            };

            return fixtureChangeBuilder;
        }

        public static IEnumerable<FixtureChange> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<FixtureChange>();
        }
    }
}