using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Schedule
    {
        public DateTimeOffset? GeneratedOn { get; set; }

        public IEnumerable<SportEvent> SportEvents { get; set; }



        public static Schedule Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Schedule
            {
                GeneratedOn = dynamicXml.GeneratedAt<DateTimeOffset>(),
                SportEvents = SportEvent.ParseList(dynamicXml.GetSportEventList())
            };

            return builder;
        }
    }
}