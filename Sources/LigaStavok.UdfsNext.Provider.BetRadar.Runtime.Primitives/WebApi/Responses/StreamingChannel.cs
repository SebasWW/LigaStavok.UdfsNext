using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class StreamingChannel
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        public static StreamingChannel Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new StreamingChannel
            {
                Id   = dynamicXml.Id,
                Name = dynamicXml.Name
            };

            return builder;
        }

        public static IEnumerable<StreamingChannel> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<StreamingChannel>();
        }
    }
}