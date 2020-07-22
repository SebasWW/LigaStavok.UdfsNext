using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Player
    {
        public string CountryCode { get; set; }

        public string DateOfBirth { get; set; }

        public int? Height { get; set; }

        public string Id { get; set; }

        public int? JerseyNumber { get; set; }

        public string Name { get; set; }

        public string Nationality { get; set; }

        public string Position { get; set; }

        public string Type { get; set; }

        public int? Weight { get; set; }

        public static Player Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Player
            {
                CountryCode  = dynamicXml.CountryCode,
                DateOfBirth  = dynamicXml.DateOfBirth,
                Height       = dynamicXml.Height<int?>(),
                Id           = dynamicXml.Id,
                JerseyNumber = dynamicXml.JerseyNumber<int?>(),
                Name         = dynamicXml.Name,
                Nationality  = dynamicXml.Nationality,
                Position     = dynamicXml.Position,
                Type         = dynamicXml.Type,
                Weight       = dynamicXml.Weight<int?>()
            };

            return builder;
        }

        public static IEnumerable<Player> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<Player>();
        }
    }
}