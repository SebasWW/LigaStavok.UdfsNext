using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class MarketDescriptionOutcome
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public static MarketDescriptionOutcome Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new MarketDescriptionOutcome
            {
                Id   = dynamicXml.Id,
                Name = dynamicXml.Name
            };

            return builder;
        }

        public static IEnumerable<MarketDescriptionOutcome> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<MarketDescriptionOutcome>();
        }
    }
}