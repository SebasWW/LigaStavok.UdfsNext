using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class TvChannel
    {
        public string Name { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public static TvChannel Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new TvChannel
            {
                Name      = dynamicXml.Name,
                StartTime = dynamicXml.StartTime<DateTimeOffset?>()
            };

            return builder;
        }

        public static IEnumerable<TvChannel> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<TvChannel>();
        }
    }
}