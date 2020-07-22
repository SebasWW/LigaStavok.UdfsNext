using System;
using System.Collections.Generic;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
	public partial class FixtureChangeList
    {
        public IEnumerable<FixtureChange> FixtureChanges { get; set; }

        public DateTimeOffset? GeneratedOn { get; set; }

        public static FixtureChangeList Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new FixtureChangeList
            {
                FixtureChanges = FixtureChange.ParseList(dynamicXml.GetFixtureChangeList()),
                GeneratedOn    = dynamicXml.GeneratedAt<DateTimeOffset?>(),
            };

            return builder;
        }
    }
}