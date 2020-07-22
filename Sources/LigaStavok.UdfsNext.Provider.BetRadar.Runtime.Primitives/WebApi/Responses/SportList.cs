using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class SportList
    {
        public DateTimeOffset? GeneratedOn { get; set; }

        public IEnumerable<Sport> Sports { get; set; }

        public static SportList Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new SportList
            {
                GeneratedOn = dynamicXml.GeneratedAt<DateTimeOffset>(),
                Sports      = Sport.ParseList(dynamicXml.GetSportList())
            };

            return builder;
        }
    }
}