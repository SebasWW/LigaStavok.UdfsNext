using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class RaceDriver
    {
        
        public string Abbreviation { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string DateOfBirth { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Nationality { get; set; }

        public Dictionary<string, string> ReferenceIds { get; set; }

        public bool? Virtual { get; set; }

        public static RaceDriver Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new RaceDriver
            {
                Abbreviation = dynamicXml.Abbreviation,
                Country      = dynamicXml.Country,
                CountryCode  = dynamicXml.CountryCode,
                DateOfBirth  = dynamicXml.DateOfBirth,
                Id           = dynamicXml.Id,
                Name         = dynamicXml.Name,
                Nationality  = dynamicXml.Nationality,
                ReferenceIds = Responses.ReferenceIds.Parse(dynamicXml.ReferenceIds),
                Virtual      = dynamicXml.Virtual<bool?>()
            };

            return builder;
        }
    }
}