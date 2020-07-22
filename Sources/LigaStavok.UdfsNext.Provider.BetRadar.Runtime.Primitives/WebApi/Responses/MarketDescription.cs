using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class MarketDescription
    {
        public string Description { get; set; }

        public string Groups { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<MarketDescriptionOutcome> Outcomes { get; set; }
        
        public string Variant { get; set; }

        public static MarketDescription Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new MarketDescription
            {
                Description = dynamicXml.Description,
                Groups      = dynamicXml.Groups,
                Id          = dynamicXml.Id<int>(),
                Name        = dynamicXml.Name,
                Outcomes    = MarketDescriptionOutcome.ParseList(dynamicXml.Outcomes?.GetOutcomeList()),
                Variant     = dynamicXml.Variant 
            };

            return builder;
        }

        public static IEnumerable<MarketDescription> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<MarketDescription>();
        }
    }
}