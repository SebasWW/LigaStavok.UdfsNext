using System;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Season
    {
        public DateTimeOffset? EndDate { get; set; }

        public string Id { get; set; }
        
        public string Name { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public string TournamentId { get; set; }

        public string Year { get; set; }
        
        public static Season Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Season
            {
                EndDate      = dynamicXml.EndDate<DateTimeOffset?>(),
                Id           = dynamicXml.Id,
                Name         = dynamicXml.Name,
                StartDate    = dynamicXml.StartDate<DateTimeOffset?>(),
                TournamentId = dynamicXml.TournamentId,
                Year         = dynamicXml.Year
            };

            return builder;
        }
    }
}