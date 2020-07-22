using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Team
    {
        public string Abbreviation { get; set; }

        public Category Category { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }
        
        public string Id { get; set; }

        public string Name { get; set; }

        public string Qualifier { get; set; }

        public Dictionary<string, string> ReferenceIds { get; set; }

        public Sport Sport { get; set; }

        public bool? Virtual { get; set; }

        public static Team Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Team
            {
                Abbreviation = dynamicXml.Abbreviation,
                Category     = Responses.Category.Parse(dynamicXml.Category),
                Country      = dynamicXml.Country,
                CountryCode  = dynamicXml.CountryCode,
                Id           = dynamicXml.Id,
                Name         = dynamicXml.Name,
                Qualifier    = dynamicXml.Qualifier,
                ReferenceIds = Responses.ReferenceIds.Parse(dynamicXml.ReferenceIds),
                Sport        = Responses.Sport.Parse(dynamicXml.Sport),
                Virtual      = dynamicXml.Virtual<bool?>()
            };

            return builder;
        }

        public static IEnumerable<Team> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray() 
                ?? Array.Empty<Team>();
        }
    }
}