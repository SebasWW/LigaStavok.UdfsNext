using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class TimelineEventPlayer
    {       
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
        
        public static TimelineEventPlayer Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new TimelineEventPlayer
            {
                Id   = dynamicXml.Id,
                Name = dynamicXml.Name,
                Type = dynamicXml.Type
            };

            return builder;
        }

        public static IEnumerable<TimelineEventPlayer> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<TimelineEventPlayer>();
        }
    }
}