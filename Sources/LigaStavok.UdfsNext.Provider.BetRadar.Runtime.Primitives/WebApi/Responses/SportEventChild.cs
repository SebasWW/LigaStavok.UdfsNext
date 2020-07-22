using System;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class SportEventChild
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? Scheduled { get; set; }

        public DateTimeOffset? ScheduledEnd { get; set; }

        public bool? StartTimeTbd { get; set; }

        public string Type { get; set; }

        public static SportEventChild Parse(dynamic dynamicXml)
        {
            var sportEventBuilder = new SportEventChild
            {
                Id           = dynamicXml.Id<string>(),
                Name         = dynamicXml.Name<string>(),
                Scheduled    = dynamicXml.Scheduled<DateTimeOffset?>(),
                ScheduledEnd = dynamicXml.ScheduledEnd<DateTimeOffset?>(),
                StartTimeTbd = dynamicXml.StartTimeTbd<bool?>(),
                Type         = dynamicXml.Type<string>()
            };

            return sportEventBuilder;
        }

        
        public static IEnumerable<SportEventChild> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<SportEventChild>();
        }
    }
}