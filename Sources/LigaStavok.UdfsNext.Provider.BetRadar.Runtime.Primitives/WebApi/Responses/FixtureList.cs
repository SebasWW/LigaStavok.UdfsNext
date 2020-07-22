using System;
using System.Collections.Generic;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
	public partial class FixtureList
    {
        public IEnumerable<Fixture> Fixtures { get; set; }

        public DateTimeOffset? GeneratedOn { get; set; }

        public static FixtureList Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var messageBuilder = new FixtureList
            {
                Fixtures    = Fixture.ParseList(dynamicXml.GetFixtureList()),
                GeneratedOn = dynamicXml.GeneratedAt<DateTimeOffset?>(),
            };

            return messageBuilder;
        }
    }
}